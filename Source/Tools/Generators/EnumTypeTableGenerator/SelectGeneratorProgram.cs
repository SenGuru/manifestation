/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Attributes;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class SelectGeneratorProgram
    {
        public static void MainApp()
        {
            var fileName = Directory.GetCurrentDirectory() + "\\SelectList.cs";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var sb = new StringBuilder();

            var t = typeof(Manifest.Common.Constants.ConfigurationConstants);

            var assembly = t.Assembly;
            if (assembly == null)
            {
                Console.WriteLine("Unabel to find the Manifest.Common assembly");
                return;
            }

            WriteHeader(ref sb);
            var start = true;
            foreach (var aClass in assembly.GetTypes())
            {
                if (!aClass.FullName.StartsWith("Manifest.Common.Enums"))
                    continue;

                if (!aClass.IsEnum)
                {
                    sb.AppendLine(aClass.FullName + " is not enum");
                    continue;
                }

                var alst = aClass.GetCustomAttributes(typeof(TypeTableAttribute), true);
                if (alst == null || alst.Length == 0)
                {
                    sb.AppendLine(aClass.FullName + " does not have TypeTable attribute");
                    continue;
                }

                var aitem = alst[0] as TypeTableAttribute;
                var tableName = aitem.TableName;

                //Console.WriteLine(aClass.FullName);
                var camelCaseClass = (aClass.Name.Substring(0, 1).ToLower() + aClass.Name.Substring(1));
                camelCaseClass = camelCaseClass.Substring(0, camelCaseClass.Length - 1);

                if (!start)
                    sb.AppendLine();

                start = false;

                sb.AppendLine($"            List<TextWithValue> {camelCaseClass}SelectionList()");
                sb.AppendLine("            {");
                sb.AppendLine("                 return new List<TextWithValue>() { ");

                foreach (var enumName in System.Enum.GetNames(aClass))
                {
                    var memberInfos = aClass.GetMember(enumName);
                    var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == aClass);
                    var pKeyValue = "";
                    var pkeyLst = enumValueMemberInfo.GetCustomAttributes(typeof(ParentKeyAttribute), true);
                    if (pkeyLst != null && pkeyLst.Length > 0)
                    {
                        pKeyValue = GetPKeyValue(pkeyLst.Select(x => x as ParentKeyAttribute));
                    }

                    var en = enumValueMemberInfo.GetCustomAttributes(typeof(EnumNameOverrideAttribute), true);
                    var an = enumValueMemberInfo.GetCustomAttributes(typeof(AlternateNameAttribute), true);

                    var newEnumName = enumName.SplitCamelCase();
                    if (en != null && en.Length > 0)
                    {
                        var enV = en.Select(x => x as EnumNameOverrideAttribute).FirstOrDefault();
                        if (enV != null && !enV.OverrideName.IsTrimmedStringNullOrEmpty())
                        {
                            newEnumName = enV.OverrideName.Trim();
                        }

                        var anV = an.Select(x => x as AlternateNameAttribute).FirstOrDefault();

                        if (anV != null && !anV.Name.IsTrimmedStringNullOrEmpty())
                        {
                            newEnumName = anV.Name.Trim();
                        }
                    }

                    sb.AppendLine($"                new TextWithValue{{ Value = (int){aClass}.{enumName},  Text = \"{tableName}.To\", {pKeyValue} CreatedDate = DateTime.UtcNow, CreatedBy = \"\" ,  ModifiedDate = DateTime.UtcNow , ModifiedBy = \"\" }} , ");
                }
                sb.AppendLine("            };");
                sb.AppendLine($"");
                sb.AppendLine($"                InsertData(\"{tableName}\",{camelCaseClass}Params);");
            }

            WriteFooter(ref sb);

            File.AppendAllText(fileName, sb.ToString());

            Process.Start("notepad.exe", fileName);

        }

        static string GetPKeyValue(IEnumerable<ParentKeyAttribute> pkAttList)
        {
            var result = string.Join(",", pkAttList.Select(x => x.ParentKeyColumn + " = " + x.ParentKeyValue).ToArray());
            return result + " , ";
        }

        static void WriteHeader(ref StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using Manifest.Common.Enums;");
            sb.AppendLine("using FluentMigrator;");
            sb.AppendLine();
            sb.AppendLine("namespace Manifest.Common.Utils");
            sb.AppendLine("{");
            sb.AppendLine("    public class EnumSelectUtils");
            sb.AppendLine("    {");
        }

        static void WriteFooter(ref StringBuilder sb)
        {
            sb.AppendLine("    }");
            sb.AppendLine("}");
        }
    }
}
