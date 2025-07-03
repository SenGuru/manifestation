using System;
using System.Linq;
using System.Threading.Tasks;
using Ben.Diagnostics;
using FluentScheduler;
using Framework.Data.Migrations;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Exceptions;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Web.Filters;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.DI;
using Manifest.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Config;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Helpers;
using System.Text.Json;
using Manifest.Service.App;
using MassTransit;
using Manifest.SharedApp;
//using Microsoft.AspNetCore.Authentication.Cookies;

namespace Manifest.Api.All
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            services.AddApplicationDI();

            services.AddLettuceEncrypt();

            services.AddHostedService<ScheduledJobInitializerService>();

            services.AddLettuceEncrypt();

            /*
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             .AddCookie()
             .AddOpenIdConnect("AzureAD", "Azure AD", options =>
             {
                 var config = services.BuildServiceProvider().GetService<ICommonConfiguration>();
                 options.ClientId = config.SingleSignOnClientId;
                 options.ClientSecret = config.SingleSignOnClientSecret;
                 options.Authority = $"https://login.microsoftonline.com/{config.SingleSignOnAuthority}";
                 options.CallbackPath = "/api/account/azureADCallback";
             });
            */
            if (string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"), "true", StringComparison.OrdinalIgnoreCase))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default
                    // Clear that restriction because forwarders are enabled by explicit
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var log = actionContext.HttpContext.RequestServices.GetService<ILog>();

                    var lstModelErrors = (from modelStateValues in actionContext.ModelState.Values
                                          from error in modelStateValues.Errors
                                          from modelStateKey in actionContext.ModelState.Keys
                                          select new ReturnErrorItem(modelStateKey, error.ErrorMessage)).ToList();
                    var result = log.ErrorAndReturnModel<object>("Validation error occurred", null, lstModelErrors, 400);
                    return new BadRequestObjectResult(result);
                };
            });

            //services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            //services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //});

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        //.AllowCredentials()
                    });
            });

            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
                x.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddMvc(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                foreach (var formatter in options.InputFormatters)
                {
                    if (formatter.GetType() == typeof(SystemTextJsonInputFormatter))
                        ((SystemTextJsonInputFormatter)formatter).SupportedMediaTypes.Add(
                            Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/plain"));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new Int32NullConverter());
                options.JsonSerializerOptions.Converters.Add(new Int64NullConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manifest API", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT" });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
                        },
                        Array.Empty<string>()
                    }
                });

                c.EnableAnnotations();
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RealtimeMessageNotificationConsumer>(typeof(RealtimeMessageConsumerDefinition));
                x.AddConsumer<JobServerNotificationConsumer>(typeof(JobServerConsumerDefinition));
                x.AddConsumer<MessageSenderNotificationConsumer>(typeof(MessageSenderConsumerDefinition));

                var env = services.BuildServiceProvider().GetService<IHostEnvironment>();
                if (env != null && env.IsEnvironment("Testing"))
                {
                    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
                    return;
                }

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
                var commonConfiguration = services.BuildServiceProvider().GetService<ICommonConfiguration>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

                if (!commonConfiguration.IsDedicatedQueue)
                {
                    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
                }
                else
                {
                    x.UsingActiveMq((context, cfg) =>
                    {
                        cfg.Host(commonConfiguration.ActiveMQHost, h =>
                        {
                            // h.UseSsl();
                            h.Username(commonConfiguration.ActiveMQUsername);
                            h.Password(commonConfiguration.ActiveMQPassword);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                }
            });

            services.AddSignalR();

        }

        public void ConfigureTesting(IServiceScopeFactory scopeFactory, IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IBaseDbConfiguration config, ILog log, IDBMigration migration, IAppDatabaseUtils appDatabaseUtils)
        {
            if (appDatabaseUtils.IsDatabaseNeedToBeRecreated())
            {
                appDatabaseUtils.DeleteDatabaseAsync().Wait();
                appDatabaseUtils.CreateDatabaseAsync().Wait();
            }

            migration.SetApplicationContext("Test");
            Configure(scopeFactory, app, env, loggerFactory, config, log, migration);
        }

        public void Configure(IServiceScopeFactory scopeFactory, IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IBaseDbConfiguration config, ILog log, IDBMigration migration)
        {
            app.UseResponseCompression();

            loggerFactory.AddFrameworkLogger(config, log);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<PerformanceLogMiddleware>();

            app.UseMiddleware<CorrelationIdHeaderMiddleware>();

            app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manifest API V1");
            });

            //explicitly only use local files when the path doesn't start with api
            app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"), file =>
            {
                file.UseRouting();

                file.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                file.UseAuthorization();

                file.UseAuthentication();

                file.UseStaticFiles();

                file.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToFile("index.html");
                    endpoints.MapHub<RealtimeMessageNotificationHub>("/realtime-messages");
                });
            });

            //explicitly map api endpoints only when path starts with api
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), api =>
            {
                api.UseRouting();

                api.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                api.UseAuthorization();

                api.UseAuthentication();

                api.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapDefaultControllerRoute();
                });
            });

            if (!migration.IsMigrationUptoDate(migration.GetMigrationAssembly()))
            {
                migration.MigrateToLatestVersion(migration.GetMigrationAssembly());
            }

            app.UseBlockingDetection();
        }
    }
}
