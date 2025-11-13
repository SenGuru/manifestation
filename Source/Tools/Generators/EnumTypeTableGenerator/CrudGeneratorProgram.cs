/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;
using Framework.Infrastructure.Utils;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using OpenXmlPowerTools;

namespace CodeGenerator
{
    public static class CrudGeneratorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length < 2)
            {
                throw new Exception($"Atleast 2 parameters need to be provided - Current Length = {args.Length}");
            }

            var appName = args[1];
            var tableName = args[2];
            var parentTableName = "";
            if (args.Length > 3)
            {
                parentTableName = args[3];
            }

            var t = GetTableType(tableName);
            if (t == null)
                throw new Exception($"Unable to find the Type for table name - {tableName}");

            var modelDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Common\\Models\\ViewModels\\WIP\\";
            var viewDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Common\\Models\\DbView\\WIP\\";
            var contrlDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Api\\All\\Controllers\\WIP\\";
            var searchDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Common\\Models\\SearchCriteria\\WIP\\";
            var testDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Tests\\IntegrationTests\\Api\\";
            var serviceDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Service\\WIP\\";
            var repoDir = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Shared\\Repository\\WIP\\";
            var auditDir = serviceDir;
            //var permDir = serviceDir;

            Generate(appName, parentTableName, t, tableName, "WipController.cs", $"WipController.{tableName}.cs", contrlDir);
            Generate(appName, parentTableName, t, tableName, "WipRepository.cs", $"WipRepository.{tableName}.cs", repoDir);
            Generate(appName, parentTableName, t, tableName, "WipAuditService.cs", $"AuditService.{tableName}.cs", auditDir);
            //Generate(appName, parentTableName, t, tableName, "WipPermissionService.cs", $"PermissionService.{tableName}.cs", permDir);
            Generate(appName, parentTableName, t, tableName, "WipService.cs", $"WipService.{tableName}.cs", serviceDir);
            Generate(appName, parentTableName, t, tableName, "WipMapperService.cs", "MapService." + tableName + ".cs", serviceDir);
            Generate(appName, parentTableName, t, tableName, "WipModel.cs", tableName + "Model.cs", modelDir);
            Generate(appName, parentTableName, t, tableName, "WipAddModel.cs", tableName + "CreateModel.cs", modelDir);
            Generate(appName, parentTableName, t, tableName, "WipAddMultipleModel.cs", "Multiple" + tableName + "CreateModel.cs", modelDir);
            Generate(appName, parentTableName, t, tableName, "WipEditModel.cs", tableName + "UpdateModel.cs", modelDir);
            Generate(appName, parentTableName, t, tableName, "WipView.cs", tableName + "View.cs", viewDir);
            Generate(appName, parentTableName, t, tableName, "WipSearchCriteria.cs", tableName + "SearchCriteria.cs", searchDir);
            Generate(appName, parentTableName, t, tableName, "WipTest.cs", "CompanyControllerTest_" + tableName + ".cs", testDir);
        }

        static Dictionary<string, Type> cache = new();
        static CrudGeneratorProgram()
        {
            var t = typeof(Manifest.Common.Constants.ConfigurationConstants);

            var assembly = t.Assembly;
            foreach (var aClass in assembly.GetTypes())
            {
                if (!aClass.FullName.StartsWith("Manifest.Common.Models.DB"))
                    continue;

                cache.Add(aClass.Name.Trim().ToLower(), aClass);
            }
        }


        public static string Generate(string appName,string parentTableName, Type t, string tableName, string templateFile, string sourceFile, string folder)
        {
            var templateFileLocation = CodeGenUtils.GetTemplateFileLocation(templateFile);
            var sourceFileLocation = GetSourceFileLocation(sourceFile, folder);

            var capsFirstLetterString = tableName[0].ToString().ToUpper() + tableName.Substring(1);
            var lowerFirstLetterString = tableName[0].ToString().ToLower() + tableName.Substring(1);
            var beginComment = "";
            var endComment = "";
            var splittedCapsFirstLetterString = capsFirstLetterString.SplitCamelCase();
            var splittedLowerFirstLetterString = lowerFirstLetterString.SplitCamelCase();

            if (parentTableName.IsTrimmedStringNullOrEmpty())
            {
                parentTableName = "XYZZYX";
                beginComment = "/*";
                endComment = "*/";
            }

            var capsFirstLetterStringParent = parentTableName[0].ToString().ToUpper() + parentTableName.Substring(1);
            var lowerFirstLetterStringParent = parentTableName[0].ToString().ToLower() + parentTableName.Substring(1);

            var str = File.ReadAllText(templateFileLocation);
            str = str.Replace("//%RepoJoin%", GetRepoJoin(tableName, t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoAssignment%", GetRepoAssignment(t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoWhereCheck%", GetRepoWhereCheck(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ServiceChecks%", GetServiceCheck(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ServiceFKeyAssignment%", GetServiceFKeyAssignment(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ViewAndModelVariable%", GetViewAndModelVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%SearchVariable%", GetSearchVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoConditionalWhere%", GetRepoAssignmentWhere(t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoAddNameCheck%", GetRepoAddNameCheck(t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoEditNameCheck%", GetRepoEditNameCheck(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ExistsCheck%", GetRepoEditNameCheckExistsCheckVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%CreateModelVariable%", GetCreateModelVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%UpdateModelVariable%", GetUpdateModelVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ModelVariable%", GetModelVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%FKeyAssignmentInCreateUpdateCode%", GetFkeyAssignmentInCreateAndUpdateCodeVariable(t), StringComparison.CurrentCulture);
            str = str.Replace("//%TestCode%", GetTestCodeVariable(), StringComparison.CurrentCulture);

            str = str.Replace("//%WebFkeyBasedFunctions%", GetWebFkeyBasedFunctions(t), StringComparison.CurrentCulture);
            str = str.Replace("//%ServiceFkeyBasedFunctions%", GetServiceFkeyBasedFunctions(t), StringComparison.CurrentCulture);
            str = str.Replace("//%RepoFkeyBasedFunctions%", GetRepoFkeyBasedFunctions(t), StringComparison.CurrentCulture);
            str = str.Replace("//%SearchCriteriaModelFkeyBasedFunctions%", GetSearchCriteriaModelFkeyBasedFunctions(t), StringComparison.CurrentCulture);
            
            str = str.Replace("EEntity123", capsFirstLetterString, StringComparison.CurrentCulture);
            str = str.Replace("eentity123", lowerFirstLetterString, StringComparison.CurrentCulture);
            str = str.Replace("FFntity123", splittedCapsFirstLetterString, StringComparison.CurrentCulture);
            str = str.Replace("ffntity123", splittedLowerFirstLetterString, StringComparison.CurrentCulture);

            str = str.Replace("XYZZYX", capsFirstLetterStringParent, StringComparison.CurrentCulture);
            str = str.Replace("xyzzyx", lowerFirstLetterStringParent, StringComparison.CurrentCulture);

            str = str.Replace("//%ParentCommentBegin%", beginComment, StringComparison.CurrentCulture);
            str = str.Replace("//%ParentCommentEnd%", endComment, StringComparison.CurrentCulture);
            str = str.Replace("%AppName%", appName, StringComparison.CurrentCulture);           


            if (File.Exists(sourceFileLocation))
            {
                File.Delete(sourceFileLocation);
            }

            File.WriteAllText(sourceFileLocation, str);

            //Process.Start("notepad.exe", sourceFileLocation);

            return sourceFileLocation;
        }

        private static string GetServiceFKeyAssignment(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Guid" || p.Name == t.Name + "Id" || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId" || p.Name == "CreatedDate" || p.Name == "ModifiedDate")
                    continue;

                var isLong = (p.PropertyType == typeof(long) || p.PropertyType == typeof(long?));

                var isDateTime = (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                var isGuid = (p.PropertyType == typeof(Guid) || p.PropertyType == typeof(Guid?));

                if (!isLong)
                {
                    continue;
                }

                if (p.Name.EndsWith("TypeId"))
                    continue;

                if (!p.Name.EndsWith("Id"))
                    continue;

                var space = i == 0 ? "" : "            ";
                const string space2 = "            ";
                var vName = p.Name.Substring(0, p.Name.Length - 2);
                var vNameSmall = vName[0].ToString().ToLower() + vName.Substring(1);
                var resolveNullable = "";
                if (p.PropertyType.IsNullable())
                {
                    sb.AppendLine(space + $"if(eentity123Model.{vName}Id > 0)");
                    sb.AppendLine(space2 + $"{{");
                    space = space2;
                    resolveNullable = "?? 0";
                }

                var vViewTag = "View";
                var needTenantCheck = true;
                if (vName == "Country")
                {
                    needTenantCheck = false;
                    vViewTag = "";
                }

                if (vName == "State")
                {
                    needTenantCheck = false;
                }

                if (vName == "State" || vName == "Country")
                {
                    sb.AppendLine(space + $"var {vNameSmall}Item = await systemRepository.Get{vName}ByIdAsync(eentity123Model.{vName}Id{resolveNullable});");
                }
                else
                {
                    sb.AppendLine(space + $"var {vNameSmall}Item = await wipRepository.Get{vName}{vViewTag}ByIdAsync(eentity123Model.{vName}Id{resolveNullable});");
                }

                sb.AppendLine(space2 + $"if ({vNameSmall}Item == null)");
                sb.AppendLine(space2 + $"{{");
                sb.AppendLine(space2 + $"       result.Add(new ReturnErrorItem(\"\", $\"Unable to Get the {vName} Information\"));");
                sb.AppendLine(space2 + $"       return result;");
                sb.AppendLine(space2 + $"}}");
                sb.AppendLine($"");

                if (needTenantCheck)
                {
                    sb.AppendLine(space2 + $"if ({vNameSmall}Item.CompanyId != eentity123Model.CompanyId)");
                    sb.AppendLine(space2 + $"{{");
                    sb.AppendLine(space2 + $"       result.Add(new ReturnErrorItem(\"\", $\"You dont have access to this {vName} Information\"));");
                    sb.AppendLine(space2 + $"       return result;");
                    sb.AppendLine(space2 + $"}}");
                    sb.AppendLine($"");
                }

                //sb.AppendLine(space2 + $"item.{vName}Id = {vNameSmall}Item.{vName}Id;");
                if (p.PropertyType.IsNullable())
                {
                    sb.AppendLine(space + $"}}");
                }
                sb.AppendLine($"");
                ++i;
            }
            return sb.ToString();
        }


        private static string GetServiceCheck(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Guid" || p.Name == t.Name + "Id" || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId" || p.Name == "CreatedDate" || p.Name == "ModifiedDate")
                    continue;

                var isNotNull = false;
                var rlst = p.GetCustomAttributes(typeof(NotNullAttribute), true);
                if (!(rlst == null || rlst.Length == 0))
                {
                    isNotNull = true;
                }

                var isString = false;
                var sllst = p.GetCustomAttributes(typeof(StringLengthAttribute), true);
                if (!(sllst == null || sllst.Length == 0))
                {
                    isString = true;
                }

                var isLong = (p.PropertyType == typeof(long) || p.PropertyType == typeof(long?));

                var isDateTime = (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                var isGuid = (p.PropertyType == typeof(Guid) || p.PropertyType == typeof(Guid?));

                if (isLong)
                {
                    if (isNotNull)
                    {
                        var isType = p.Name.StartsWith("Predefined");

                        var space = i == 0 ? "" : "            ";
                        const string space2 = "            ";

                        if (isType)
                        {
                            sb.AppendLine(space + $"if (eentity123Model.{p.Name} == 0)");
                            sb.AppendLine(space2 + $"{{");
                            sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", \"{p.Name} cannot be empty\"));");
                            sb.AppendLine(space2 + $"}}");

                            var enumPartialname = p.Name.Replace("Id", "") + "s";
                            if (CodeGenUtils.IsEnumAvailable(enumPartialname))
                            {
                                sb.AppendLine(space2 + $"else if (!ValidationUtils.IsEnumValid<{enumPartialname},long>(eentity123Model.{p.Name}))");
                                sb.AppendLine(space2 + $"{{");
                                sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", \"{p.Name} is not a valid {enumPartialname}\"));");
                                sb.AppendLine(space2 + $"}}");
                            }
                            i++;
                        }
                        else if (p.Name.EndsWith("Id"))
                        {
                            var vName = p.Name.Substring(0, p.Name.Length - 2);

                            var vNameGuid = vName + "Id";

                            sb.AppendLine(space + $"if (!ValidationUtils.IsLongValid(eentity123Model.{vNameGuid}))");
                            sb.AppendLine(space2 + $"{{");
                            sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{vNameGuid}\", \"{vName} cannot be empty\"));");
                            sb.AppendLine(space2 + $"}}");
                            sb.AppendLine(space2 + $"else");
                            sb.AppendLine(space2 + $"{{");
                            sb.AppendLine(space + $"    var itemFor{vName} = await wipRepository.Get{vName}ByIdAsync(eentity123Model.{vNameGuid});");
                            sb.AppendLine(space2 + $"    if (itemFor{vName} == null)");
                            sb.AppendLine(space2 + $"    {{");
                            sb.AppendLine(space2 + $"        result.Add(new ReturnErrorItem(\"{vNameGuid}\", \"{vName} is not available\"));");
                            sb.AppendLine(space2 + $"    }}");
                            sb.AppendLine(space2 + $"    else");
                            sb.AppendLine(space2 + $"    {{");
                            sb.AppendLine(space2 + $"       if (itemFor{vName}.CompanyId != eentity123Model.CompanyId)");
                            sb.AppendLine(space2 + $"       {{");
                            sb.AppendLine(space2 + $"           result.Add(new ReturnErrorItem(\"{vNameGuid}\", \"{vName} does not belong to you.\"));");
                            sb.AppendLine(space2 + $"       }}");
                            sb.AppendLine(space2 + $"    }}");
                            sb.AppendLine(space2 + $"}}");
                            i++;
                        }
                        else
                        {
                            ++i;
                            --i;
                        }
                    }
                    else
                    {
                        ++i;
                        --i;
                    }
                }
                else if (isString)
                {
                    var space = i == 0 ? "" : "            ";
                    const string space2 = "            ";
                    sb.AppendLine(space + $"if (eentity123Model.{p.Name} != null && eentity123Model.{p.Name}.Length > EEntity123Model.COLUMN_SIZE_{p.Name})");
                    sb.AppendLine(space2 + $"{{");
                    sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", $\"{p.Name} size should not exceed {{EEntity123Model.COLUMN_SIZE_{p.Name}}}\"));");
                    sb.AppendLine(space2 + $"}}");

                    if (isNotNull)
                    {
                        sb.AppendLine(space2 + $"if (eentity123Model.{p.Name} == null)");
                        sb.AppendLine(space2 + $"{{");
                        sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", \"{p.Name} is required \"));");
                        sb.AppendLine(space2 + $"}}");
                    }
                    i++;
                }
                else if (isDateTime)
                {
                    var space = i == 0 ? "" : "            ";
                    const string space2 = "            ";
                    if (!isNotNull)
                    {
                        sb.AppendLine(space + $"if (eentity123Model.{p.Name} != null && !DateUtils.IsValidDate(eentity123Model.{p.Name}.Value))");
                        sb.AppendLine(space2 + $"{{");
                        sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", $\"{p.Name} is not a valid date\"));");
                        sb.AppendLine(space2 + $"}}");
                    }
                    else
                    {
                        sb.AppendLine(space + $"if (!DateUtils.IsValidDate(eentity123Model.{p.Name}))");
                        sb.AppendLine(space2 + $"{{");
                        sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", $\"{p.Name} is not a valid date\"));");
                        sb.AppendLine(space2 + $"}}");
                    }
                    i++;
                }
                else if (isGuid)
                {
                    var space = i == 0 ? "" : "            ";
                    const string space2 = "            ";
                    if (!isNotNull)
                    {
                        sb.AppendLine(space + $"if (eentity123Model.{p.Name} != null && Guid.Empty  == eentity123Model.{p.Name}.Value)");
                        sb.AppendLine(space2 + $"{{");
                        sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", $\"{p.Name} is not a validate guid\"));");
                        sb.AppendLine(space2 + $"}}");
                    }
                    else
                    {
                        sb.AppendLine(space + $"if (Guid.Empty  == eentity123Model.{p.Name})");
                        sb.AppendLine(space2 + $"{{");
                        sb.AppendLine(space2 + $"    result.Add(new ReturnErrorItem(\"{p.Name}\", $\"{p.Name} is not a validate guid\"));");
                        sb.AppendLine(space2 + $"}}");
                    }
                    i++;
                }
            }
            return sb.ToString();
        }

        private static string GetRepoWhereCheck(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Id" || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;
                var sllst = p.GetCustomAttributes(typeof(StringLengthAttribute), true);
                if (sllst == null || sllst.Length == 0)
                    continue;
                var space = i == 0 ? "" : "                                ";
                sb.AppendLine(space + $"|| x.{p.Name}.Trim().ToUpper().Contains(search.Keyword.Trim().ToUpper())");
                i++;
            }
            return sb.ToString();
        }

        private static string GetPropertyNameOfName(Type pType)
        {
            foreach (var p in pType.GetProperties())
            {
                if (p.Name == pType.Name + "Name")
                    return p.Name;
            }
            return null;
        }
        private static string GetPropertyNameOfGuid(Type pType)
        {
            foreach (var p in pType.GetProperties())
            {
                if (p.Name == pType.Name + "Name")
                    return pType.Name + "Guid";
            }
            return null;
        }

        private static string GetViewAndModelVariable(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            string space;
            string space2;

            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Id" || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;

                var fkeyType = GetFkeyType(t.Name, p);
                if (fkeyType == null)
                {
                    if (!p.Name.EndsWith("TypeId"))
                    {
                        if (p.Name.IsSameWithCaseIgnoreTrimmed("CompanyId"))
                        {
                            continue;
                        }

                        if (p.Name.EndsWith("Id"))
                        {
                            space = i == 0 ? "" : "        ";
                            space2 = "        ";

                            //sb.AppendLine("[NotColumn]");
                            //sb.AppendLine($"public Guid {p.Name.Substring(0, p.Name.Length - 2)}Guid {{ get; set;}}");
                            sb.AppendLine("\n[NotColumn]");
                            sb.AppendLine($"public string {p.Name.Substring(0, p.Name.Length - 2)}Name {{ get; set; }}");
                            i++;
                        }
                    }
                    else
                    {
                        continue;
                        //throw new Exception("TypeId is not having FKey relationship in DB");
                    }
                    continue;
                }

                var fkeyName = GetPropertyNameOfName(fkeyType);
                space = i == 0 ? "" : "        ";
                space2 = "        ";

                if (fkeyName == null)
                    continue;

                var fkeyGuid = GetPropertyNameOfGuid(fkeyType);

                sb.AppendLine(space + "[NotColumn]");
                if (fkeyName == "EnumTypeName")
                {
                    sb.AppendLine(space2 + $"public string {p.Name.Replace("Id","Name")} {{ get; set; }}");
                }
                else
                {
                    sb.AppendLine(space2 + $"public string {fkeyName} {{ get; set; }}");
                }

                //sb.AppendLine(space2 + "[NotColumn]");
                //sb.AppendLine(space2 + $"public Guid {fkeyGuid} {{ get; set; }}");
                i++;
            }
            return sb.ToString();
        }

        private static string GetSearchVariable(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Id")
                {
                    continue;
                }

                if (p.Name.IsSameWithCaseIgnoreTrimmed("CompanyId"))
                {
                    continue;
                }

                if (p.Name.EndsWith("TypeId") || p.Name.EndsWith("Predefined"))
                {
                    var fKeyType = GetFkeyType(t.Name, p);
                    if (fKeyType == null)
                        continue;

                    var space = i == 0 ? "" : "        ";
                    sb.AppendLine();
                    sb.AppendLine(space + $"public List<long> {p.Name}s {{ get; set; }}");
                }
                else if (p.Name.EndsWith("Id"))
                {
                    if (!(p.PropertyType == typeof(long) || p.PropertyType == typeof(long?)))
                        continue;
                    var space = i == 0 ? "" : "        ";
                    //var pName = p.Name.Substring(0, p.Name.Length - 2) + "Id";
                    sb.AppendLine();
                    sb.AppendLine(space + $"public long? {p.Name} {{ get; set; }}");
                }
                else
                {
                    continue;
                }
                ++i;
            }
            return sb.ToString();
        }

        private static string GetRepoAssignment(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                var joinSyb = ((char)('a' + i)).ToString();

                var fKeyType = GetFkeyType(t.Name, p);
                if (fKeyType == null)
                    continue;
                var fkeyName = GetPropertyNameOfName(fKeyType);
                if (fkeyName == null)
                    continue;

                var space = i == 0 ? "" : "                       ";
                if (fkeyName == "EnumTypeName")
                {
                    sb.AppendLine(space + $"{p.Name.Replace("Id", "Name")} = {joinSyb}.{p.Name.Replace("Id","Name")},");
                }
                else
                {
                    sb.AppendLine(space + $"{fkeyName} = {joinSyb}.{fkeyName},");
                }
                //var space2 = "                       ";

                //var fkeyGuid = GetPropertyNameOfGuid(fKeyType);
                //if (fkeyGuid != null)
                //{
                //    //sb.AppendLine(space2 + $"{fkeyGuid} = {joinSyb}.{fkeyGuid},");
                //}
                ++i;
            }

            sb.AppendLine($"");
            foreach (var p in t.GetProperties())
            {
                var space = i == 0 ? "" : "                       ";
                sb.AppendLine(space + $"{p.Name} = item.{p.Name},");
            }
            return sb.ToString();
        }

        private static string GetRepoAssignmentWhere(Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (p.Name == t.Name + "Id")
                {
                    continue;
                }

                if (p.Name.EndsWith("TypeId"))
                {

                    var fKeyType = GetFkeyType(t.Name, p);
                    if (fKeyType == null)
                        continue;

                    string isNullableStr = "";

                    if (p.IsNullableValueMember())
                        isNullableStr = "?? 0";

                    var space = i == 0 ? "" : "                ";
                    const string space2 = "                ";
                    sb.AppendLine(space + $"if (search.{p.Name}s != null && search.{p.Name}s.Count > 0)");
                    sb.AppendLine(space2 + $"    sql1 = sql1.Where(x => search.{p.Name}s.Contains(x.{p.Name}{isNullableStr}));");
                    ++i;
                }
                else if (p.Name.EndsWith("Id"))
                {
                    if (!(p.PropertyType == typeof(long) || p.PropertyType == typeof(long?)))
                        continue;

                    var space = i == 0 ? "" : "                ";
                    const string space2 = "                ";
                    sb.AppendLine(space + $"if (search.{p.Name} != null && search.{p.Name} > 0)");
                    sb.AppendLine(space2 + $"    sql1 = sql1.Where(x => search.{p.Name} == x.{p.Name});");
                    ++i;
                }
            }
            return sb.ToString();
        }


        private static string GetRepoJoin(string tableName, Type t)
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                var joinSyb = ((char)('a' + i)).ToString();
                var fKeyType = GetFkeyType(tableName, p);
                if (fKeyType == null)
                    continue;
                var space = i == 0 ? "" : "                   ";
                const string space2 = "                   ";
                if (p.PropertyType.IsNullable())
                {
                    sb.AppendLine(space + $"join o{joinSyb} in appDBManager.GetTable<{fKeyType.Name}>() on item.{p.Name} equals o{joinSyb}.{fKeyType.Name}Id into {joinSyb}Temp");
                    sb.AppendLine(space2 + $"from {joinSyb} in {joinSyb}Temp.DefaultIfEmpty()");
                }
                else
                {
                    sb.AppendLine(space + $"join {joinSyb} in appDBManager.GetTable<{fKeyType.Name}>() on item.{p.Name} equals {joinSyb}.{fKeyType.Name}Id");
                }

                i++;
            }
            return sb.ToString();
        }

        public static string GetRepoEditNameCheckExistsCheckVariable(Type t)
        {
            var p = GetPropertyNameOfName(t);
            if (p == null)
                return "\"\"";

            return $"eentity123Model.EEntity123Name";
        }

        public static string GetFkeyAssignmentInCreateAndUpdateCodeVariable(Type t)
        {
            return "";
        }

        public static string GetViewVariable(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                if (p.Name.EndsWith("TypeId"))
                    continue;

                var space = i == 0 ? "" : "        ";
                var varName = p.Name.Substring(0, p.Name.Length - 2) + "Guid";
                sb.AppendLine($"{space}public Guid {varName} {{ get; set; }}");
                i++;
            }
            return sb.ToString();
        }



        public static string GetModelVariable(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                if (p.Name.EndsWith("TypeId"))
                    continue;

                var space = i == 0 ? "" : "        ";
                sb.AppendLine($"{space}private new string {p.Name} {{ get; set; }}");
                i++;
            }
            return sb.ToString();
        }

        public static string GetUpdateModelVariable(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                var space = i == 0 ? "" : "        ";
                var varName = p.Name.Substring(0, p.Name.Length - 2) + "Name";
                sb.AppendLine($"{space}private new string {varName} {{ get; set; }}");
                i++;
            }
            return sb.ToString();
        }

        public static string GetCreateModelVariable(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 0;
            foreach (var p in t.GetProperties())
            {
                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                var space = i == 0 ? "" : "        ";
                var varName = p.Name.Substring(0, p.Name.Length - 2) + "Name";
                sb.AppendLine($"{space}private new string {varName} {{ get; set; }}");
                i++;
            }
            return sb.ToString();
        }

        public static string GetTestCodeVariable()
        {
            return "Test Code";
        }

        public static string GetWebFkeyBasedFunctions(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 2;

            foreach (var p in t.GetProperties())
            {
                var pName = p.Name;
                var pNameLowerFirst = p.Name.MakeFirstCharAsLower();

                var pNameWithoutId = p.Name.Replace("Id", "");
                var tName = t.Name;
                var tNameWithId = t.Name + "Id";
                var tNameLowerFirst = t.Name.MakeFirstCharAsLower();

                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;
                
                if (p.Name.EndsWith("TypeId") || p.Name.StartsWith("Predefined") || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;

                var space = i == 0 ? "" : "        ";
                sb.AppendLine($"");
                sb.AppendLine($"[HttpGet]");
                sb.AppendLine($"[SwaggerOperation(Tags = new[] {{\"ToTest\"}})]");
                sb.AppendLine($"{space}public Task<ReturnListModel<{t.Name}Model>> Get{t.Name}By{p.Name}([FromQuery] long {pNameLowerFirst})");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return wipService.Get{t.Name}By{p.Name}Async({pNameLowerFirst});");
                sb.AppendLine($"{space}}}");

                sb.AppendLine($"");
                sb.AppendLine($"[HttpGet]");
                sb.AppendLine($"[SwaggerOperation(Tags = new[] {{\"ToTest\"}})]");
                sb.AppendLine($"{space}public Task<ReturnListModel<{t.Name}Model>> Get{t.Name}By{p.Name}Search({t.Name}By{p.Name}SearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return wipService.Get{t.Name}By{p.Name}Async(search);");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                if (!(t.Name == "Country" || p.Name == "CountryId" || t.Name == "State" || p.Name == "StateId"))
                {
                    sb.AppendLine($"[HttpGet]");
                    sb.AppendLine($"[SwaggerOperation(Tags = new[] {{\"ToTest\"}})]");
                    sb.AppendLine($"{space}public Task<ReturnListModel<{pNameWithoutId}Model>> Get{pNameWithoutId}By{t.Name}Id(long {tNameLowerFirst}Id)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}return wipService.Get{pNameWithoutId}By{t.Name}IdAsync({tNameLowerFirst}Id);");
                    sb.AppendLine($"{space}}}");

                    sb.AppendLine($"");
                    sb.AppendLine($"[HttpGet]");
                    sb.AppendLine($"[SwaggerOperation(Tags = new[] {{\"ToTest\"}})]");
                    sb.AppendLine($"{space}public Task<ReturnListModel<{pNameWithoutId}Model>> Get{pNameWithoutId}By{t.Name}IdSearch({pNameWithoutId}By{t.Name}IdSearchCriteria search)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}return wipService.Get{pNameWithoutId}By{t.Name}IdAsync(search);");
                    sb.AppendLine($"{space}}}");
                    sb.AppendLine($"");
                }

                i++;
            }
            return sb.ToString();
        }

        public static string GetServiceFkeyBasedFunctions(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 2;

            foreach (var p in t.GetProperties())
            {
                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                if (p.Name.EndsWith("TypeId") || p.Name.StartsWith("Predefined") || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;

                var pName = p.Name;
                var pNameLowerFirst = p.Name.MakeFirstCharAsLower();
                
                var pNameWithoutId = p.Name.Replace("Id","");
                var tName = t.Name;
                var tNameWithId = t.Name + "Id";
                var tNameWithoutId = t.Name;

                var tNameLowerFirst = t.Name.MakeFirstCharAsLower();

                var space = i == 0 ? "" : "        ";

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{t.Name}Model>> Get{t.Name}By{p.Name}Async(long {pNameLowerFirst})");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (pNameWithoutId == "Country" || pNameWithoutId == "State")
                {
                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{pNameWithoutId}ByIdAsync({pNameLowerFirst});");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                    sb.AppendLine($"{space}{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}Model>($\"Unable to find the {pNameWithoutId}\", new {{ {pNameLowerFirst} }});");
                    sb.AppendLine($"{space}{space}{space}}}");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{pNameWithoutId}ByIdForReadAsync({pNameLowerFirst});");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                    sb.AppendLine($"{space}{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}Model>($\"Unable to find the {pNameWithoutId}\", new {{ {pNameLowerFirst} }});");
                    sb.AppendLine($"{space}{space}{space}}}");
                }
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.Get{t.Name}ViewBy{p.Name}Async({pNameLowerFirst});");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{t.Name}Model>.Success(objectMapperService.{t.Name}ViewListTo{t.Name}ModelList(items));");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}Model>($\"Unexpected error when getting FFntity123 by {p.Name}\", new {{ {pNameLowerFirst} }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{t.Name}Model>> Get{t.Name}By{p.Name}Async({t.Name}By{p.Name}SearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (pNameWithoutId == "State" || pNameWithoutId == "Country")
                {
                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{pNameWithoutId}ByIdAsync(search.{p.Name});");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{pNameWithoutId}ByIdForReadAsync(search.{p.Name});");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                }
                sb.AppendLine($"{space}{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}Model>($\"Unable to find the {pNameWithoutId}\", new {{ search.{p.Name} }});");
                sb.AppendLine($"{space}{space}{space}}}");
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.Get{t.Name}ViewBy{p.Name}Async(search);");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{t.Name}Model>.Success(objectMapperService.{t.Name}ViewListTo{t.Name}ModelList(items.Result));");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}Model>($\"Unexpected error when getting FFntity123 by {p.Name}\", new {{ search.{p.Name} }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{t.Name}>> GetBare{t.Name}By{p.Name}Async(long {pNameLowerFirst})");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (pNameWithoutId == "State" || pNameWithoutId == "Country")
                {

                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{pNameWithoutId}ByIdAsync({pNameLowerFirst});");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{pNameWithoutId}ByIdForReadAsync({pNameLowerFirst});");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                }
                sb.AppendLine($"{space}{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}>($\"Unable to find the {pNameWithoutId}\", new {{ {pNameLowerFirst} }});");
                sb.AppendLine($"{space}{space}{space}}}");
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.GetBare{t.Name}By{p.Name}Async({pNameLowerFirst});");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{t.Name}>.Success(items);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}>($\"Unexpected error when getting FFntity123 by {p.Name}\", new {{ {pNameLowerFirst} }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{t.Name}>> GetBare{t.Name}By{p.Name}Async({t.Name}By{p.Name}SearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (pNameWithoutId == "State" || pNameWithoutId == "Country")
                {

                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{pNameWithoutId}ByIdAsync(search.{p.Name});");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{pNameWithoutId}ByIdForReadAsync(search.{p.Name});");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                }
                sb.AppendLine($"{space}{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}>($\"Unable to find the {pNameWithoutId}\", new {{ search.{p.Name} }});");
                sb.AppendLine($"{space}{space}{space}}}");
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.GetBare{t.Name}By{p.Name}Async(search);");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{t.Name}>.Success(items);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{t.Name}>($\"Unexpected error when getting FFntity123 by {p.Name}\", new {{ search.{p.Name} }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                if (!(t.Name == "Country" || p.Name == "CountryId" || t.Name == "State" || p.Name == "StateId"))
                {
                    sb.AppendLine($"");
                    sb.AppendLine($"{space}public async Task<ReturnListModel<{pNameWithoutId}Model>> Get{pNameWithoutId}By{t.Name}IdAsync(long {tNameLowerFirst}Id)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}try");
                    sb.AppendLine($"{space}{space}{{");
                    if (tNameWithoutId == "State" || tNameWithoutId == "Country")
                    {
                        sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{tNameWithoutId}ByIdAsync({tNameLowerFirst}Id);");
                        sb.AppendLine($"{space}{space}{space}if (view == null)");
                    }
                    else
                    {
                        sb.AppendLine($"{space}{space}{space}var view = await Get{tNameWithoutId}ByIdForReadAsync({tNameLowerFirst}Id);");
                        sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");

                    }
                    sb.AppendLine($"{space}{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}Model>($\"Unable to find the {tNameWithoutId}\", new {{ {tNameLowerFirst}Id }});");
                    sb.AppendLine($"{space}{space}{space}}}");
                    sb.AppendLine($"{space}{space}{space}");
                    sb.AppendLine($"{space}{space}{space}var items = await wipRepository.Get{pNameWithoutId}ViewBy{t.Name}IdAsync({tNameLowerFirst}Id);");
                    sb.AppendLine($"{space}{space}{space}return ReturnListModel<{pNameWithoutId}Model>.Success(objectMapperService.{pNameWithoutId}ViewListTo{pNameWithoutId}ModelList(items));");
                    sb.AppendLine($"{space}{space}}}");
                    sb.AppendLine($"{space}{space}catch(Exception ex)");
                    sb.AppendLine($"{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}Model>($\"Unexpected error when getting FFntity123 by {t.Name}\", new {{ {tNameLowerFirst}Id }}, ex);");
                    sb.AppendLine($"{space}{space}}}");
                    sb.AppendLine($"{space}}}");
                    sb.AppendLine($"");

                    sb.AppendLine($"");
                    sb.AppendLine($"{space}public async Task<ReturnListModel<{pNameWithoutId}Model>> Get{pNameWithoutId}By{t.Name}IdAsync({pNameWithoutId}By{t.Name}IdSearchCriteria search)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}try");
                    sb.AppendLine($"{space}{space}{{");
                    if (tNameWithoutId == "State" || tNameWithoutId == "Country")
                    {
                        sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{tNameWithoutId}ByIdAsync(search.{t.Name}Id);");
                        sb.AppendLine($"{space}{space}{space}if (view == null)");
                    }
                    else
                    {
                        sb.AppendLine($"{space}{space}{space}var view = await Get{tNameWithoutId}ByIdForReadAsync(search.{t.Name}Id);");
                        sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                    }
                    sb.AppendLine($"{space}{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}Model>($\"Unable to find the {tNameWithoutId}\", new {{ search.{t.Name}Id }});");
                    sb.AppendLine($"{space}{space}{space}}}");
                    sb.AppendLine($"{space}{space}{space}");
                    sb.AppendLine($"{space}{space}{space}var items = await wipRepository.Get{pNameWithoutId}ViewBy{t.Name}IdAsync(search);");
                    sb.AppendLine($"{space}{space}{space}return ReturnListModel<{pNameWithoutId}Model>.Success(objectMapperService.{pNameWithoutId}ViewListTo{pNameWithoutId}ModelList(items.Result));");
                    sb.AppendLine($"{space}{space}}}");
                    sb.AppendLine($"{space}{space}catch(Exception ex)");
                    sb.AppendLine($"{space}{space}{{");
                    sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}Model>($\"Unexpected error when getting FFntity123 by {t.Name}\", new {{ search.{t.Name}Id }}, ex);");
                    sb.AppendLine($"{space}{space}}}");
                    sb.AppendLine($"{space}}}");
                    sb.AppendLine($"");
                }

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{pNameWithoutId}>> GetBare{pNameWithoutId}By{t.Name}IdAsync(long {tNameLowerFirst}Id)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (tNameWithoutId == "State" || tNameWithoutId == "Country")
                {
                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{tNameWithoutId}ByIdAsync({tNameLowerFirst}Id);");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{tNameWithoutId}ByIdForReadAsync({tNameLowerFirst}Id);");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                }
                sb.AppendLine($"{space}{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}>($\"Unable to find the {tNameWithoutId}\", new {{ {tNameLowerFirst}Id }});");
                sb.AppendLine($"{space}{space}{space}}}");
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.GetBare{pNameWithoutId}By{t.Name}IdAsync({tNameLowerFirst}Id);");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{pNameWithoutId}>.Success(items);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}>($\"Unexpected error when getting FFntity123 by {t.Name}\", new {{ {tNameLowerFirst}Id }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public async Task<ReturnListModel<{pNameWithoutId}>> GetBare{pNameWithoutId}By{t.Name}IdAsync({pNameWithoutId}By{t.Name}IdSearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}try");
                sb.AppendLine($"{space}{space}{{");
                if (tNameWithoutId == "State" || tNameWithoutId == "Country")
                {
                    sb.AppendLine($"{space}{space}{space}var view = await systemRepository.Get{tNameWithoutId}ByIdAsync(search.{t.Name}Id);");
                    sb.AppendLine($"{space}{space}{space}if (view == null)");
                }
                else
                {
                    sb.AppendLine($"{space}{space}{space}var view = await Get{tNameWithoutId}ByIdForReadAsync(search.{t.Name}Id);");
                    sb.AppendLine($"{space}{space}{space}if (!view.IsSuccess)");
                }
                sb.AppendLine($"{space}{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}>($\"Unable to find the {tNameWithoutId}\", new {{ search.{t.Name}Id }});");
                sb.AppendLine($"{space}{space}{space}}}");
                sb.AppendLine($"{space}{space}{space}");
                sb.AppendLine($"{space}{space}{space}var items = await wipRepository.GetBare{pNameWithoutId}By{t.Name}IdAsync(search);");
                sb.AppendLine($"{space}{space}{space}return ReturnListModel<{pNameWithoutId}>.Success(items);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}{space}catch(Exception ex)");
                sb.AppendLine($"{space}{space}{{");
                sb.AppendLine($"{space}{space}{space}return log.ErrorAndReturnListModel<{pNameWithoutId}>($\"Unexpected error when getting FFntity123 by {t.Name}\", new {{ search.{t.Name}Id }}, ex);");
                sb.AppendLine($"{space}{space}}}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                i++;
            }
            return sb.ToString();
        }

        public static string GetRepoFkeyBasedFunctions(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 2;

            foreach (var p in t.GetProperties())
            {
                var pName = p.Name;
                var pNameLowerFirst = p.Name.MakeFirstCharAsLower();

                var pNameWithoutId = p.Name.Replace("Id", "");
                var tName = t.Name;
                var tNameWithId = t.Name + "Id";
                var tNameLowerFirst = t.Name.MakeFirstCharAsLower();

                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                if (p.Name.EndsWith("TypeId") || p.Name.StartsWith("Predefined") || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;

                var space = i == 0 ? "" : "        ";

                sb.AppendLine($"");
                sb.AppendLine($"{space}public Task<List<{t.Name}View>> Get{t.Name}ViewBy{p.Name}Async(long {pNameLowerFirst})");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return Get{t.Name}sViewAsIQueryable().Where(x => x.IsDeleted == false && x.{p.Name} == {pNameLowerFirst}).ToListAsync();");
                sb.AppendLine($"{space}}}");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public Task<DbReturnListModel<{t.Name}View>> Get{t.Name}ViewBy{p.Name}Async({t.Name}By{p.Name}SearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return Get{t.Name}sViewAsIQueryable().Where(x => x.IsDeleted == false && x.{p.Name} == search.{p.Name}).ReturnListModelResultAsync(search);");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                sb.AppendLine($"{space}public Task<List<{t.Name}>> GetBare{t.Name}By{p.Name}Async(long {pNameLowerFirst})");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{p.Name} == {pNameLowerFirst}).ToListAsync();");
                sb.AppendLine($"{space}}}");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public Task<DbReturnListModel<{t.Name}>> GetBare{t.Name}By{p.Name}Async({t.Name}By{p.Name}SearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}return appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{p.Name} == search.{p.Name}).ReturnListModelResultAsync(search);");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                if (!(t.Name == "Country" || p.Name == "CountryId" || t.Name == "State" || p.Name == "StateId"))
                {
                    sb.AppendLine($"");
                    sb.AppendLine($"{space}public Task<List<{pNameWithoutId}View>> Get{pNameWithoutId}ViewBy{t.Name}IdAsync(long {tNameLowerFirst}Id)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}var idsSql = appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{t.Name}Id == {tNameLowerFirst}Id).Select(x => x.{pNameWithoutId}Id);");
                    sb.AppendLine($"{space}{space}return Get{pNameWithoutId}sViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.{pNameWithoutId}Id)).ToListAsync();");
                    sb.AppendLine($"{space}}}");

                    sb.AppendLine($"");
                    sb.AppendLine($"{space}public Task<DbReturnListModel<{pNameWithoutId}View>> Get{pNameWithoutId}ViewBy{t.Name}IdAsync({pNameWithoutId}By{t.Name}IdSearchCriteria search)");
                    sb.AppendLine($"{space}{{");
                    sb.AppendLine($"{space}{space}var idsSql = appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{t.Name}Id == search.{tName}Id).Select(x => x.{pNameWithoutId}Id);");
                    sb.AppendLine($"{space}{space}return Get{pNameWithoutId}sViewAsIQueryable().Where(x => x.IsDeleted == false && idsSql.Contains(x.{pNameWithoutId}Id)).ReturnListModelResultAsync(search);");

                    sb.AppendLine($"{space}}}");
                    sb.AppendLine($"");
                }

                sb.AppendLine($"{space}public Task<List<{pNameWithoutId}>> GetBare{pNameWithoutId}By{t.Name}IdAsync(long {tNameLowerFirst}Id)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}var idsSql = appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{t.Name}Id == {tNameLowerFirst}Id).Select(x => x.{pNameWithoutId}Id);");
                sb.AppendLine($"{space}{space}return appDBManager.GetTable<{pNameWithoutId}>().Where(x => x.IsDeleted == false && idsSql.Contains(x.{pNameWithoutId}Id)).ToListAsync();");
                sb.AppendLine($"{space}}}");

                sb.AppendLine($"");
                sb.AppendLine($"{space}public Task<DbReturnListModel<{pNameWithoutId}>> GetBare{pNameWithoutId}By{t.Name}IdAsync({pNameWithoutId}By{t.Name}IdSearchCriteria search)");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}var idsSql = appDBManager.GetTable<{t.Name}>().Where(x => x.IsDeleted == false && x.{t.Name}Id == search.{tName}Id).Select(x => x.{pNameWithoutId}Id);");
                sb.AppendLine($"{space}{space}return appDBManager.GetTable<{pNameWithoutId}>().Where(x => x.IsDeleted == false && idsSql.Contains(x.{pNameWithoutId}Id)).ReturnListModelResultAsync(search);");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                i++;
            }

            return sb.ToString();
        }

        public static string GetSearchCriteriaModelFkeyBasedFunctions(Type t)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            var i = 2;

            foreach (var p in t.GetProperties())
            {
                var pName = p.Name;
                var pNameLowerFirst = p.Name.MakeFirstCharAsLower();

                var pNameWithoutId = p.Name.Replace("Id", "");
                var tName = t.Name;
                var tNameWithId = t.Name + "Id";
                var tNameLowerFirst = t.Name.MakeFirstCharAsLower();

                if (!p.Name.EndsWith("Id") || t.Name + "Id" == p.Name)
                    continue;

                if (p.Name.EndsWith("TypeId") || p.Name.StartsWith("Predefined") || p.Name == "CreatedByUserId" || p.Name == "ModifiedByUserId")
                    continue;

                var space = i == 0 ? "" : "        ";

                sb.AppendLine($"");
                sb.AppendLine($"{space}public class {t.Name}By{p.Name}SearchCriteria : BaseSearchCriteria");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}public long {p.Name} {{ get; set; }}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");


                sb.AppendLine($"");
                sb.AppendLine($"{space}public class {p.Name.Replace("Id","")}By{t.Name}IdSearchCriteria : BaseSearchCriteria");
                sb.AppendLine($"{space}{{");
                sb.AppendLine($"{space}{space}public long {t.Name}Id {{ get; set; }}");
                sb.AppendLine($"{space}}}");
                sb.AppendLine($"");

                i++;
            }

            return sb.ToString();
        }

        public static string GetRepoEditNameCheck(Type t)
        {
            var p = GetPropertyNameOfName(t);
            if (p == null)
                return "return Task.FromResult(false);";

            return $"return dbMgr.ExistsAsync<EEntity123>(x => x.EEntity123Id != eentity123Id && x.EEntity123Name.ToLower().Trim() == eentity123Name.ToLower().Trim() && x.CompanyId == companyId);";
        }

        public static string GetRepoAddNameCheck(Type t)
        {
            var p = GetPropertyNameOfName(t);
            if (p == null)
                return "return Task.FromResult(false);";

            return "return dbMgr.ExistsAsync<EEntity123>(x => x.EEntity123Name.ToLower().Trim() == eentity123Name.ToLower().Trim() && x.CompanyId == companyId);";
        }

        private static Type GetFkeyType(string tableName, PropertyInfo pi, bool forValidation = false)
        {

            if (tableName + "Id" == pi.Name)
                return null;

            if (!pi.Name.EndsWith("Id"))
                return null;

            if (pi.Name.StartsWith("Predefined"))
            {
                if (!forValidation)
                {
                    var fEnumKeyType = GetTableType("EnumType");
                    return fEnumKeyType;
                }
            }

            var fkeyTableName = pi.Name.Substring(0, pi.Name.Length - 2);

            var fKeyType = GetTableType(fkeyTableName);

            return fKeyType;
        }

        public static Type GetTableType(string tableName)
        {
            if (cache.TryGetValue(tableName.Trim().ToLower(), out Type t))
                return t;
            return null;
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
    }
}
