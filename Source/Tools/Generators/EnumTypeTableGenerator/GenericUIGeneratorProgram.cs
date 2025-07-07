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
    public static class GenericUIGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            var tableName = args[1];
            var theme = args[2];
            var type = args[3]?.ToLower();

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

           if(!getGenericUITypes().Contains(type))
            {
                throw new Exception($"Unable to find the UI Type {type}");
            }


            var camelCaseSplitName = tableName.SplitCamelCase("-").ToLower();

            var baseModalDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Source\\Web\\Web\\src\\app\\shared\\modals\\" + camelCaseSplitName;


            if (type == "import")
            {
                var modalDir = baseModalDir + "-csv-import-modal\\";

                Generate(t, st, tableName, "UIGeneric\\import\\csv-import-modal.component.html."+ theme + ".cshtml", $"{camelCaseSplitName}-csv-import-modal.component.html", modalDir);
                Generate(t, st, tableName, "UIGeneric\\import\\csv-import-modal.component.ts.cshtml", $"{camelCaseSplitName}-csv-import-modal.component.ts", modalDir);

            } else if(type == "inlinecrud")
            {
                var modalDir = baseModalDir + "-inline-crud-modal\\";

                Generate(t, st, tableName, "UIGeneric\\InlineCrud\\inline-crud-modal.component.html." + theme + ".cshtml", $"{camelCaseSplitName}-inline-crud-modal.component.html", modalDir);
                Generate(t, st, tableName, "UIGeneric\\InlineCrud\\inline-crud-modal.component.ts.cshtml", $"{camelCaseSplitName}-inline-crud-modal.component.ts", modalDir);
            }
            else if(type == "multicrud") {

                var modalDir = baseModalDir + "-multi-crud-modal\\";

                Generate(t, st, tableName, "UIGeneric\\MultiCrud\\multi-crud-modal.component.html." + theme + ".cshtml", $"{camelCaseSplitName}-multi-crud-modal.component.html", modalDir);
                Generate(t, st, tableName, "UIGeneric\\MultiCrud\\multi-crud-modal.component.ts.cshtml", $"{camelCaseSplitName}-multi-crud-modal.component.ts", modalDir);
            }
             else if(type == "multiselection") {

               var modalDir = baseModalDir + "-multi-selection-modal\\";

                Generate(t, st, tableName, "UIGeneric\\MultiSelection\\multi-selection-modal.component.html." + theme + ".cshtml", $"{camelCaseSplitName}-multi-selection-modal.component.html", modalDir);
                Generate(t, st, tableName, "UIGeneric\\MultiSelection\\multi-selection-modal.component.ts.cshtml", $"{camelCaseSplitName}-multi-selection-modal.component.ts", modalDir);
            }

        }

        static Dictionary<string, Type> cache = new Dictionary<string, Type>();
        static GenericUIGeneratorProgram()
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

        public static List<string> getGenericUITypes()
        {

            var list = new List<string>();
            list.Add("importcrud");
            list.Add("inlinecrud");
            list.Add("multicrud");
            list.Add("multiselection");
            return list; 
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
