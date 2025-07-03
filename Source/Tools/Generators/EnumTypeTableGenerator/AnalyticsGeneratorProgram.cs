/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public class ParamClass
    {
        public string name { get; set; }
        public string returnModel { get; set; }
        public string splittedFileName { get; set; }
        public string typeName { get; set; }
        public string typeOrEmptyForTypeInName { get; set; }
        public string weeklyFunctionName { get; set; }
        public string dailyFunctionName { get; set; }
        public string monthlyFunctionName { get; set; }
        public string yearlyFunctionName { get; set; }
        public string modelListFunctionName { get; set; }
        public string statsClassName { get; set; }
        public Type statsClass { get; set; }
        public string statsPieChartStatements { get; set; }
        public string statsBarChartStatements { get; set; }

        public string returnModelWithSmallFirstLetter
        {
            get
            {
                return returnModel.MakeFirstCharAsLower();
            }
        }

        public string nameWithSpaceSeperation
        {
            get
            {
                return name.SplitCamelCase();
            }
        }

        public string returnModelSmall
        {
            get
            {
                return returnModel.ToLower();
            }
        }

        public string nameWithSmallFirstLetter
        {
            get
            {
                return name.MakeFirstCharAsLower();
            }
        }
    }

    public static class AnalyticsGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Parameter should be <Name> <ReturnModel>");
                return;
            }

            var param = new ParamClass();

            param.name = args[1];
            param.returnModel = args[2];
            param.splittedFileName = ConverToSplittedName(args[1]);
            //param.typeName = "";
            //if (!param.name.EndsWith("Type"))
            param.typeName = "Type";

            param.weeklyFunctionName = $"GetWeekly{param.returnModel}sBy{param.name}{param.typeName}AnalyticsReport";
            var t = GetFunctionValue(param.weeklyFunctionName);
            if (t == null)
            {
                param.weeklyFunctionName = $"GetWeekly{param.returnModel}sBy{param.name}AnalyticsReport";
                t = GetFunctionValue(param.weeklyFunctionName);
                if (t == null)
                    throw new Exception($"Unable to find the Type for function - {param.weeklyFunctionName}");
                else
                {
                    param.typeOrEmptyForTypeInName = "Type";
                    param.typeName = "";
                }
            }

            param.dailyFunctionName = $"GetDaily{param.returnModel}sBy{param.name}{param.typeName}AnalyticsReport";
            t = GetFunctionValue(param.dailyFunctionName);
            if (t == null)
            {
                param.dailyFunctionName = $"GetDaily{param.returnModel}sBy{param.name}AnalyticsReport";
                t = GetFunctionValue(param.dailyFunctionName);
                if (t == null)
                    throw new Exception($"Unable to find the Type for function - {param.dailyFunctionName}");
            }

            param.monthlyFunctionName = $"GetMonthly{param.returnModel}sBy{param.name}{param.typeName}AnalyticsReport";
            t = GetFunctionValue(param.monthlyFunctionName);
            if (t == null)
            {
                param.monthlyFunctionName = $"GetMonthly{param.returnModel}sBy{param.name}AnalyticsReport";
                t = GetFunctionValue(param.monthlyFunctionName);
                if (t == null)
                    throw new Exception($"Unable to find the Type for function - {param.monthlyFunctionName}");
            }

            param.yearlyFunctionName = $"GetYearly{param.returnModel}sBy{param.name}{param.typeName}AnalyticsReport";
            t = GetFunctionValue(param.yearlyFunctionName);
            if (t == null)
            {
                param.yearlyFunctionName = $"GetYearly{param.returnModel}sBy{param.name}AnalyticsReport";
                t = GetFunctionValue(param.yearlyFunctionName);
                if (t == null)
                    throw new Exception($"Unable to find the Type for function - {param.yearlyFunctionName}");
            }



            param.modelListFunctionName = $"get{param.returnModel}sListBy{param.name}{param.typeName}AnalyticsReport";
            t = GetFunctionValue(param.modelListFunctionName);
            if (t == null)
            {
                param.modelListFunctionName = $"get{param.returnModel}sListBy{param.name}AnalyticsReport";
                t = GetFunctionValue(param.modelListFunctionName);

                if (t == null)
                    throw new Exception($"Unable to find the Type for function - {param.modelListFunctionName}");
            }

            param.statsClassName = $"{param.returnModel}sBy{param.name}{param.typeName}StatsModel";
            param.statsClass = GetClassValue(param.statsClassName);
            if (param.statsClass == null)
                throw new Exception($"Unable to find the Stats Model - {param.statsClassName}");

            param.statsPieChartStatements = GetPieChartStatements(param.statsClass);
            param.statsBarChartStatements = GetBarChartStatements(param.statsClass);

            var modelDir = Directory.GetCurrentDirectory() + $"\\..\\..\\..\\..\\Web\\src\\app\\shared\\components\\screens\\analytics-screens\\analytics-{param.returnModelSmall}-by-{param.splittedFileName}-screen\\";
            if (!Directory.Exists(modelDir))
            {
                Directory.CreateDirectory(modelDir);
            }

            Generate(param, "Analytics.html.template", $"analytics-{param.returnModelSmall}-by-{param.splittedFileName}-screen.html", modelDir);
            Generate(param, "Analytics.ts.template", $"analytics-{param.returnModelSmall}-by-{param.splittedFileName}-screen.ts", modelDir);
            Generate(param, "AnalyticsChart.ts.template", $"{param.splittedFileName}-chart.ts", modelDir);
        }

        private static Type GetClassValue(string className)
        {
            className = className.ToLower();

            if (!typeCache.ContainsKey(className))
                return null;

            return typeCache[className];
        }

        private static string GetPieChartStatements(Type statsClass)
        {
            var sb = new StringBuilder();
            foreach (var prop in statsClass.GetProperties())
            {
                if (prop.Name == "Day" || prop.Name == "Week" || prop.Name == "Month" || prop.Name == "Year" || prop.Name.StartsWith("Total"))
                    continue;

                sb.AppendLine($"    if (item.{prop.Name.MakeFirstCharAsLower()} > 0) {{");
                sb.AppendLine($"      chartSeriesLabel.push(\'{GetLabelName(prop.Name)}\');");
                sb.AppendLine($"      chartSeriesData.push(item.{prop.Name.MakeFirstCharAsLower()});");
                sb.AppendLine($"    }}");
            }
            return sb.ToString();
        }

        private static string GetLabelName(string name)
        {
            var str = name.SplitCamelCase();
            if (str.EndsWith(" Count"))
                str = str.Replace(" Count", "");
            return str;
        }

        private static string GetBarChartStatements(Type statsClass)
        {
            var sb = new StringBuilder();
            foreach (var prop in statsClass.GetProperties())
            {
                if (prop.Name == "Day" || prop.Name == "Week" || prop.Name == "Month" || prop.Name == "Year")
                    continue;

                sb.AppendLine($"    let {prop.Name.MakeFirstCharAsLower()}List = data.map(list => list.{prop.Name.MakeFirstCharAsLower()});");
                sb.AppendLine($"    if ({prop.Name.MakeFirstCharAsLower()}List.some(x => x > 0)) {{");
                sb.AppendLine($"      chartSeries.push({{ name: \'{GetLabelName(prop.Name)}\', data: {prop.Name.MakeFirstCharAsLower()}List }});");
                sb.AppendLine($"    }}");
            }
            return sb.ToString();
        }

        private static string ConverToSplittedName(string name)
        {
            return name.SplitCamelCase("-").ToLower();
        }

        private static readonly Dictionary<string, MethodInfo> cache = new();
        private static readonly Dictionary<string, Type> typeCache = new();

        static AnalyticsGeneratorProgram()
        {
            var t = typeof(Manifest.Api.All.Controllers.AccountController);

            var assembly = t.Assembly;
            foreach (var aClass in assembly.GetTypes())
            {
                if (!aClass.FullName.EndsWith("Controller"))
                    continue;

                var members = aClass.GetMethods();
                foreach (var mem in members)
                {
                    var k = mem.Name.Trim().ToLower();
                    if (!cache.ContainsKey(k))
                        cache.Add(k, mem);

                    //Console.WriteLine(aClass.Name + " - " + k);
                }
            }

            t = typeof(Manifest.Common.Models.ViewModels.Dashboard.MarketingDashboardModel);

            assembly = t.Assembly;
            foreach (var aClass in assembly.GetTypes())
            {
                var k = aClass.Name.Trim().ToLower();
                if (!typeCache.ContainsKey(k))
                {
                    typeCache.Add(k, aClass);
                }
            }
        }

        public static string Generate(ParamClass param, string templateFile, string sourceFile, string folder)
        {
            var templateFileLocation = GetTemplateFileLocation(templateFile);
            var sourceFileLocation = GetSourceFileLocation(sourceFile, folder);

            var str = File.ReadAllText(templateFileLocation);

            str = str.Replace("%name%", param.name, StringComparison.CurrentCulture);
            str = str.Replace("%nameWithSmallFirstLetter%", param.nameWithSmallFirstLetter, StringComparison.CurrentCulture);
            str = str.Replace("%returnModel%", param.returnModel, StringComparison.CurrentCulture);
            str = str.Replace("%typeName%", param.typeName, StringComparison.CurrentCulture);
            str = str.Replace("%returnModelWithSmallFirstLetter%", param.returnModelWithSmallFirstLetter, StringComparison.CurrentCulture);
            str = str.Replace("%splittedFileName%", param.splittedFileName, StringComparison.CurrentCulture);
            str = str.Replace("%weeklyFunctionName%", param.weeklyFunctionName.MakeFirstCharAsLower(), StringComparison.CurrentCulture);
            str = str.Replace("%dailyFunctionName%", param.dailyFunctionName.MakeFirstCharAsLower(), StringComparison.CurrentCulture);
            str = str.Replace("%monthlyFunctionName%", param.monthlyFunctionName.MakeFirstCharAsLower(), StringComparison.CurrentCulture);
            str = str.Replace("%yearlyFunctionName%", param.yearlyFunctionName.MakeFirstCharAsLower(), StringComparison.CurrentCulture);
            str = str.Replace("%modelListFunctionName%", param.modelListFunctionName.MakeFirstCharAsLower(), StringComparison.CurrentCulture);
            str = str.Replace("%nameWithSpaceSeperation%", param.nameWithSpaceSeperation, StringComparison.CurrentCulture);
            str = str.Replace("%pieChartStatements%", param.statsPieChartStatements, StringComparison.CurrentCulture);
            str = str.Replace("%barChartStatements%", param.statsBarChartStatements, StringComparison.CurrentCulture);
            str = str.Replace("%typeOrEmptyForTypeInName%", param.typeOrEmptyForTypeInName, StringComparison.CurrentCulture);
            str = str.Replace("%returnModelSmall%", param.returnModelSmall, StringComparison.CurrentCulture);

            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            File.WriteAllText(sourceFileLocation, str);

            //Process.Start("notepad.exe", sourceFileLocation);

            return sourceFileLocation;
        }

        public static MemberInfo GetFunctionValue(string function)
        {
            MemberInfo t;
            if (cache.ContainsKey(function.Trim().ToLower()))
            {
                t = cache[function.Trim().ToLower()];
                return t;
            }
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
                str = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Api\\All\\WIP\\";

            if (!Directory.Exists(str))
                throw new Exception($"Source directory does not exists in {str}");
            return str + sourceFile;

        }

        public static void BatchTest()
        {
            var items = new List<Tuple<string, string>>();
            items.Add(new Tuple<string, string>("Case", "CaseStatusType"));
            items.Add(new Tuple<string, string>("Case", "CaseType"));
            items.Add(new Tuple<string, string>("Case", "SubEntityProfileType"));
            items.Add(new Tuple<string, string>("Case", "ActiveCase"));
            items.Add(new Tuple<string, string>("Case", "CasePriorityType"));
            items.Add(new Tuple<string, string>("Case", "CaseComplianceType"));
            items.Add(new Tuple<string, string>("Case", "CaseContractingStatusType"));
            items.Add(new Tuple<string, string>("Case", "CaseOnOnboardingStatusType"));
            items.Add(new Tuple<string, string>("Case", "CaseCredentialingStatusType"));
            items.Add(new Tuple<string, string>("Case", "NpiType"));
            items.Add(new Tuple<string, string>("Case", "EntityProfileType"));

            foreach (var i in items)
            {
                MainApp(new string[] { "xyz", i.Item2, i.Item1 });
            }
        }
    }
}