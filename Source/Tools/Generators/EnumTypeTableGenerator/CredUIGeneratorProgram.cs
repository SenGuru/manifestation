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
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.EMMA;
using System.Diagnostics;

namespace CodeGenerator
{
    public static class CredUIGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            var filePath = args[1];

            if (!File.Exists(filePath))
                throw new Exception($"Unable to find CSV file at {filePath}");

            //var camelCaseSplitName = "";            
            //var modalDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\ui-code-all\\projects\\shared-library\\src\\lib\\shared\\modals\\" + camelCaseSplitName + "-crud-modal\\";
            var migDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Repository\\Migration\\AppDBMigrationSteps\\";
            var csCreateUpdateModelDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Common\\Models\\ViewModels\\CredUI\\";
            //var csViewModelDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Common\\Models\\ViewModels\\CredUI\\";
            var controllDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Api\\All\\Controllers\\";
            var csServiceDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Service\\Completed\\App\\";
            var csRepoDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Repository\\Common\\";

            var tsModalDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\ui-code-all\\projects\\shared-library\\src\\lib\\shared\\modals\\";
            var tsCompDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\ui-code-all\\projects\\shared-library\\src\\lib\\shared\\components\\screens\\case-screens\\";

            var c = CredUIHelper.GetModel(filePath);

            //GenerateAngularDialogList(c, "DialogSummary.ts.cshtml", "DialogSummary.ts.html.cshtml", tsCompDir);

            Generate(c, "CredScreenTemplate\\CsMig_006.cs.cshtml", $"Mig_006.cs.txt", migDir);
            Generate(c, "CredScreenTemplate\\CsCreateUpdateModel.cs.cshtml", $"CredCrudModels.cs.txt", csCreateUpdateModelDir);
            Generate(c, "CredScreenTemplate\\CsViewModel.cs.cshtml", $"CredViewModels.cs.txt", csCreateUpdateModelDir);
            Generate(c, "CredScreenTemplate\\CsController.cs.cshtml", $"CredController.UI.cs.txt", controllDir);
            Generate(c, "CredScreenTemplate\\CsServices.cs.cshtml", $"CredService.UI.cs.txt", csServiceDir);
            Generate(c, "CredScreenTemplate\\CsRepo.cs.cshtml", $"CredRepository.UI.cs.txt", csRepoDir);


            //GenerateAngularForAllComponent(c, "AngularDialogComponent.ts.cshtml", "AngularDialogComponent.html.cshtml", "AngularMainComponent.ts.cshtml", "AngularMainComponent.html.cshtml", tsModalDir, tsCompDir);

            //GenerateAngularForAllComponent(c, "AngularDialogComponent.ts.cshtml", "AngularDialogComponent.html.cshtml", "AngularMainComponent.ts.cshtml", "AngularMainComponent.html.cshtml", tsModalDir, tsCompDir,1);
            //GenerateAngularForAllComponent(c, "AngularDialogComponent.ts.cshtml", "AngularDialogComponent.html.cshtml", "AngularMainComponent.ts.cshtml", "AngularMainComponent.html.cshtml", tsModalDir, tsCompDir, 2);

