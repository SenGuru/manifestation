using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class UIEnumPillGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Parameter should be <EnumName> <TsClassNameToBeUsed> <theme - velzon | tabler>");
                return;
            }

            Console.WriteLine(GetEnumFunction(args[1], args[3]));
            Console.WriteLine(GetUsage(args[1], args[2], args[3]));
        }

        private static string GetUsage(string enumName, string className, string theme)
        {

            var typeNameWithCaps = enumName.EndsWith("s") ? enumName.Substring(0, enumName.Length - "s".Length) : enumName;
            var typeNameWithFirstSmall = typeNameWithCaps;
            typeNameWithFirstSmall = typeNameWithFirstSmall[0].ToString().ToLower() + typeNameWithFirstSmall.Substring(1);

            var classNameFirstSmall = className[0].ToString().ToLower() + className.Substring(1);

            var sb = new StringBuilder();
            if (theme == "velzon")
            {
                sb.AppendLine($" <span class=\"badge badge bg-{{{{enumViewService.get{typeNameWithCaps}Class({classNameFirstSmall}?.{typeNameWithFirstSmall}Id)}}}}-subtle text-{{{{enumViewService.get{typeNameWithCaps}Class({classNameFirstSmall}?.{typeNameWithFirstSmall}Id)}}}}\">");
                sb.AppendLine($"	{{{{{classNameFirstSmall}?.{typeNameWithFirstSmall}Name}}}}");
                sb.AppendLine($" </span>");
            }
            else if (theme == "tabler")
            {
                throw new NotImplementedException("Tabler Implementation is not done");
            }
            else
            {
                throw new NotImplementedException($"Provided Theme is empty or not implemented - {theme}");
            }
            return sb.ToString();
        }

        private static Type GetEnum(string enumName)
        {
            return CodeGenUtils.GetClassFromAssembly(typeof(Manifest.Common.Constants.ConfigurationConstants).Assembly, enumName);
        }

        private static string GetEnumFunction(string enumTypeName, string theme)
        {
            int stringIndex = 0; // Index for cycling through the string list

            var stringList = GetColorClass();

            var sb = new StringBuilder();

            Type enumType = GetEnum(enumTypeName);
            if (enumType == null)
                throw new Exception($"{enumTypeName} is not a valid type");

            if (!enumType.IsEnum)
                throw new Exception($"{enumTypeName} is not an enum");

            var typeNameWithCaps = enumTypeName.EndsWith("s") ? enumTypeName.Substring(0, enumTypeName.Length - "s".Length) : enumTypeName;

            sb.AppendLine($" public get{typeNameWithCaps}Class(typeId: number): string {{");
            sb.AppendLine($"  switch (typeId) {{");
            foreach (var e in Enum.GetValues(enumType))
            {
                sb.AppendLine($"    case {enumTypeName}.{e.ToString()}:");
                sb.AppendLine($"      return \'{stringList[stringIndex]}\';");
                stringIndex = (stringIndex + 1) % stringList.Count;
            }
            sb.AppendLine($"    default:");
            sb.AppendLine($"      return 'primary';");
            sb.AppendLine($"    }}");
            sb.AppendLine($"}}");

            return sb.ToString();
        }

        static List<string> GetColorClass()
        {
            var result = new List<string>();
            result.Add("success");
            result.Add("primary");
            result.Add("secondary");
            result.Add("danger");
            result.Add("info");
            result.Add("warning");
            return result;
        }
    }
}