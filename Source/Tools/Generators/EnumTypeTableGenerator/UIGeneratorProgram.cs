/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using RazorLight;
using Framework.Infrastructure.Utils;
using EnumTypeTableGenerator.Models;

namespace CodeGenerator
{
    public static class UIGeneratorProgram
    {
        public static void MainApp(string[] args, bool isCredList)
        {
            var tableName = args[1];
            var theme = args[2];

            var t = GetTableType(tableName);
            if (t == null)
                throw new Exception($"Unable to find the Type for table name - {tableName}");

            var st = GetTableType(tableName + "SearchCriteria");
            if (st == null)
            {
                if (tableName.EndsWith("Model"))
                {
                    var searchName = tableName.Substring(0, tableName.Length - "Model".Length);
                    st = GetTableType(searchName + "SearchCriteria");
                }

                if (st == null)
                    throw new Exception($"Unable to find the Search Criteria Type for table name - {tableName + "SearchCriteria"}");
            }

            var camelCaseSplitName = tableName.SplitCamelCase("-").ToLower();
            var listDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Source\\Web\\Web\\src\\app\\shared\\components\\screen\\" + camelCaseSplitName + "-list-screen\\";

            if (!isCredList)
            {
                var modalDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Source\\Web\\Web\\src\\app\\shared\\modals\\" + camelCaseSplitName + "-crud-modal\\";
                //var superListDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\ui-code-all\\projects\\shared-library\\src\\lib\\shared\\components\\screens\\super-admin-" + camelCaseSplitName + "-screen\\";

                Generate(t, st, tableName, "UI\\crud-modal.component.html."+ theme + ".cshtml", $"{camelCaseSplitName}-crud-modal.component.html", modalDir);
                Generate(t, st, tableName, "UI\\crud-modal.component.ts.cshtml", $"{camelCaseSplitName}-crud-modal.component.ts", modalDir);

                Generate(t, st, tableName, "UI\\list-screen.component.html." + theme + ".cshtml", $"{camelCaseSplitName}-list-screen.component.html", listDir);
                Generate(t, st, tableName, "UI\\list-screen.component.ts.cshtml", $"{camelCaseSplitName}-list-screen.component.ts", listDir);

                //Generate(t, st, tableName, "UI\\super-admin.component.html" + theme + ".cshtml", $"super-admin-{camelCaseSplitName}-screen.component.html", superListDir);
                //Generate(t, st, tableName, "UI\\super-admin.component.ts.cshtml", $"super-admin-{camelCaseSplitName}-screen.component.ts", superListDir);
            }
            else
            {
                Generate(t, st, tableName, "UI\\cred-list-screen.component.html.cshtml", $"{camelCaseSplitName}-list-screen.component.html", listDir);
                Generate(t, st, tableName, "UI\\cred-list-screen.component.ts.cshtml", $"{camelCaseSplitName}-list-screen.component.ts", listDir);
            }
        }

        static Dictionary<string, Type> cache = new Dictionary<string, Type>();
        static UIGeneratorProgram()
        {
            var t = typeof(Manifest.Common.Constants.ConfigurationConstants);

            var assembly = t.Assembly;
            foreach (var aClass in assembly.GetTypes())
            {
                if (!aClass.FullName.StartsWith("Manifest.Common"))
                    continue;

                if (cache.ContainsKey(aClass.Name.Trim().ToLower()))
                    continue;

                cache.Add(aClass.Name.Trim().ToLower(), aClass);
            }
        }

        public static bool GetHasIdOrIds(Type st, string entityName)
        {
            bool result = false;
            foreach(var p in st.GetProperties())
            {
                if (p.Name == (entityName+"Id"))
                {
                    continue;
                }
                else if (CodeGenUtils.IsPropertyIgnoreForSearchFilter(p.Name))
                {
                    continue;
                }
                else if ((p.Name == "StateId") || (p.Name == "CountryId") || (p.Name.StartsWith("Predefined") && p.Name.EndsWith("TypeId")) || (p.Name.StartsWith("Predefined") && p.Name.EndsWith("TypeIds")) || (p.Name.StartsWith("Predefined") && p.Name.EndsWith("Id")))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static string Generate(Type t, Type st, string tableName, string templateFile, string sourceFile, string folder)
        {
            var templateFileLocation = GetTemplateFileLocation(templateFile);
            var sourceFileLocation = GetSourceFileLocation(sourceFile, folder);

            var capsFirstLetterString = tableName[0].ToString().ToUpper() + tableName.Substring(1);
            var lowerFirstLetterString = tableName[0].ToString().ToLower() + tableName.Substring(1);

            var capsSearchFirstLetterString = st.Name[0].ToString().ToUpper() + st.Name.Substring(1);
            var lowerSearchFirstLetterString = st.Name[0].ToString().ToLower() + st.Name.Substring(1);

            var smallDashEntityName = StringUtils.SplitCamelCase(tableName, "-").ToLower();

            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            var hasIdOrIds = GetHasIdOrIds(st, tableName);

            try
            {
                var model = new UiTemplateData { SearchHasIdOrIds = hasIdOrIds, SplitetCapitalizeEntityName = capsFirstLetterString.SplitCamelCase(), CapitalizeEntityName = capsFirstLetterString, SmallDashEntityName = smallDashEntityName, SmallEntityName = lowerFirstLetterString, CapitalizeSearchName = capsSearchFirstLetterString, SmallSearchName = lowerSearchFirstLetterString, EntityType = t, SearchCriteriaType = st };
                var engine = new RazorLightEngineBuilder()
                                .AddDefaultNamespaces("Framework.Infrastructure.Utils")
                                .AddDefaultNamespaces("Framework.Infrastructure")
                                .UseFileSystemProject(Path.GetDirectoryName(templateFileLocation), Path.GetExtension(templateFileLocation))
                                .UseMemoryCachingProvider()
                                .UseOptions(new RazorLightOptions { EnableDebugMode = true })
                                .Build();

                string result = engine.CompileRenderAsync(Path.GetFileName(templateFileLocation), model).Result;

                File.WriteAllText(sourceFileLocation, result);

            }
            catch
            {
                Console.WriteLine("Error in generating file - " + templateFileLocation + " \n " + sourceFileLocation);
                throw;
            }
            //Process.Start("notepad.exe", sourceFileLocation);
            return sourceFileLocation;
        }

        public static Type GetTableType(string tableName)
        {
            if (cache.TryGetValue(tableName.Trim().ToLower(), out Type t))
                return t;
            return null;
        }

        public static string GetTemplateFileLocation(string templateFile)
        {
            var str = Directory.GetCurrentDirectory() + "\\CrudTemplate\\";
            if (!Directory.Exists(str))
                throw new Exception($"Template directory does not exists in {str}");
            str = str + templateFile;
            if (!File.Exists(str))
                throw new Exception($"Template File does not exists in {str}");
            return str;
        }

        public static string GetSourceFileLocation(string sourceFile, string folder = null)
        {
            var str = folder;
            if (folder == null)
                str = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Backend\\Api\\All\\WIP\\";

            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            return str + sourceFile;

        }
    }
}