            //Generate(c, "CredScreenTemplate\\TsViewModel.ts.cshtml", $"cred-view-models.ts", tsModalDir);
            //Generate(c, "CredScreenTemplate\\TsCreateUpdateModel.ts.cshtml", $"cred-create-update-models.ts", tsModalDir);
            //Generate(c, "CredScreenTemplate\\TsServices.ts.cshtml", $"cred-service.ts", tsModalDir);
        }

        static Dictionary<string, Type> cache = new();
        static CredUIGeneratorProgram()
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


        public static string Generate(Code c, string templateFile, string sourceFile, string folder)
        {
            var templateFileLocation = GetTemplateFileLocation(templateFile);
            var sourceFileLocation = GetSourceFileLocation(sourceFile, folder);

            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            try
            {
                var model = new CredUiTemplateData { Code = c };
                var engine = new RazorLightEngineBuilder()
                                .AddDefaultNamespaces("Framework.Infrastructure.Utils")
                                .AddDefaultNamespaces("Framework.Infrastructure")
                                .UseFileSystemProject(Path.GetDirectoryName(templateFileLocation), Path.GetExtension(templateFileLocation))
                                .UseMemoryCachingProvider()

                                .Build();

                string result = engine.CompileRenderAsync(Path.GetFileName(templateFileLocation), model).Result;

                //Console.WriteLine(result);

                File.WriteAllText(sourceFileLocation, result);
            }
            catch
            {
                Console.WriteLine("Error in generating file - " + templateFileLocation + " \n " + sourceFileLocation);
                throw;
            }
            //System.Diagnostics.Process.Start("notepad.exe", sourceFileLocation);
            return sourceFileLocation;
        }

        public static string GenerateAngularDialogListInternal(List<(string Name, string DashName)> c, string templateFileLocation, string sourceFileLocation)
        {
            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            try
            {
                var engine = new RazorLightEngineBuilder()
                                .AddDefaultNamespaces("Framework.Infrastructure.Utils")
                                .AddDefaultNamespaces("Framework.Infrastructure")
                                .UseFileSystemProject(Path.GetDirectoryName(templateFileLocation), Path.GetExtension(templateFileLocation))
                                .UseMemoryCachingProvider()

                                .Build();

                string result = engine.CompileRenderAsync(Path.GetFileName(templateFileLocation), c).Result;

                //Console.WriteLine(result);

                File.WriteAllText(sourceFileLocation, result);
            }
            catch
            {
                Console.WriteLine("Error in generating file - " + templateFileLocation + " \n " + sourceFileLocation);
                throw;
            }
            //System.Diagnostics.Process.Start("notepad.exe", sourceFileLocation);
            return sourceFileLocation;
        }


        public static string GenerateAngularInternal(Code c, Section currentSection, Table t, string templateFileLocation, string sourceFileLocation)
        {
            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            try
            {
                var model = new CredUiTemplateData { Code = c, CSection = currentSection, CTable = t };
                var engine = new RazorLightEngineBuilder()
                                .AddDefaultNamespaces("Framework.Infrastructure.Utils")
                                .AddDefaultNamespaces("Framework.Infrastructure")
                                .UseFileSystemProject(Path.GetDirectoryName(templateFileLocation), Path.GetExtension(templateFileLocation))
                                .UseMemoryCachingProvider()

                                .Build();

                string result = engine.CompileRenderAsync(Path.GetFileName(templateFileLocation), model).Result;

                //Console.WriteLine(result);

                File.WriteAllText(sourceFileLocation, result);
            }
            catch
            {
                Console.WriteLine("Error in generating file - " + templateFileLocation + " \n " + sourceFileLocation);
                throw;
            }
            //System.Diagnostics.Process.Start("notepad.exe", sourceFileLocation);
            return sourceFileLocation;
        }

        public static void GenerateAngularDialogList(Code c, string templateDialogTsFile, string templateDialogHtmlFile, string destinationDialogFolder)
        {
            var lst = new List<(string Name, string DashName)>();

            foreach (var s in c.Sections)
            {
                foreach (var col in s.AllColumns)
                {
                    if (!col.IsArray || col.DbColumnName.EndsWith("MetaDataIds"))
                    {
                        continue;
                    }

                    var dash = "case-" + col.ParentTable.DbLowerTableNameWithDash;

                    lst.Add((col.ParentTable.DbTableName, dash));
                }
            }

            var templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateDialogTsFile);
            var sourceFileLocation = GetSourceFileLocation("case-provider-test-screen.component.ts", destinationDialogFolder + "\\case-provider-test-screen\\");
            GenerateAngularDialogListInternal(lst, templateFileLocation, sourceFileLocation);

            templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateDialogHtmlFile);
            sourceFileLocation = GetSourceFileLocation("case-provider-test-screen.component.html", destinationDialogFolder + "\\case-provider-test-screen\\");
            GenerateAngularDialogListInternal(lst, templateFileLocation, sourceFileLocation);
        }


        public static void GenerateAngularForAllComponent(Code c, string templateDialogTsFile, string templateDialogHtmlFile, string templateComponentTsFile, string templateComponentHtmlFile, string destinationDialogFolder, string destinationComponentFolder, int? optionalPage = null)
        {
            int i = 0;

            foreach (var s in c.Sections)
            {
                i++;
                if (optionalPage != null)
                {
                    if (optionalPage != i)
                    {
                        continue;
                    }
                }
                GenerateAngular(c, s, templateDialogTsFile, templateDialogHtmlFile, templateComponentTsFile, templateComponentHtmlFile, destinationDialogFolder, destinationComponentFolder);
            }
        }


        public static void GenerateAngular(Code c, Section currentSection, string templateDialogTsFile, string templateDialogHtmlFile, string templateComponentTsFile, string templateComponentHtmlFile, string destinationDialogFolder, string destinationComponentFolder)
        {
            string sourceFile;
            string sourceFileLocation;
            string templateFileLocation;

            /*
            foreach (var col in currentSection.AllColumns)
            {
                if (!col.IsArray || col.DbColumnName.EndsWith("MetaDataIds"))
                {
                    continue;
                }

                sourceFile = "case-" + col.ParentTable.DbLowerTableNameWithDash;                

                templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateDialogTsFile);
                sourceFileLocation = GetSourceFileLocation("add-"+sourceFile + "-modal.component.ts", destinationDialogFolder+"\\"+ "add-" + sourceFile + "-modal"+"\\");

                GenerateAngularInternal(c, currentSection, col.ParentTable, templateFileLocation, sourceFileLocation);

                templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateDialogHtmlFile);
                sourceFileLocation = GetSourceFileLocation("add-" + sourceFile + "-modal.component.html", destinationDialogFolder + "\\" + "add-" + sourceFile + "-modal" + "\\");

                GenerateAngularInternal(c, currentSection, col.ParentTable, templateFileLocation, sourceFileLocation);
            }
            */

            /*
            foreach(var s in currentSection.SubSections)
            {
                Console.WriteLine($"{s.SubSectionName}");
                foreach(var cx in s.SectionColumnsX)
                {
                    Console.WriteLine($"    C - {cx.Tag} - {cx.DbColumnName}");
                }
            }
            */
            sourceFile = "case-" + currentSection.ViewLowerClassNameWithDash;
            var sourceFileDir = currentSection.ViewLowerClassNameWithDash;

            templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateComponentTsFile);
            sourceFileLocation = GetSourceFileLocation(sourceFile + "-screen.component.ts", destinationComponentFolder + "\\" + sourceFileDir + "\\");

            GenerateAngularInternal(c, currentSection, null, templateFileLocation, sourceFileLocation);

            templateFileLocation = GetTemplateFileLocation("CredScreenTemplate\\" + templateComponentHtmlFile);
            sourceFileLocation = GetSourceFileLocation(sourceFile + "-screen.component.html", destinationComponentFolder + "\\" + sourceFileDir + "\\");

            GenerateAngularInternal(c, currentSection, null, templateFileLocation, sourceFileLocation);

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
