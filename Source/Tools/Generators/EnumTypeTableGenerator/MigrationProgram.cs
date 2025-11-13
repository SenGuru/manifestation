/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Attributes;
using Framework.Infrastructure.Utils;
using Manifest.Common.Enums;
using Manifest.Common.Models.DB;

namespace CodeGenerator
{
    public static class MigrationProgram
    {
        public static void MainApp()
        {
            var fileName = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Repository\\Migration\\AppDBMigrationSteps\\Mig_002.cs";

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
                if (!aClass.FullName.StartsWith("Manifest.Common.EnumsXXX"))
                    continue;

                if (!aClass.IsEnum)
                {
                    sb.AppendLine(aClass.FullName + " is not enum");
                    continue;
                }

                var alst = aClass.GetCustomAttributes(typeof(IgnoredTypeTableAttribute), true);
                if (alst != null && alst.Length > 0)
                {
                    continue;
                }

                alst = aClass.GetCustomAttributes(typeof(TypeTableAttribute), true);
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

                sb.AppendLine($"            dynamic[] {camelCaseClass}Params =");
                sb.AppendLine("            {");
                foreach (var enumName in System.Enum.GetNames(aClass))
                {
                    var memberInfos = aClass.GetMember(enumName);
                    var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == aClass);
                    var pKeyValue = "";
                    //var pKeyIntValue = 0L;
                    var pkeyLst = enumValueMemberInfo.GetCustomAttributes(typeof(ParentKeyAttribute), true);
                    if (pkeyLst != null && pkeyLst.Length > 0)
                    {
                        var pk = GetPKeyValue(pkeyLst.Select(x => x as ParentKeyAttribute));
                        pKeyValue = pk.commaString;
                        //pKeyIntValue = pk.pkValue;
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

                    sb.AppendLine($"                new {{ {tableName}Id = (int){aClass}.{enumName},  {tableName}Name = \"{newEnumName}\", {pKeyValue} CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }}, ");
                }
                sb.AppendLine("            };");
                sb.AppendLine($"");
                sb.AppendLine($"                InsertData(\"{tableName}\", {camelCaseClass}Params);");
            }

            sb.AppendLine($"////////////////////////////////////////////////////////");
            //sb.Clear();
            //Get the EnumGroup and loop through it and write the Value
            sb.AppendLine($"            dynamic[] enumGroupParams =");
            sb.AppendLine("            {");
            foreach (var value in Enum.GetValues(typeof(EnumGroupTypes)))
            {
                var enumGroupId = value;
                var enumGroupName = Enum.GetName(typeof(EnumGroupTypes), value);

                sb.AppendLine($"                new {{ EnumGroupId = (int)EnumGroupTypes.{enumGroupId},  EnumGroupName = \"{enumGroupName}\", IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }}, ");
            }

            sb.AppendLine("            };");
            sb.AppendLine($"                InsertData(\"EnumGroup\", enumGroupParams);");
            sb.AppendLine($"");
            sb.AppendLine($"            dynamic[] enumTypeParams =");
            sb.AppendLine("            {");

            foreach (var value in Enum.GetValues(typeof(EnumGroupTypes)))
            {
                var enumName = Enum.GetName(typeof(EnumGroupTypes), value);
                var enumType = assembly.GetTypes().FirstOrDefault(x => x.FullName.StartsWith("Manifest.Common.Enums") && x.IsEnum && (x.Name == enumName || x.Name == enumName + "s"));
                if (enumType == null)
                    throw new Exception($"Unable to create Type for Enum - {enumName}");

                foreach (var etValue in Enum.GetValues(enumType))
                {
                    var enumTypeId = ((int)value * 100000) + (int)etValue;
                    var enumTypeName = Enum.GetName(enumType, etValue);

                    var enumData = GetEnumData(enumType, enumTypeName);

                    var enumValue = (int)etValue;
                    var enumGroupId = value;
                    var pKeyValue = "";
                    if (!enumData.PKeyValue.IsTrimmedStringNullOrEmpty())
                    {
                        pKeyValue = ", ParentEnumTypeId = " + enumData.PKeyIntValue;
                    }

                    sb.AppendLine($"                new {{ EnumTypeId = (int){enumName}s.{enumTypeName},  EnumGroupId = (int)EnumGroupTypes.{enumGroupId}, EnumTypeName = \"{enumData.EName}\"{pKeyValue}, InternalValue = {enumValue} , IsDeleted = false, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }}, ");
                }
            }

