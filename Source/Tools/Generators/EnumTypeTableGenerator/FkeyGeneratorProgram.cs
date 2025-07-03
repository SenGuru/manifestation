using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Manifest.Common.Models.DB;
using CodeGenerator.Models;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class FkeyGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Parameter should be <ClassName>");
                return;
            }

            Console.WriteLine(GetFKeyAndIndex(args[1]));
        }

        private static string GetFKeyAndIndex(string className)
        {
            var cls = CodeGenUtils.GetClassFromAssembly(typeof(Manifest.Common.Constants.ConfigurationConstants).Assembly, className);

            if (cls == null)
                throw new Exception($"Unable to find in the assembly - {className}");

            var sb = new StringBuilder();
            //sb.AppendLine(GetBaseCode(className));

            foreach (var t in cls.GetProperties())
            {
                if (t.Name == className + "Id" || t.Name.ToUpper().EndsWith("GUID"))
                {
                    continue;
                }

                if (t.Name.ToUpper().Equals("CreatedByUserId".ToUpper()) || t.Name.ToUpper().Equals("ModifiedByUserId".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, "User", "UserId"));
                }
                else if (t.Name.ToUpper().EndsWith("MetadataId".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, "MetaData", "MetaDataId"));
                }
                else if (t.Name.ToUpper().StartsWith("Predefined".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, "EnumType", "EnumTypeId"));
                }
                else if (t.Name.ToUpper().StartsWith("CopiedFrom".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("CopiedFrom", ""), t.Name.Replace("CopiedFrom", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("RunAs".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("RunAs", ""), t.Name.Replace("RunAs", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("SubmittedBy".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("SubmittedBy", ""), t.Name.Replace("SubmittedBy", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent1".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent1", ""), t.Name.Replace("Parent1", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent2".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent2", ""), t.Name.Replace("Parent2", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent3".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent3", ""), t.Name.Replace("Parent3", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent4".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent4", ""), t.Name.Replace("Parent4", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent5".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent5", ""), t.Name.Replace("Parent5", "") + "Id"));
                }
                else if (t.Name.ToUpper().StartsWith("Parent".ToUpper()))
                {
                    sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Parent", ""), t.Name.Replace("Parent", "") + "Id"));
                }
                else if (t.Name.ToUpper().EndsWith("Id".ToUpper()))
                {
                    if ((className == "WhatsAppNotificationServer" && (t.Name == "BusinessAccountId" || t.Name == "PhoneNumberId")) ||
                        (className == "VoiceNotificationServer" && (t.Name == "TwilioAccountSid")) ||
                        (className == "SingleSignOnServer" && (t.Name == "ClientId" || t.Name == "ClientTenantId")) ||
                        (className == "PushNotificationServer" && (t.Name == "ProjectId" || t.Name == "AppleTeamId" || t.Name == "AppleKeyId"))
                        )
                    {
                        sb.Append("");
                    }
                    else
                    {
                        sb.AppendLine(GetCode(className, t.Name, t.Name.Replace("Id", "")));
                    }
                }
            }
            return sb.ToString();
        }

        private static string GetCode(string table, string column, string toTable, string toTableColumn = null)
        {
            if (toTableColumn == null)
                toTableColumn = column;
            return @$"
            Create.Index(""IX_{table}_{column}"").OnTable(""{table}"")
                .OnColumn(""{column}"");

            Create.ForeignKey(""FK_{table}_{column}"").FromTable(""{table}"").ForeignColumns(""{column}"")
                .ToTable(""{toTable}"").PrimaryColumns(""{toTableColumn}"");
                ";
        }
    }
}