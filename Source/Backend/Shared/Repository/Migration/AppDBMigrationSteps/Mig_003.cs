using System;
using Manifest.Common.Constants;
using Manifest.Common.Enums;
using FluentMigrator;
using Framework.Infrastructure.Utils;

namespace Manifest.Repository.Migration.Application
{
    [Migration(003)]
#pragma warning disable S101 // Types should be named in PascalCase
    public class Mig_003 : BaseMigration
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public override void Up()
        {
            dynamic[] configurationGroupParams =
                {
                new { ConfigurationGroupId =ConfigurationConstants.ConfigurationGroup.BaseConfiguration,GroupName = "Framework",  IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new { ConfigurationGroupId =ConfigurationConstants.ConfigurationGroup.CommonConfiguration ,GroupName = "Common",  IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new { ConfigurationGroupId =ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration ,GroupName = "Manifest Web",  IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                };

            foreach (var param in configurationGroupParams)
            {
                InsertData("ConfigurationGroup", param);
            }

            dynamic[] configurationEntryParams =
                {
                    //Framework
                    new { ConfigurationItemId = 1 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logTrace", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 2 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logDebug", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 3 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logInfo", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 4 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logSql", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 5 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logWarn", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 6 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logError", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 7 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logPerformance", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 8 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logToFile", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 9 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logLocation", Value = "|ConfigPath|/../Data/Logs", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 10 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logToDebugger", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 11 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logToConsole", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 12 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logToServer", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 13 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logServerHost", Value = "", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 14 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logServerPort", Value = "", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 15 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logServerUserName", Value = "", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 16 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "logServerPassword", Value = "", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 17 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "LogServerAccessKey", Value = "", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },

                    new { ConfigurationItemId = 18 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "CacheHighRefreshInMinutes", Value = "5", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 19 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "CachMediumRefreshInMinutes", Value = "10", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 20 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "CachLowRefreshInMinutes", Value = "30", Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 21 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "UseMiniProfiler", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 22 + (ConfigurationConstants.ConfigurationGroup.BaseConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.BaseConfiguration, Key = "ShowAllLinksWithoutPermissionCheck", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },

                    //ManifestWebConfiguration
                    new { ConfigurationItemId = 1 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "ManifestWebServerPort", Value = "7000",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 2 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "ViewUrl", Value = "http://localhost:8000//#/guesturl/{jobnumber}/{guid}",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 3 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "ReadOnlyViewUrl", Value = "http://localhost:8000//#/readonly-guesturl/{jobnumber}/{guid}",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 4 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "EnableFrontPageDisplayMessage", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 5 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "FrontPageDisplayMessage", Value = "Running Dev Version",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 6 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "FrontPageDisplayMessageTitle", Value = "Important Info",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 7 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "FrontPageDisplayMessageDisplayTime", Value = "",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 8 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "FrontPageDisplayMessageUntilDate", Value = "12/12/2030",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 9 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "ReadOnlyAccess", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 10 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "ReadOnlyAccessMessage", Value = "You are running under read-only mode. You cannot make any changes to you account.",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 11 + (ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.ManifestWebConfiguration, Key = "IsProduction", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },

                    //CommonConfiguration
                    new { ConfigurationItemId = 1 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "JwtSecretKey", Value = Guid.NewGuid().ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 2 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "ForceSSL", Value = false.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 3 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailServer", Value = "smtp.sendgrid.net",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 4 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailServerPort", Value = "587",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 5 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailServerUserName", Value = "apikey",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 6 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailServerPassword", Value = "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",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 7 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailUseSSL", Value = true.ToString(), Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 8 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailFromName", Value = "Manifest",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 9 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "EmailFromEmail", Value = "Manifest@Manifest.net",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 10 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "AdminNameForEmail", Value = "Manifest Admin",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 11 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "AdminEmailForEmail", Value = "admin@Manifest.net",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 12 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "DefaultCommonEmailDomain", Value = ConfigurationConstants.CommonConfiguration.CommonEmailDomains , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 13 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "LockFolder", Value = "|ConfigPath|/../LockFolder",  Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 14 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "TimeoutForLockInMin", Value = 2 , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 15 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "SingleSignOnClientId", Value = "" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 16 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "SingleSignOnClientSecret", Value = "" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 17 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "SingleSignOnAuthority", Value = "" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 18 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "SingleSigsnOnCallbackPath", Value = "" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 19 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "WriterLogFileStorageConnectionString", Value = "disk://path=|ConfigPath|/../Data/WriterLogs" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 20 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "LocalLogServerUrl", Value = "http://localhost:5285/" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 21 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "APIServerUrl", Value = "http://localhost:9009/api/v1/" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 22 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "WebApplicationServerUrl", Value = "http://localhost:4200/" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                    new { ConfigurationItemId = 23 + (ConfigurationConstants.ConfigurationGroup.CommonConfiguration * 1000), ConfigurationGroupId = ConfigurationConstants.ConfigurationGroup.CommonConfiguration, Key = "WhatsAppGraphAPIServerUrl", Value = "https://graph.facebook.com/" , Required = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow },
                };

            foreach (var param in configurationEntryParams)
            {
                InsertData("ConfigurationItem", param);
            }

            dynamic[] countryParams =
            {
                new { CountryId = 1,  CountryName = "United States" , AlternateCountryName1 = "US", AlternateCountryName2 = "USA",IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2,  CountryName = "Canada" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3,  CountryName = "Mexico" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 4,  CountryName = "Afghanistan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 5,  CountryName = "Albania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 6,  CountryName = "Algeria" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 7,  CountryName = "Andorra" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 8,  CountryName = "Angola" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 9,  CountryName = "Antigua and Barbuda" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 10,  CountryName = "Argentina" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 11,  CountryName = "Armenia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 12,  CountryName = "Australia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 13,  CountryName = "Austria" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 14,  CountryName = "Azerbaijan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 15,  CountryName = "Bahamas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 16,  CountryName = "Bahrain" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 17,  CountryName = "Bangladesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 18,  CountryName = "Barbados" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 19,  CountryName = "Belarus" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 20,  CountryName = "Belgium" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 21,  CountryName = "Belize" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 22,  CountryName = "Benin" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 23,  CountryName = "Bhutan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 24,  CountryName = "Bolivia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 25,  CountryName = "Bosnia and Herzegovina" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 26,  CountryName = "Botswana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 27,  CountryName = "Brazil" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 28,  CountryName = "Brunei" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 29,  CountryName = "Bulgaria" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 30,  CountryName = "Burkina Faso" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 31,  CountryName = "Burundi" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 32,  CountryName = "C�te d'Ivoire" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 33,  CountryName = "Cabo Verde" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 34,  CountryName = "Cambodia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 35,  CountryName = "Cameroon" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 36,  CountryName = "Central African Republic" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 37,  CountryName = "Chad" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 38,  CountryName = "Chile" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 39,  CountryName = "China" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 40,  CountryName = "Colombia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 41,  CountryName = "Comoros" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 42,  CountryName = "Congo (Congo-Brazzaville)" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 43,  CountryName = "Costa Rica" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 44,  CountryName = "Croatia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 45,  CountryName = "Cuba" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 46,  CountryName = "Cyprus" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 47,  CountryName = "Czechia (Czech Republic)" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 48,  CountryName = "Democratic Republic of the Congo" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 49,  CountryName = "Denmark" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 50,  CountryName = "Djibouti" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 51,  CountryName = "Dominica" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 52,  CountryName = "Dominican Republic" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 53,  CountryName = "Ecuador" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 54,  CountryName = "Egypt" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 55,  CountryName = "El Salvador" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 56,  CountryName = "Equatorial Guinea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 57,  CountryName = "Eritrea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 58,  CountryName = "Estonia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 59,  CountryName = "Eswatini (fmr. Swaziland)" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 60,  CountryName = "Ethiopia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 61,  CountryName = "Fiji" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 62,  CountryName = "Finland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 63,  CountryName = "France" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 64,  CountryName = "Gabon" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 65,  CountryName = "Gambia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 66,  CountryName = "Georgia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 67,  CountryName = "Germany" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 68,  CountryName = "Ghana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 69,  CountryName = "Greece" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 70,  CountryName = "Grenada" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 71,  CountryName = "Guatemala" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 72,  CountryName = "Guinea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 73,  CountryName = "Guinea-Bissau" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 74,  CountryName = "Guyana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 75,  CountryName = "Haiti" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 76,  CountryName = "Holy See" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 77,  CountryName = "Honduras" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 78,  CountryName = "Hungary" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 79,  CountryName = "Iceland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80,  CountryName = "India" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 81,  CountryName = "Indonesia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 82,  CountryName = "Iran" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 83,  CountryName = "Iraq" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 84,  CountryName = "Ireland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 85,  CountryName = "Israel" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 86,  CountryName = "Italy" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 87,  CountryName = "Jamaica" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 88,  CountryName = "Japan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 89,  CountryName = "Jordan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 90,  CountryName = "Kazakhstan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 91,  CountryName = "Kenya" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 92,  CountryName = "Kiribati" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 93,  CountryName = "Kuwait" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 94,  CountryName = "Kyrgyzstan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 95,  CountryName = "Laos" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 96,  CountryName = "Latvia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 97,  CountryName = "Lebanon" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 98,  CountryName = "Lesotho" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 99,  CountryName = "Liberia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 100,  CountryName = "Libya" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 101,  CountryName = "Liechtenstein" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 102,  CountryName = "Lithuania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 103,  CountryName = "Luxembourg" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 104,  CountryName = "Madagascar" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 105,  CountryName = "Malawi" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 106,  CountryName = "Malaysia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 107,  CountryName = "Maldives" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 108,  CountryName = "Mali" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 109,  CountryName = "Malta" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 110,  CountryName = "Marshall Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 111,  CountryName = "Mauritania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 112,  CountryName = "Mauritius" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 113,  CountryName = "Micronesia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 114,  CountryName = "Moldova" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 115,  CountryName = "Monaco" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 116,  CountryName = "Mongolia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 117,  CountryName = "Montenegro" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 118,  CountryName = "Morocco" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 119,  CountryName = "Mozambique" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 120,  CountryName = "Myanmar (formerly Burma)" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 121,  CountryName = "Namibia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 122,  CountryName = "Nauru" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 123,  CountryName = "Nepal" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 124,  CountryName = "Netherlands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 125,  CountryName = "New Zealand" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 126,  CountryName = "Nicaragua" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 127,  CountryName = "Niger" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 128,  CountryName = "Nigeria" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 129,  CountryName = "North Korea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 130,  CountryName = "North Macedonia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 131,  CountryName = "Norway" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 132,  CountryName = "Oman" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 133,  CountryName = "Pakistan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 134,  CountryName = "Palau" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 135,  CountryName = "Palestine State" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 136,  CountryName = "Panama" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 137,  CountryName = "Papua New Guinea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 138,  CountryName = "Paraguay" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 139,  CountryName = "Peru" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 140,  CountryName = "Philippines" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 141,  CountryName = "Poland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 142,  CountryName = "Portugal" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 143,  CountryName = "Qatar" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 144,  CountryName = "Romania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 145,  CountryName = "Russia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 146,  CountryName = "Rwanda" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 147,  CountryName = "Saint Kitts and Nevis" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 148,  CountryName = "Saint Lucia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 149,  CountryName = "Saint Vincent and the Grenadines" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 150,  CountryName = "Samoa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 151,  CountryName = "San Marino" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 152,  CountryName = "Sao Tome and Principe" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 153,  CountryName = "Saudi Arabia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 154,  CountryName = "Senegal" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 155,  CountryName = "Serbia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 156,  CountryName = "Seychelles" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 157,  CountryName = "Sierra Leone" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 158,  CountryName = "Singapore" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 159,  CountryName = "Slovakia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 160,  CountryName = "Slovenia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 161,  CountryName = "Solomon Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 162,  CountryName = "Somalia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 163,  CountryName = "South Africa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 164,  CountryName = "South Korea" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 165,  CountryName = "South Sudan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 166,  CountryName = "Spain" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 167,  CountryName = "Sri Lanka" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 168,  CountryName = "Sudan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 169,  CountryName = "Suriname" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 170,  CountryName = "Sweden" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 171,  CountryName = "Switzerland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 172,  CountryName = "Syria" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 173,  CountryName = "Tajikistan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 174,  CountryName = "Tanzania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 175,  CountryName = "Thailand" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 176,  CountryName = "Timor-Leste" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 177,  CountryName = "Togo" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 178,  CountryName = "Tonga" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 179,  CountryName = "Trinidad and Tobago" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 180,  CountryName = "Tunisia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 181,  CountryName = "Turkey" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 182,  CountryName = "Turkmenistan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 183,  CountryName = "Tuvalu" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 184,  CountryName = "Uganda" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 185,  CountryName = "Ukraine" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 186,  CountryName = "United Arab Emirates" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 187,  CountryName = "United Kingdom" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 188,  CountryName = "Uruguay" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 189,  CountryName = "Uzbekistan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 190,  CountryName = "Vanuatu" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 191,  CountryName = "Venezuela" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 192,  CountryName = "Vietnam" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 193,  CountryName = "Yemen" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 194,  CountryName = "Zambia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 195,  CountryName = "Zimbabwe" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 196,  CountryName = "Hong Kong" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 197,  CountryName = "Cook Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 199,  CountryName = "Taiwan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
            };

            IdentityInsertData("Country", countryParams);

            dynamic[] stateParams =
            {
                new { CountryId = 1, StateId = 1, StateCode = "AL", StateName = "Alabama" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 2, StateCode = "AK", StateName = "Alaska" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 3, StateCode = "AZ", StateName = "Arizona" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 4, StateCode = "AR", StateName = "Arkansas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 5,StateCode = "CA",  StateName = "California" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 6,StateCode = "CO",  StateName = "Colorado" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 7,StateCode = "CT",  StateName = "Connecticut" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 8,StateCode = "DE",  StateName = "Delaware" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 9,StateCode = "FL",  StateName = "Florida" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 10,StateCode = "GA",  StateName = "Georgia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 11,StateCode = "HI",  StateName = "Hawaii" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 12,StateCode = "ID",  StateName = "Idaho" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 13,StateCode = "IL",  StateName = "Illinois" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 14,StateCode = "IN",  StateName = "Indiana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 15,StateCode = "IA",  StateName = "Iowa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 16,StateCode = "KS",  StateName = "Kansas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 17,StateCode = "KY",  StateName = "Kentucky" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 18,StateCode = "LA",  StateName = "Louisiana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 19,StateCode = "ME",  StateName = "Maine" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 20,StateCode = "MD",  StateName = "Maryland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 21,StateCode = "MA",  StateName = "Massachusetts" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 22,StateCode = "MI",  StateName = "Michigan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 23,StateCode = "MN",  StateName = "Minnesota" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 24,StateCode = "MS",  StateName = "Mississippi" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 25,StateCode = "MO",  StateName = "Missouri" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 26,StateCode = "MT",  StateName = "Montana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 27,StateCode = "NE",  StateName = "Nebraska" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 28,StateCode = "NV",  StateName = "Nevada" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 29,StateCode = "NH",  StateName = "New Hampshire" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 30,StateCode = "NJ",  StateName = "New Jersey" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 31,StateCode = "NM",  StateName = "New Mexico" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 32,StateCode = "NY",  StateName = "New York" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 33,StateCode = "NC",  StateName = "North Carolina" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 34,StateCode = "ND",  StateName = "North Dakota" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 35,StateCode = "OH",  StateName = "Ohio" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 36,StateCode = "OK",  StateName = "Oklahoma" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 37,StateCode = "OR",  StateName = "Oregon" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 38,StateCode = "PA",  StateName = "Pennsylvania" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 39,StateCode = "RI",  StateName = "Rhode Island" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 40,StateCode = "SC",  StateName = "South Carolina" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 41,StateCode = "SD",  StateName = "South Dakota" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 42,StateCode = "TN",  StateName = "Tennessee" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 43,StateCode = "TX",  StateName = "Texas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 44,StateCode = "UT",  StateName = "Utah" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 45,StateCode = "VT",  StateName = "Vermont" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 46,StateCode = "VA",  StateName = "Virginia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 47,StateCode = "WA",  StateName = "Washington" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 48,StateCode = "WV",  StateName = "West Virginia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 49,StateCode = "WI",  StateName = "Wisconsin" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 50,StateCode = "WY",  StateName = "Wyoming" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 51,StateCode = "DC",  StateName = "District of Columbia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 52,StateCode = "AS",  StateName = "American Samoa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 53,StateCode = "GU",  StateName = "Guam" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 54,StateCode = "MP",  StateName = "Northern Mariana Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 55,StateCode = "PR",  StateName = "Puerto Rico" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 56,StateCode = "VI",  StateName = "U.S. Virgin Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 57,StateCode = "UM",  StateName = "U.S. Minor Outlying Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 58,StateCode = "FM",  StateName = "Micronesia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 59,StateCode = "MH",  StateName = "Marshall Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 1, StateId = 60,StateCode = "PW",  StateName = "Palau" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,

                new { CountryId = 2, StateId = 100,StateCode = "AB",  StateName = "Alberta" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 101,StateCode = "BC",  StateName = "British Columbia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 102,StateCode = "MB",  StateName = "Manitoba" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 103,StateCode = "NB",  StateName = "New Brunswick" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 104,StateCode = "NL",  StateName = "Newfoundland and Labrador" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 105,StateCode = "NS",  StateName = "Nova Scotia" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 106,StateCode = "ON",  StateName = "Ontario" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 107,StateCode = "PE",  StateName = "Prince Edward Island" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 108,StateCode = "QC",  StateName = "Quebec" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 109,StateCode = "SK",  StateName = "Saskatchewan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 110,StateCode = "NT",  StateName = "Northwest Territories" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 111,StateCode = "NU",  StateName = "Nunavut" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 2, StateId = 112,StateCode = "YT",  StateName = "Yukon" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,

                new { CountryId = 3, StateId = 200,StateCode = "AGU",  StateName = "Aguascalientes" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 201,StateCode = "BCN",  StateName = "Baja California" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 202,StateCode = "BCS",  StateName = "Baja California Sur" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 203,StateCode = "CAM",  StateName = "Campeche" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 204,StateCode = "CHP",  StateName = "Chiapas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 205,StateCode = "CHH",  StateName = "Chihuahua" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 206,StateCode = "COA",  StateName = "Coahuila" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 207,StateCode = "COL",  StateName = "Colima" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 208,StateCode = "DUR",  StateName = "Durango" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 209,StateCode = "GUA",  StateName = "Guanajuato" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 210,StateCode = "GRO",  StateName = "Guerrero" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 211,StateCode = "HID",  StateName = "Hidalgo" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 212,StateCode = "JAL",  StateName = "Jalisco" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 213,StateCode = "MEX",  StateName = "M�xico" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 214,StateCode = "CMX",  StateName = "M�xico City" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 215,StateCode = "MIC",  StateName = "Michoac�n" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 216,StateCode = "MOR",  StateName = "Morelos" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 217,StateCode = "NAY",  StateName = "Nayarit" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 218,StateCode = "NLE",  StateName = "Nuevo Le�n" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 219,StateCode = "OAX",  StateName = "Oaxaca" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 220,StateCode = "PUE",  StateName = "Puebla" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 221,StateCode = "QUE",  StateName = "Quer�taro" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 222,StateCode = "ROO",  StateName = "Quintana Roo" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 223,StateCode = "SLP",  StateName = "San Luis Potos�" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 224,StateCode = "SIN",  StateName = "Sinaloa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 225,StateCode = "SON",  StateName = "Sonora" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 226,StateCode = "TAB",  StateName = "Tabasco" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 227,StateCode = "TAM",  StateName = "Tamaulipas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 228,StateCode = "TLA",  StateName = "Tlaxcala" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 229,StateCode = "VER",  StateName = "Veracruz" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 230,StateCode = "YUC",  StateName = "Yucat�n" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 3, StateId = 231,StateCode = "ZAC",  StateName = "Zacatecas" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 232,StateCode = "AP",  StateName = "Andhra Pradesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 233,StateCode = "AR",  StateName = "Arunachal Pradesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 234,StateCode = "AS",  StateName = "Assam" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 235,StateCode = "BR",  StateName = "Bihar" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 236,StateCode = "CG",  StateName = "Chhattisgarh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 237,StateCode = "GA",  StateName = "Goa" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 238,StateCode = "GJ",  StateName = "Gujarat" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 239,StateCode = "HR",  StateName = "Haryana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 240,StateCode = "HP",  StateName = "Himachal Pradesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 241,StateCode = "JH",  StateName = "Jharkhand" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 242,StateCode = "KA",  StateName = "Karnataka" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 243,StateCode = "KL",  StateName = "Kerala" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 244,StateCode = "MP",  StateName = "Madhya Pradesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 245,StateCode = "MH",  StateName = "Maharashtra" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 246,StateCode = "MN",  StateName = "Manipur" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 247,StateCode = "ML",  StateName = "Meghalaya" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 248,StateCode = "MZ",  StateName = "Mizoram" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 249,StateCode = "NL",  StateName = "Nagaland" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 250,StateCode = "OD",  StateName = "Odisha" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 251,StateCode = "PB",  StateName = "Punjab" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 252,StateCode = "RJ",  StateName = "Rajasthan" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 253,StateCode = "SK",  StateName = "Sikkim" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 254,StateCode = "TN",  StateName = "Tamil Nadu" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 255,StateCode = "TG",  StateName = "Telangana" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 256,StateCode = "TR",  StateName = "Tripura" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 257,StateCode = "UP",  StateName = "Uttar Pradesh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 258,StateCode = "UK",  StateName = "Uttarakhand" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 259,StateCode = "WB",  StateName = "West Bengal" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 260,StateCode = "AN",  StateName = "Andaman and Nicobar Islands" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 261,StateCode = "CH",  StateName = "Chandigarh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 262,StateCode = "DL",  StateName = "Delhi" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 263,StateCode = "JK",  StateName = "Jammu and Kashmir" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 264,StateCode = "LA",  StateName = "Ladakh" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 265,StateCode = "LD",  StateName = "Lakshadweep" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 266,StateCode = "PY",  StateName = "Puducherry" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
                new { CountryId = 80, StateId = 267,StateCode = "DD",  StateName = "Dadra and Nagar Haveli and Daman and Diu" , IsDeleted = false , CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow } ,
            };

            IdentityInsertData("State", stateParams);

            dynamic[] userParams =
            {
                new { UserId = 1, UserGuid = Guid.NewGuid(), UserName = "super-admin", PredefinedUserTypeId = (int)PredefinedUserTypes.SuperAdmin, PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "sa@op.com", FirstName = "Super", LastName= "Admin",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },

                //new { UserId = 1000, CompanyId = 1000, UserName = "acme-admin",PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "admin@acme.op.com", FirstName = "Acme", LastName= "Admin",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 1001, CompanyId = 1000, UserName = "acme-config",PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyConfigurator,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@acme.op.com", FirstName = "Acme", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 1002, CompanyId = 1000, UserName = "acme-producer",PredefinedUserTypeId = (int)PredefinedUserTypes.Producer,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@acme.op.com", FirstName = "Acme", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 1003, CompanyId = 1000, UserName = "acme-ast-producer",PredefinedUserTypeId = (int)PredefinedUserTypes.AssistantProducer,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@acme.op.com", FirstName = "Acme", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },

                //new { UserId = 2000, CompanyId = 1001, UserName = "big-admin",PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "admin@big.op.com", FirstName = "Big", LastName= "Admin",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 2001, CompanyId = 1001, UserName = "big-config",PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyConfigurator,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@big.op.com", FirstName = "Big", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 2002, CompanyId = 1001, UserName = "big-producer",PredefinedUserTypeId = (int)PredefinedUserTypes.Producer,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@big.op.com", FirstName = "Big", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
                //new { UserId = 2003, CompanyId = 1001, UserName = "big-ast-producer",PredefinedUserTypeId = (int)PredefinedUserTypes.AssistantProducer,PasswordHash= PasswordHash.CreateHash("!test1234!", out _), Email= "user1@big.op.com", FirstName = "Big", LastName= "User 1",  Active = true, IsDeleted = false, CreatedDate = DateTime.UtcNow,  ModifiedDate = DateTime.UtcNow },
            };

            IdentityInsertData("User", userParams);

            var mailServerParam = new { MailServerName = "DefaultManifestMailServer", MailServerCode = "M1", Host = "smtp.gmail.com", Port = 465, SenderName = "Sender", SenderEmail = "oppanttest@gmail.com", UserName = "oppanttest@gmail.com", Password = "ypwe yewx uowd dxhi", IsSecure = true, IsDefault = true, IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };

            InsertData("MailServer", mailServerParam);

            var smsServerParam = new { SmsServerName = "DefaultManifestSmsServer", PredefinedSmsAndVoiceServerTypeId = (int)PredefinedSmsAndVoiceServerTypes.Twilio, SmsServerCode = "S1", Host = "Twilio.com", Port = 90, SenderName = "Sender", SenderEmail = "sender@domain.com", UserName = "username", Password = "password", IsSecure = true, IsDefault = true, IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };

            InsertData("SmsServer", smsServerParam);

            dynamic[] singleSignOnServerParams =
            {
                new { SingleSignOnServerName = "DefaultManifestGoogleSSOServer", SingleSignOnServerCode = "SSC1", PredefinedSingleSignOnServerTypeId = (long)PredefinedSingleSignOnServerTypes.Google, ForAllCompany = true, IsDefault = true, IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new { SingleSignOnServerName = "DefaultManifestMicrosoftSSOServer", SingleSignOnServerCode = "SSC2", PredefinedSingleSignOnServerTypeId = (long)PredefinedSingleSignOnServerTypes.Microsoft, ForAllCompany = true, IsDefault = true, IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            };

            InsertData("SingleSignOnServer", singleSignOnServerParams);

            dynamic[] defaultJobCategoryParams =
            {
                new { JobTemplateCategoryId = 1, Name = "General", IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new { JobTemplateCategoryId = 2, Name = "Scheduled", IsDeleted = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            };

            IdentityInsertData("JobTemplateCategory", defaultJobCategoryParams);

            dynamic[] defaultJobTemplateParams =
            {
                new { JobTemplateId = 1, PredefinedJobUsageForTypeId = (int)PredefinedJobUsageForTypes.SuperAdmins, PredefinedJobTypeId = (int)PredefinedJobTypes.DisableMovieProcessor, JobTemplateCategoryId = 2, TemplateName = "Disable Movie Processor", Description = "Disable Movie Processor", ExecutableFolder = "",ExecutableName= "", ReportTemplateFile = "", DefaultOutputFileName= "", TaskIdentifierName = "", CanExportCSV = true, CanExportDOC = true, CanExportPDF = true, CanExportPPT = true, CanExportXLS = true, IsDeleted = false, CanExportCSVDefaultValue = false, CanExportCustom = false, CanExportCustomDefaultValue = false, CanExportDOCDefaultValue = false, CanExportPDFDefaultValue = false, CanExportPPTDefaultValue = false, CanExportXLSDefaultValue = false, CanExportXML = false, CanExportXMLDefaultValue = false, CustomExportName = "", ExecutableParameter = "", IncludeDataSourceFile = true,IncludeDataSourceFileDefaultValue = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new { JobTemplateId = 2, PredefinedJobUsageForTypeId = (int)PredefinedJobUsageForTypes.All, PredefinedJobTypeId = (int)PredefinedJobTypes.WakeupNotificationProcessor, JobTemplateCategoryId = 2, TemplateName = "Wakeup Notification Processor", Description = "Wakeup Notification Processor", ExecutableFolder = "",ExecutableName= "", ReportTemplateFile = "", DefaultOutputFileName= "", TaskIdentifierName = "", CanExportCSV = true, CanExportDOC = true, CanExportPDF = true, CanExportPPT = true, CanExportXLS = true, IsDeleted = false, CanExportCSVDefaultValue = false, CanExportCustom = false, CanExportCustomDefaultValue = false, CanExportDOCDefaultValue = false, CanExportPDFDefaultValue = false, CanExportPPTDefaultValue = false, CanExportXLSDefaultValue = false, CanExportXML = false, CanExportXMLDefaultValue = false, CustomExportName = "", ExecutableParameter = "", IncludeDataSourceFile = true,IncludeDataSourceFileDefaultValue = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            };

            IdentityInsertData("JobTemplate", defaultJobTemplateParams);

            dynamic[] defaultScheduledJobParams =
            {
                new { ScheduledJobId = 1, JobTemplateId = 1,  JobName = "Disable Movie Processor", PredefinedJobTypeId = (int)PredefinedJobTypes.DisableMovieProcessor, StartDate = DateTime.UtcNow, ExecutableFolder = "", ExecutableName = "", ExecutableParameter = "", ExportCSV = false, ExportDOC = false, ExportPDF = false, ExportPPT = false, ExportXLS = false,Active = true, IsDaily = true, Recurring = true, RunDailyTime = new DateTime(2021,1,1, 20,0,0),IsDeleted = false,CustomExportName = "", ExportXML = false, IncludeDataSourceFile = true, ExportCustom = false, IsHourly = false, IsMonthly = false, IsWeekly = false, RunFriday = true,RunMonday = true, RunSaturday = true, RunSunday = true, RunThursday = true, RunTuesday = true, RunWednesday = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            };

            IdentityInsertData("ScheduledJob", defaultScheduledJobParams);

            //dynamic[] defaultUserRolePermissionParams =
            //{
            //    //SuperAdmin
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.SuperAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureSystem, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //HostAdmin
            //    ////ConfigureCompany = 400011,
            //    ////ConfigureOtherCompany
            //    ////ManageProduct = 400061,
            //    ////ManageCatalog = 400063,
            //    ////ApproveOrder = 400023,
            //    //////CompanyAdmin
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCustomerCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CompanyAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //HostManager
            //    ////ConfigureCompany = 400011,
            //    ////ConfigureOtherCompany
            //    ////ManageProduct = 400061,
            //    ////ManageCatalog = 400063,
            //    ////ApproveOrder = 400023,
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCustomerCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //HostDeliveryManager
            //    ////ViewDeliveryOrders = 400051,
            //    ////AssignDeliveryOrdersToOthers = 400052,
            //    ////UpdateDeliveryOrders = 400053,

            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewDeliveryOrders, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.AssignDeliveryOrdersToOthers, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.UpdateOrderDeliveryStatus, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //HostDeliveryPersonnel
            //    ////ViewDeliveryOrders = 400051,
            //    ////UpdateDeliveryOrders = 400053,

            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostDeliveryPersonnel, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewDeliveryOrders, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.HostDeliveryPersonnel, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.UpdateOrderDeliveryStatus, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //MarketPlaceAdmin
            //    ////ConfigureCompany = 400011,
            //    ////ManageProduct = 400061,
            //    ////ManageCatalog = 400063,
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //MarketPlaceManager
            //    ////ConfigureCompany = 400011,
            //    ////ManageProduct = 400061,
            //    ////ManageCatalog = 400063,
            //    ////ApproveOrder = 400023,
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //MarketPlaceDeliveryManager
            //    ////ViewDeliveryOrders = 400051,
            //    ////AssignDeliveryOrdersToOthers = 400052,
            //    ////UpdateDeliveryOrders = 400053,
            //    ////ApproveOrder = 400023,

            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewDeliveryOrders, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.AssignDeliveryOrdersToOthers, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.UpdateOrderDeliveryStatus, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewDeliveryOrders, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.UpdateOrderDeliveryStatus, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //MarketPlaceDeliveryPersonnel
            //    ////ViewDeliveryOrders = 400051,
            //    ////UpdateDeliveryOrders = 400053,
            //    ///

            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryPersonnel, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewDeliveryOrders, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.MarketPlaceDeliveryPersonnel, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.UpdateOrderDeliveryStatus, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //CustomerAdmin
            //    ////ConfigureCompany = 400011,
            //    ////CreateOwnOrder = 400022,
            //    ////CreateOthersOrder
            //    ////ApproveOrder = 400023,
            //    ////ManageOrder = 400023,
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnEmployeeOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerAdmin, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //CustomerManager
            //    ////ConfigureCompany = 400011,
            //    ////ManageOrder = 400023,
            //    ////ApproveOrder = 400023,
            //    ////CreateOwnOrder = 400022,
            //    ////CreateOthersOrder
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ManageOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnEmployeeOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerManager, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //CustomerHr
            //    ////ViewCompany = 400012,
            //    ////ViewOrder = 400022,
            //    ////ApproveOrder = 400023,
            //    ////CreateOwnOrder = 400022,
            //    ////CreateOthersOrder
            //    ///ConfigureUser
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewCompany, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ApproveOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnEmployeeOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.CustomerHr, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ConfigureUser, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //Employee
            //    ////CreateOwnOrder = 400022,
            //    ////ViewOrder = 400024,
            //    ////ViewProduct = 400062,
            //    ////ViewCatalog = 400064,
            //    ///
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.Employee, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.Employee, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.Employee, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.Employee, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,

            //    //EmployeeReferral
            //    ////CreateOwnOrder = 400022,
            //    ////ViewOrder = 400024,
            //    ////ViewProduct = 400062,
            //    ////ViewCatalog = 400064,
            //    ///
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.EmployeeReferral, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.CreateOwnOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.EmployeeReferral, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewProduct, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.EmployeeReferral, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewCatalog, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //    new { PredefinedUserTypeId = (int)PredefinedUserTypes.EmployeeReferral, PredefinedUserPermissionTypeId = (int)PredefinedUserPermissionTypes.ViewOrder, IsDeleted = false, CreatedDate = DateTime.UtcNow ,  ModifiedDate = DateTime.UtcNow } ,
            //};

            //InsertData("DefaultUserTypePermission", defaultUserRolePermissionParams);
        }
    }
}