            sb.AppendLine("            };");
            sb.AppendLine($"                InsertData(\"EnumType\", enumTypeParams);");
            /*
            sb.AppendLine("////////////////////////////////");
            foreach (var value in Enum.GetValues(typeof(EnumGroupTypes)))
            {
                var enumName = Enum.GetName(typeof(EnumGroupTypes), value);
                var enumType = assembly.GetTypes().FirstOrDefault(x => x.FullName.StartsWith("Manifest.Common.Enums") && x.IsEnum && (x.Name == enumName || x.Name == enumName + "s"));
                if (enumType == null)
                    throw new Exception($"Unable to create Type for Enum - {enumName}");
                sb.AppendLine($"///public enum {enumName}");
                sb.AppendLine("///{");

                foreach (var etValue in Enum.GetValues(enumType))
                {
                    var enumTypeId = ((int)value * 100000) + (int)etValue;
                    var enumTypeName = Enum.GetName(enumType, etValue);

                    var enumData = GetEnumData(enumType, enumTypeName);

                    var enumValue = (int)etValue;
                    var enumGroupId = value;
                    sb.AppendLine($"///{etValue} = {enumTypeId},");
                }
                sb.AppendLine("///}");
            }

            sb.AppendLine("////////////////////////////////");
            */
            WriteFooter(ref sb);

            File.AppendAllText(fileName, sb.ToString());

            //Process.Start("notepad.exe", fileName);

        }

        static (string EName, string PKeyValue, long PKeyIntValue) GetEnumData(Type aClass, string enumName)
        {
            var memberInfos = aClass.GetMember(enumName);
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == aClass);
            var pKeyValue = "";
            var pKeyIntValue = 0L;

            (string commaString, string pkName, long pkValue) pk;
            var pkeyLst = enumValueMemberInfo.GetCustomAttributes(typeof(ParentKeyAttribute), true);
            if (pkeyLst != null && pkeyLst.Length > 0)
            {
                pk = GetPKeyValue(pkeyLst.Select(x => x as ParentKeyAttribute),includeLastComma: false);
                pKeyValue = pk.commaString;
                pKeyIntValue = pk.pkValue;
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

            return (newEnumName, pKeyValue, pKeyIntValue);
        }

        static (string commaString,string pkName, long pkValue) GetPKeyValue(IEnumerable<ParentKeyAttribute> pkAttList, bool includeLastComma = true)
        {
            var pkOne = pkAttList.FirstOrDefault();
            var result = string.Join(",", pkAttList.Select(x => x.ParentKeyColumn + " = " + x.ParentKeyValue).ToArray());
            return (result + (includeLastComma ? " , " : ""), pkOne != null ? pkOne.ParentKeyColumn : "", pkOne != null ? pkOne.ParentKeyValue : 0);
        }

        static void WriteHeader(ref StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using Manifest.Common.Enums;");
            sb.AppendLine("using FluentMigrator;");
            sb.AppendLine();
            sb.AppendLine("#pragma warning disable SA1028");
            sb.AppendLine("#pragma warning disable S101 // Types should be named in PascalCase");
            sb.AppendLine("#pragma warning disable SA1137");
            sb.AppendLine("namespace Manifest.Repository.Migration.Application");
            sb.AppendLine("{");
            sb.AppendLine("    [Migration(002)]");
            sb.AppendLine("    public class Mig_002 : BaseMigration");
            sb.AppendLine("    {");
            sb.AppendLine("        public override void Up()");
            sb.AppendLine("        {");
        }

        static void WriteFooter(ref StringBuilder sb)
        {
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("#pragma warning restore SA1137");
            sb.AppendLine("#pragma warning restore S101 // Types should be named in PascalCase");
            sb.AppendLine("#pragma warning restore SA1028");
        }
    }
}
