using System;
using Manifest.Common.Interfaces.Repository.DbAccess;
using Manifest.Common.Interfaces.Service.Cache;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Repository.Common;
using Manifest.Repository.DbAccess;
using Manifest.Repository.Migration;
using Manifest.Service.Admin;
using Manifest.Service.App;
using Manifest.Service.Cache;
using Manifest.Service.Completed.Config;
using Manifest.Service.Config;
using Manifest.Service.JobServer;
using Manifest.Service.Manager;
using Manifest.Service.ObjectMapping;
using Manifest.Service.Report;
using Manifest.Service.Storage;
using Manifest.Service.Utils;
using Manifest.SharedApp;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Interfaces.Helpers;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Scope;
using Framework.Infrastructure.Services;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Hosting;
using SecurityAssessment.Service.DataUpdate;
#if DI_WEB
using System.Threading.Tasks;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Web.Authentication;
using Manifest.Web.Authentication;
using Manifest.Web.Config;
using Manifest.Web.Filters;
using Framework.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
#else
using Framework.Infrastructure.Helpers;
#endif
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.DI
{
    public static class DISetup
    {
        static DISetup()
        {
            ServiceProvider = new ServiceCollection()
                .AddApplicationDI()
                .BuildServiceProvider();
        }

        public static IServiceProvider ServiceProvider { get; set; }

        public static string Environment { get; set; }

        public static IServiceCollection AddApplicationDI(this IServiceCollection serviceCollection)
        {
            serviceCollection = serviceCollection.AddSingleton<IServiceScopeFactory, CustomServiceScopeFactory>();

            serviceCollection = serviceCollection.AddSingleton<CustomServiceScopeFactory>();

            serviceCollection.AddLogging()
                .AddDistributedMemoryCache()
                .AddMemoryCache();
#if DI_WEB
            // Web related
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<ICurrentUser, WebAppCurrentUser>()
                .AddControllers(config =>
                {
                    config.Filters.Add<ModelStateValidationActionFilterAttribute>();
                    //config.Filters.Add<HttpResponseFixResultFilter>();
                });

            serviceCollection.AddRouting(options => options.LowercaseUrls = true);

            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) && !accessToken.ToString().IsTrimmedStringNullOrEmpty())
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            serviceCollection.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            serviceCollection.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (accessToken.Count != 0)
                        {
                            context.Token = accessToken[0];
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        /*
                        var permissions = context.HttpContext.RequestServices.GetRequiredService<IUserCacheService>().GetUserPermissionsAsync(context.Principal.Identity.Name).Result;
                        if (permissions != null)
                        {
                            var claims = new List<Claim>();
                            foreach (var perm in permissions)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, perm));
                            }

                            var appIdentity = new ClaimsIdentity(claims);
                            context.Principal.AddIdentity(appIdentity);
                        }
                        */
                        return Task.CompletedTask;
                    }
                };
            });
            serviceCollection.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
#else
            serviceCollection.AddScoped<ICurrentUser, ConsoleAppCurrentUser>();
#endif
            //Config and Framework
            serviceCollection.AddSingleton<ILog, Log>()
                .AddScoped<IDBMigration, AppDBMigration>()

                //App Framework
                .AddSingleton<IDBInfo, AppDBInfo>()
                .AddSingleton<AppConfigurationFile>()
                .AddSingleton<AppConfiguration>()
                .AddScoped<IAppDBManager, AppDBManager>()
                .AddScoped<IDBManager>(x => { return x.GetService<IAppDBManager>(); })
                .AddTransient<IConfigDBManager, ConfigDBManager>()
                .AddScoped<ITransactionManager, TransactionManager>()
                .AddScoped<IAppDatabaseUtils, AppDatabaseUtils>()
                .AddScoped<IEnumSelectService, EnumSelectService>()

                //Configuration
                .AddSingleton<ILogConfiguration>(s =>
                {
                    using (var scope = s.CreateScope())
                    {
                        var env = scope.ServiceProvider.GetService<IHostEnvironment>();
                        if (Environment?.Trim().ToLower() == "testing" || (env != null && env.IsEnvironment("Testing")))
                            return LogUtils.TestLogSettings();

                        var cfgMgr = scope.ServiceProvider.GetService<IConfigDBManager>();
                        var appConfigurationFile = scope.ServiceProvider.GetService<AppConfigurationFile>();

                        try
                        {
                            return new LogConfiguration(cfgMgr, appConfigurationFile);
                        }
                        catch
                        {
                            return LogUtils.TraceLogSettings();
                        }
                    }
                })
                .AddSingleton<IBaseDbConfiguration, DbConfiguration>()
                .AddSingleton<ICommonConfiguration, CommonConfiguration>()
                .AddSingleton<ISystemConfiguration, SystemConfiguration>()
                .AddSingleton<ICoordinatorConfiguration, CoordinatorConfiguration>()
                .AddSingleton<IReportServerConfiguration, ReportServerConfiguration>()
                .AddSingleton<IManifestWebConfiguration, ManifestWebConfiguration>()
                .AddSingleton<IBaseAppConfiguration, AppConfiguration>()
                .AddSingleton<ScheduledJobManager>()

                //Repository
                .AddScoped<ConfigurationRepository>()
                .AddScoped<CompanyRepository>()
                .AddScoped<WipRepository>()
                .AddScoped<AuditRepository>()
                .AddScoped<SettingRepository>()
                .AddScoped<ManifestRepository>()
                .AddScoped<SystemRepository>()
                .AddScoped<ReportRepository>()
                .AddScoped<ChatRepository>()
                .AddScoped<IntermediateDBRepository>()

                //.AddScoped<IPermissionRepository, PermissionRepository>()
                //Services
                .AddScoped<AccountService>()
                .AddScoped<CompanyService>()
                .AddScoped<ManifestService>()
                .AddScoped<DummyDataGenerationService>()
                .AddScoped<SystemService>()
                .AddScoped<WipService>()
                .AddScoped<ConfigurationCrudService>()
                .AddScoped<PermissionService>()
                .AddScoped<AuditService>()
                .AddScoped<NewReportServerFileStorage>()
                .AddScoped<FileAttachmentFileStorage>()
                .AddScoped<EnumSelectService>()
                .AddScoped<AdminService>()
                .AddScoped<LogServerClientService>()
                .AddScoped<BackendTestService>()
                .AddScoped<ExternalAppJob>()
                .AddScoped<PredefinedAppJob>()
                .AddScoped<JobFileLocatorService>()
                .AddScoped<ReportService>()
                .AddScoped<ReportCreationService>()
                .AddScoped<ChatService>()
                .AddScoped<TwilioService>()
                .AddScoped<WhatsAppService>()
                //Cache Services
                .AddScoped<IUserCacheService, UserCacheService>()
                //.AddScoped<MovieCacheService>()
                //Util Services
                .AddSingleton<ObjectMapperService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ProfilePictureFileStorage>()
                .AddScoped<CompanyCacheService>()
                .AddScoped<RealtimeMessageNotificationHub>()
                .AddScoped<WordService>()
                .AddScoped<DocumentConverterService>()
                .AddScoped<DataUpdateService>();
#if DI_WEB
            serviceCollection.AddScoped<ICurrentSessionService, CurrentSessionService>();
            serviceCollection.AddScoped<ISingleSignOnService, SingleSignOnService>();
            serviceCollection.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            serviceCollection.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

#endif
            return serviceCollection;
        }
    }
}