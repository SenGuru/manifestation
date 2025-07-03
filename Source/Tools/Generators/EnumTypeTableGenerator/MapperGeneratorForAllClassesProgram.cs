using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class MapperGeneratorForAllClassesProgram
    {
        public static void MainApp(bool forSelected = false, string tableListString = "")
        {
            //var sb = new StringBuilder();

            var t = typeof(Manifest.Common.Constants.ConfigurationConstants);

            var assembly = t.Assembly;
            if (assembly == null)
            {
                Console.WriteLine("Unable to find the Manifest.Common assembly");
                return;
            }

            if (forSelected && tableListString.IsNullOrEmpty())
            {
                Console.WriteLine("Please provide the list of tables separated by a semicolon to generate foreign keys and indexes for.");
                return;
            }

            if (forSelected)
            {
                Generate(tableListString.Split(':').ToArray());
            }
            else
            {
                var lst = new List<string>();
                foreach (var aClass in assembly.GetTypes())
                {
                    if (!aClass.FullName.StartsWith("Manifest.Common.Models.DB."))
                        continue;

                    lst.Add(aClass.Name);                
                }
                Generate(lst.ToArray());
            }
        }

        static void Generate(string[] str)
        {
            var serviceFile = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Service\\WIP\\"+"GeneratedMapper.cs";
            var templateFileLocation = CodeGenUtils.GetTemplateFileLocation("WipMapperService.cs");
            var templateContent = File.ReadAllText(templateFileLocation);
            var sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("namespace Manifest.Service.ObjectMapping");
            sb.AppendLine("{");
            sb.AppendLine("public partial class ObjectMapperService");
            sb.AppendLine("{");
            foreach (var template in str)
            {
                var templateC = templateContent;
                var capsFirstLetterString = template[0].ToString().ToUpper() + template.Substring(1);
                var lowerFirstLetterString = template[0].ToString().ToLower() + template.Substring(1);


                templateC = templateC.Replace("EEntity123", capsFirstLetterString, StringComparison.CurrentCulture);
                templateC = templateC.Replace("eentity123", lowerFirstLetterString, StringComparison.CurrentCulture);

                sb.AppendLine(templateC);
            }
            sb.AppendLine("}");
            sb.AppendLine("}");
            File.WriteAllText(serviceFile, sb.ToString());
        }

        //private static string[] ClassesToGenerate = new string[]
        //{
        //    "Job",
        //    "JobAttachment",
        //    "JobNotificationEmail",
        //    "JobTemplate",
        //    "JobTemplateCategory",
        //    "JobTemplateParameter",
        //    "MessageToSend",
        //    "MetaData",
        //    "MetaDataDeleted",
        //    "MetaDataGroup",
        //    "MetaDataGroupSection",
        //    "RealtimeMessage",
        //    "SavedJob",
        //    "SavedJobNotificationEmail",
        //    "ScheduledJob",
        //    "ScheduledJobHistory",
        //    "ScheduledJobNotificationEmail",
        //};
    }
}