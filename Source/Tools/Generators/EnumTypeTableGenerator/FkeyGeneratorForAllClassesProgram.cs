using Framework.Infrastructure.Utils;
using System;

namespace CodeGenerator
{
    public static class FkeyGeneratorForAllClassesProgram
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
                foreach (var cls in tableListString.Split(':'))
                {
                    FkeyGeneratorProgram.MainApp(new string[] { "", cls.Trim() });
                }
            }
            else
            {
                foreach (var aClass in assembly.GetTypes())
                {
                    if (!aClass.FullName.StartsWith("Manifest.Common.Models.DB."))
                        continue;

                    FkeyGeneratorProgram.MainApp(new string[] { "", aClass.Name });
                }
            }
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