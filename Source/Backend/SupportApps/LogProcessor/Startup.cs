using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using LogProcessor.Services;
using System;

public class Startup
{
    // private readonly ConfigurationService _configuration;
    public Startup() {}
    public void ConfigureServices(IServiceCollection services)
    {
        var isClient = Environment.GetCommandLineArgs().IsParamAvailable("--client");
        var isServer = Environment.GetCommandLineArgs().IsParamAvailable("--server");

        if (!isClient && !isServer)
        {
            Console.WriteLine("Please specify if this is a client or server instance using --client or --server");
            Environment.Exit(1);
        }

        if (isServer)
        {
            services.AddControllers();
        }

        services.AddLogging();
        services.AddSingleton<ConfigurationService>();
        services.AddSingleton<LogSettingRepository>();
        services.AddSingleton<PerfLogSettingRepository>();
        services.AddSingleton<ILogRepository>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<ConfigurationService>();
            if (config.UsePostgresDb)
                return new PostgresDbLogRepository(config);
            return new LiteDbLogRepository(config);
        });

        services.AddSingleton<IPerfLogRepository>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<ConfigurationService>();
            if (config.UsePostgresDb)
                return new PostgresDbPerfLogRepository(config);
            return new LiteDbPerfLogRepository(config);
        });

        services.AddSingleton<LogDispatcherService>();
        services.AddSingleton<AppLogService>();
        services.AddSingleton<PerfLogService>();
        //services.AddHostedService<LogProcessorBackgroundService>();
        //services.AddHostedService<PerfLogProcessorBackgroundService>();
        if (isClient)
        {
            services.AddHostedService<UnifiedLogProcessorBackgroundService>();
        }

        var key = Encoding.ASCII.GetBytes("your_secret_key_here"); // Replace with your secret key

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Set to true if you have an issuer you want to validate
                    ValidateAudience = false, // Set to true if you have an audience you want to validate
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

        if (isServer)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fc Log", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT" });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
                    },
                    new string[]
                    {
                    }
                }
            });
            });
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var isClient = Environment.GetCommandLineArgs().IsParamAvailable("--client");
        var isServer = Environment.GetCommandLineArgs().IsParamAvailable("--server");

        if (!isClient && !isServer)
        {
            Console.WriteLine("Please specify if this is a client or server instance using --client or --server");
            Environment.Exit(1);
        }

        app.UseAuthentication();
        
        if (isServer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(s =>
                {
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fc Log V1");
                });
            }
        
            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
