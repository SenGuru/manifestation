using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public static class MesssageTemplateGeneratorProgram
    {
        public static void MainApp()
        {
            var fileName = Directory.GetCurrentDirectory() + "\\TemplateMigration.cs";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var sb = new StringBuilder();

            var t = typeof(Manifest.Common.Enums.PredefinedUserTypes);

            sb.AppendLine($"            dynamic[] defaultMessageTemplateParams =");
            sb.AppendLine("            {");
            foreach (var enumName in System.Enum.GetNames(t))
            {
                var str = string.Format("                new {{ MessageTemplateTypeId = (int)MessageTemplateTypes.{0},EmailSubject = Manifest.Common.Resources.EmailTemplateResource.{0}EmailSubject, EmailBody = Manifest.Common.Resources.EmailTemplateResource.{0}EmailBody, SmsSubject = Manifest.Common.Resources.EmailTemplateResource.{0}SmsSubject, SmsBody = Manifest.Common.Resources.EmailTemplateResource.{0}SmsBody, AppNotificationSubject = Manifest.Common.Resources.EmailTemplateResource.{0}AppNotificationSubject, AppNotificationBody = Manifest.Common.Resources.EmailTemplateResource.{0}AppNotificationBody, WebNotificationSubject = Manifest.Common.Resources.EmailTemplateResource.{0}WebNotificationSubject, WebNotificationBody= Manifest.Common.Resources.EmailTemplateResource.{0}WebNotificationBody,  CompanyId = 0, IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }} ,", enumName);
                sb.AppendLine(str);
            }

            sb.AppendLine($"");

            File.AppendAllText(fileName, sb.ToString());
            Process.Start("notepad.exe", fileName);

            var resourceFileName = Directory.GetCurrentDirectory() + "\\ResourceFile.xml";

            if (File.Exists(resourceFileName))
            {
                File.Delete(resourceFileName);
            }

            sb = new StringBuilder();
            var temp = "  <data name=\"{0}{1}{2}\" xml:space=\"preserve\">" +
            "    <value>{0}{1}{2}</value>" +
            "  </data>\n";

            var mode = new List<string> { "Email", "Sms", "AppNotification", "WebNotification" };
            var part = new List<string> { "Subject", "Body" };

            foreach (var enumName in System.Enum.GetNames(t))
            {
                foreach (var m in mode)
                {
                    foreach (var p in part)
                    {
                        sb.AppendFormat(temp, enumName, m, p);
                    }
                }
            }

            sb.AppendLine($"");

            File.AppendAllText(resourceFileName, sb.ToString());
            Process.Start("notepad.exe", resourceFileName);
        }
    }
}
