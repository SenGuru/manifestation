/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
namespace CodeGenerator
{
    using CodeGenerator.Models;
    using Framework.Infrastructure.Utils;
    using LinqToDB.Mapping;
    using Manifest.Api.All.Controllers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;

    public static class ApiModelGeneratorProgram
    {
        public static void MainApp(bool isTest = false)
        {
            var isTS = false;
            var sb = new StringBuilder();
            var ssb = new StringBuilder();

            var t = typeof(AccountController);

            var assembly = t.Assembly;
            if (assembly == null)
            {
                Console.WriteLine("Unabel to find the Manifest.Common assembly");
                return;
            }

            var combined = CodeGenUtils.GetParsedStructure(assembly);

            //Process all the Models to make sure they are not missed when generating the TS Models
            CodeGenUtils.AddDependentModels(ref combined);


            //sb.AppendLine(System.Text.Json.JsonSerializer.Serialize<CombinedClass>(combined, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            //CodeGenUtils.CleanAndWriteAndOpenFile(Directory.GetCurrentDirectory() + "\\TS_Code_Structure.json", sb);

            var testSB = new StringBuilder();
            sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            if (isTest) { sb.AppendLine("using System.Text.Json.Serialization;"); } else sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine("using Framework.Infrastructure.Attributes;");
            sb.AppendLine("using Framework.Infrastructure.Models.FileInput;");

            if (isTest) { sb.AppendLine("namespace Manifest.Tests.Common"); } else sb.AppendLine($"namespace Manifest.WordAddIn.Shared.Models");
            sb.AppendLine("{");

            foreach (var cInfo in combined.TypescriptModelClassInfoList)
            {
                if (cInfo.CanIgnore)
                    continue;
                if (cInfo.Name.StartsWith("List`1"))
                    continue;

                if (cInfo.Name == "TenantClauseCreateModel")
                {
                    if (cInfo.Name + "1" == "1")
                    {
                        continue;
                    }
                }

                if (cInfo.Name == "FileStreamModel")
                {
                    continue;
                }

                sb.AppendLine($"    public class {cInfo.Name}");
                sb.AppendLine($"    {{");
                sb.AppendLine("");
                sb.AppendLine($"        public {cInfo.Name}()");
                sb.AppendLine($"        {{");
                sb.AppendLine($"        }}");

                var reParaList = new List<KeyValuePair<string, string>>();
                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        continue;
                    }

                    rlst = prop.GetCustomAttributes(typeof(RequiredAttribute), true);
                    if (rlst == null || rlst.Length == 0)
                    {
                        rlst = prop.GetCustomAttributes(typeof(NotNullAttribute), true);
                        if (rlst == null || rlst.Length == 0)
                        {
                            continue;
                        }
                    }
                    var tName = CodeGenUtils.GetModelProperty(isTS, prop.PropertyType);
                    var pName = prop.Name;
                    reParaList.Add(new KeyValuePair<string, string>(tName, pName));
                }

                if (reParaList.Count > 0)
                {
                    var paramValue = string.Join(",", reParaList.Select(x => x.Key + " " + x.Value.MakeFirstCharAsLower()));

                    sb.AppendLine("");
                    sb.AppendLine($"        public {cInfo.Name}({paramValue})");
                    sb.AppendLine($"        {{");
                    foreach (var p in reParaList)
                    {
                        sb.AppendLine($"            {p.Value} = {p.Value.MakeFirstCharAsLower()};");
                    }
                    sb.AppendLine($"        }}");
                    sb.AppendLine("");
                }

                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    var requiredStr = "";
                    var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        continue;
                    }

                    rlst = prop.GetCustomAttributes(typeof(RequiredAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        requiredStr = "[Required]";
                    }
                    if (isTest) { sb.AppendLine($"        {requiredStr}[JsonPropertyName(\"{CodeGenUtils.LowerFirstCharacter(prop.Name)}\")]"); } else sb.AppendLine($"        {requiredStr}[JsonProperty(\"{CodeGenUtils.LowerFirstCharacter(prop.Name)}\")]");
                    sb.AppendLine($"        public {CodeGenUtils.GetModelProperty(isTS, prop.PropertyType)} {prop.Name} {{set; get;}}");
                }

                if (!(cInfo.Name.EndsWith("CreateModel") || cInfo.Name.EndsWith("UpdateModel")))
                {
                    var cInfoProperties = cInfo.ModelType.GetProperties();

                    var cName = " ";
                    if (cInfo.Name.ToLower().EndsWith("model"))
                    {
                        cName = cInfo.Name.Substring(0, cInfo.Name.Length - "Model".Length);
                    }
                    var cModel = combined.TypescriptModelClassInfoList.FirstOrDefault(x => x.Name == cName + "CreateModel");
                    var uModel = combined.TypescriptModelClassInfoList.FirstOrDefault(x => x.Name == cName + "UpdateModel");
                    //var cModelAlternative = false;
                    //var uModelAlternative = false;

                    if (cModel == null)
                    {
                        cModel = combined.TypescriptModelClassInfoList.FirstOrDefault(x => x.Name == cName + "CreateWithFileFormModel");
                        //cModelAlternative = true;
                    }

                    if (uModel == null)
                    {
                        uModel = combined.TypescriptModelClassInfoList.FirstOrDefault(x => x.Name == cName + "UpdateWithFileFormModel");
                        //uModelAlternative = true;
                    }

                    if (cModel != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"        public {cModel.Name} GetAsCreateModel()");
                        sb.AppendLine($"        {{");
                        sb.AppendLine($"            var obj = new {cModel.Name}();");
                        foreach (var prop in cModel.ModelType.GetProperties())
                        {
                            var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                            if (!(rlst == null || rlst.Length == 0))
                            {
                                continue;
                            }

                            var cItem = cInfoProperties.FirstOrDefault(x => x.Name == prop.Name && x.PropertyType.Name == prop.PropertyType.Name);
                            if (cItem == null)
                                continue;

                            if (CodeGenUtils.GetActualPropertyTypeName(cItem) != CodeGenUtils.GetActualPropertyTypeName(prop))
                            {
                                sb.AppendLine($"            //Skipping Coz Internal Type is different obj.{CodeGenUtils.GetActualPropertyTypeName(cItem)} prop.{CodeGenUtils.GetActualPropertyTypeName(prop)} ,  -            result = result && (obj.{prop.Name} == {prop.Name});");
                                continue;
                            }
                            sb.AppendLine($"            obj.{prop.Name} = {prop.Name};");
                        }
                        sb.AppendLine($"            return obj;");
                        sb.AppendLine($"        }}");
                        sb.AppendLine();
                        sb.AppendLine($"        public bool IsSameAsCreateModel({cModel.Name} obj)");
                        sb.AppendLine($"        {{");
                        sb.AppendLine($"            var result = true;");
                        foreach (var prop in cModel.ModelType.GetProperties())
                        {
                            var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                            if (!(rlst == null || rlst.Length == 0))
                            {
                                continue;
                            }

                            var cItem = cInfoProperties.FirstOrDefault(x => x.Name == prop.Name && x.PropertyType.Name == prop.PropertyType.Name);
                            if (cItem == null)
                                continue;

                            if (CodeGenUtils.GetActualPropertyTypeName(cItem) !=  CodeGenUtils.GetActualPropertyTypeName(prop))
                            {
                                sb.AppendLine($"            //Skipping Coz Internal Type is different obj.{CodeGenUtils.GetActualPropertyTypeName(cItem)} prop.{CodeGenUtils.GetActualPropertyTypeName(prop)} ,  -            result = result && (obj.{prop.Name} == {prop.Name});");
                                continue;
                            }
                            sb.AppendLine($"            result = result && (obj.{prop.Name} == {prop.Name});");
                        }
                        sb.AppendLine($"            return result;");
                        sb.AppendLine($"        }}");
                        sb.AppendLine();
                    }

                    if (uModel != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"        public {uModel.Name} GetAsUpdateModel()");
                        sb.AppendLine($"        {{");
                        sb.AppendLine($"            var obj = new {uModel.Name}();");
                        foreach (var prop in uModel.ModelType.GetProperties())
                        {
                            var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                            if (!(rlst == null || rlst.Length == 0))
                            {
                                continue;
                            }

                            var cItem = cInfoProperties.FirstOrDefault(x => x.Name == prop.Name && x.PropertyType.Name == prop.PropertyType.Name);
                            if (cItem == null)
                                continue;

                            if (CodeGenUtils.GetActualPropertyTypeName(cItem) != CodeGenUtils.GetActualPropertyTypeName(prop))
                            {
                                sb.AppendLine($"            //Skipping Coz Internal Type is different obj.{CodeGenUtils.GetActualPropertyTypeName(cItem)} prop.{CodeGenUtils.GetActualPropertyTypeName(prop)} ,  -            result = result && (obj.{prop.Name} == {prop.Name});");
                                continue;
                            }

                            sb.AppendLine($"            obj.{prop.Name} = {prop.Name};");
                        }
                        sb.AppendLine($"            return obj;");
                        sb.AppendLine($"        }}");
                        sb.AppendLine();
                        sb.AppendLine($"        public bool IsSameAsUpdateModel({uModel.Name} obj)");
                        sb.AppendLine($"        {{");
                        sb.AppendLine($"            var result = true;");
                        foreach (var prop in uModel.ModelType.GetProperties())
                        {
                            var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                            if (!(rlst == null || rlst.Length == 0))
                            {
                                continue;
                            }

                            var cItem = cInfoProperties.FirstOrDefault(x => x.Name == prop.Name && x.PropertyType.Name == prop.PropertyType.Name);
                            if (cItem == null)
                                continue;

                            if (CodeGenUtils.GetActualPropertyTypeName(cItem) != CodeGenUtils.GetActualPropertyTypeName(prop))
                            {
                                sb.AppendLine($"            //Skipping Coz Internal Type is different obj.{CodeGenUtils.GetActualPropertyTypeName(cItem)} prop.{CodeGenUtils.GetActualPropertyTypeName(prop)} ,  -            result = result && (obj.{prop.Name} == {prop.Name});");
                                continue;
                            }

                            sb.AppendLine($"            result = result && (obj.{prop.Name} == {prop.Name});");
                        }
                        sb.AppendLine($"            return result;");
                        sb.AppendLine($"        }}");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine();
                sb.AppendLine($"        public {cInfo.Name} GetCopy()");
                sb.AppendLine($"        {{");
                sb.AppendLine($"            var obj = new {cInfo.Name}();");
                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        continue;
                    }
                    sb.AppendLine($"            obj.{prop.Name} = {prop.Name};");
                }
                sb.AppendLine($"            return obj;");
                sb.AppendLine($"        }}");
                sb.AppendLine();

                sb.AppendLine($"        // Property Names");
                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    sb.AppendLine($"        public const string {prop.Name + "__Name"} = \"{prop.Name}\";");
                }

                var sSb = new List<string>();
                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        continue;
                    }

                    var slst = prop.GetCustomAttributes(typeof(StringLengthAttribute), true);
                    if (slst == null || slst.Length == 0)
                    {
                        continue;
                    }
                    sSb.Add($"        public const int {prop.Name + "__Size"} = {((StringLengthAttribute)slst[0]).MaximumLength};");
                }

                if (sSb.Count > 0)
                {
                    sb.AppendLine($"        // String Column Sizes");
                    foreach (var item in sSb)
                    {
                        sb.AppendLine(item);
                    }
                }

                sb.AppendLine($"    }}");
                sb.AppendLine($"");
            }

            sb.AppendLine($"    public class AppHeaderModel");
            sb.AppendLine($"    {{");
            sb.AppendLine($"        [AlternateName(\"x-tenant-entity-guid\")]");
            sb.AppendLine($"        public string TenantEntityGuid {{ get; set; }}");
            sb.AppendLine($"        [AlternateName(\"x-user-role-id\")]");
            sb.AppendLine($"        public long Role {{ get; set; }}");
            sb.AppendLine($"        //Constants");
            sb.AppendLine($"        public const string TenantEntityGuidHeaderValue = \"x-tenant-entity-guid\";");
            sb.AppendLine($"        public const string RoleHeaderValue = \"x-user-role-id\";");

            sb.AppendLine($"    }}");
            sb.AppendLine($"");

            if (!isTest)
            {
                var tE = typeof(Manifest.Common.Constants.ConfigurationConstants);

                var assemblyE = tE.Assembly;
                if (assemblyE == null)
                {
                    Console.WriteLine("Unabel to find the Manifest.Common assembly");
                    return;
                }

                foreach (var enumValue in assemblyE.GetTypes())
                {
                    if (!enumValue.IsEnum)
                        continue;

                    sb.AppendLine($"    public enum {enumValue.Name}");
                    sb.AppendLine($"    {{");
                    var typ = enumValue;
                    var enumValues = System.Enum.GetValues(typ);
                    var underlyingType = System.Enum.GetUnderlyingType(typ);

                    foreach (var val in enumValues)
                    {
                        var actualValue = Convert.ChangeType(val, underlyingType);
                        sb.AppendLine($"        {System.Enum.GetName(typ, val)} = {actualValue},");
                    }

                    sb.AppendLine($"    }}");
                    sb.AppendLine($"");
                }
            }

            sb.AppendLine("}");

            if (isTest)
                CodeGenUtils.CleanAndWriteAndOpenFile(GetTestHttpCommonDirectory() + "\\ApiModels.cs", sb);
            else
            {
                CodeGenUtils.CleanAndWriteAndOpenFile(GetReactModelDirectory() + "\\ApiModels.cs", sb);
            }

            var isb = new StringBuilder();
            isb.AppendLine("using System;");
            isb.AppendLine("using System.Collections.Generic;");
            isb.AppendLine("using System.IO;");
            isb.AppendLine("using Framework.Infrastructure.Models.Result;");
            isb.AppendLine("using Framework.Infrastructure.Utils;");
            //isb.AppendLine("using Framework.Infrastructure.Models.FileInput;");

            if (isTest) { isb.Append(""); } else isb.AppendLine("using Manifest.WordAddIn.Shared.Models;");
            if (isTest) { isb.Append(""); } else isb.AppendLine("using Manifest.WordAddIn.Shared.Services;");
            isb.AppendLine();
            if (isTest) { isb.AppendLine("namespace Manifest.Tests.Common"); } else isb.AppendLine($"namespace Manifest.WordAddIn.Shared.Interfaces");
            isb.AppendLine("{");
            isb.AppendLine($"    public interface IHttpService");
            isb.AppendLine($"    {{");

            sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.IO;");
            if (isTest) { sb.AppendLine("using System.Net.Http;"); }
            if (isTest) { sb.AppendLine("using System.Threading.Tasks;"); }
            sb.AppendLine("using Framework.Infrastructure.Models.Result;");
            sb.AppendLine("using Framework.Infrastructure.Utils;");
            if (isTest) { sb.AppendLine("using Framework.TestUtilies;"); }
            //if (isTest) { sb.AppendLine("using Framework.Infrastructure.Models.FileInput;"); }
            if (isTest) { sb.Append(""); } else sb.AppendLine("using Manifest.WordAddIn.Shared.Interfaces;");
            if (isTest) { sb.Append(""); } else sb.AppendLine("using Manifest.WordAddIn.Shared.Models;");
            if (isTest) { sb.Append(""); } else sb.AppendLine("using Manifest.WordAddIn.Shared.Services;");
            sb.AppendLine();
            if (isTest) { sb.AppendLine("namespace Manifest.Tests.Common"); } else sb.AppendLine($"namespace Manifest.WordAddIn.Shared.Services");
            sb.AppendLine("{");

            if (isTest) { sb.AppendLine("    public class HttpService<T> : IDisposable"); } else sb.AppendLine($"    public class HttpService : IHttpService");
            sb.AppendLine($"    {{");
            if (isTest) { sb.AppendLine("        private readonly string site;"); } else sb.AppendLine($"        private readonly AppConfigService appConfigService;");
            if (isTest) { sb.AppendLine("        private readonly FrameworkTestEnvironment<T> env;"); }
            if (isTest) { sb.AppendLine("        private bool disposedValue;"); }

            if (isTest) { sb.AppendLine("        public HttpService(string site, FrameworkTestEnvironment<T> env)"); } else sb.AppendLine($"        public HttpService(AppConfigService appConfigService)");
            sb.AppendLine($"        {{");
            if (isTest) { sb.AppendLine("            this.site = site ?? \"\";"); } else sb.AppendLine($"            this.appConfigService = appConfigService;");
            if (isTest) { sb.AppendLine("            this.env = env;"); }
            sb.AppendLine($"        }}");
            sb.AppendLine("");
            if (isTest) { sb.AppendLine("        protected virtual void Dispose(bool disposing)"); }
            if (isTest) { sb.AppendLine("        {"); }
            if (isTest) { sb.AppendLine("            if (!disposedValue)"); }
            if (isTest) { sb.AppendLine("            {"); }
            if (isTest) { sb.AppendLine("                disposedValue = true;"); }
            if (isTest) { sb.AppendLine("            }"); }
            if (isTest) { sb.AppendLine("        }"); }
            if (isTest) { sb.AppendLine(""); }
            if (isTest) { sb.AppendLine("        public void Dispose()"); }
            if (isTest) { sb.AppendLine("        {"); }
            if (isTest) { sb.AppendLine("            Dispose(disposing: true);"); }
            if (isTest) { sb.AppendLine("            GC.SuppressFinalize(this);"); }
            if (isTest) { sb.AppendLine("        }"); }
            sb.AppendLine($"         public string Url(string fragment)");
            sb.AppendLine($"         {{");
            if (isTest) { sb.AppendLine("             return RestClientUtils.Url(site, fragment);"); } else sb.AppendLine($"             return RestClientUtils.Url(appConfigService.Site, fragment);");
            sb.AppendLine($"         }}");
            sb.AppendLine("");

            var tokenVar = "token: appConfigService.CurrentToken, headers : appConfigService.CurrentHeaders";
            if (isTest)
                tokenVar = "appHeader = appHeader";

            string lineValue;
            string iLineValue;

            foreach (var funcInfo in combined.FunctionInfoList)
            {
                testSB.Clear();

                foreach (var method in funcInfo.Methods)
                {
                    if (method.CanIgnore)
                    {
                        //if (!isTest)
                        //    continue;

                        if (!method.IsMvcGenericResponse)
                            continue;

                        if (method.CallType != MethodCallType.Get)
                            continue;
                    }

                    //var returnModel = "ReturnModel";
                    var listSuffix = "";
                    if (method.GetOutputParameterTypeWithModuleNamePrefix(isTS).Contains("ReturnListModel"))
                    {
                        listSuffix = "List";
                        //returnModel = "ReturnListModel";
                    }

                    var (modelName, outerClass) = CodeGenUtils.GetModelPrefixedClassNameInfo(isTS, method.OutputParameterType);
                    if (modelName == null)
                        modelName = "";

                    if (modelName == null)
                        modelName = "";

                    if (isTest)
                    {
                        if (method.IsMvcGenericResponse)
                        {
                            lineValue = ($"        public virtual Task<HttpResponseMessage> {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)})");
                            //GenerateTestFunctions(testSB, method.MethodName, isTS, method.InputParameters, isTest);
                        }
                        else
                        {
                            lineValue = ($"        public virtual Task<(HttpResponseMessage response, {method.GetOutputParameterTypeWithModuleNamePrefix(isTS)} result)> {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)})");
                            GenerateTestFunctions(testSB, method.MethodName, isTS, method.InputParameters, isTest);
                        }
                    }
                    else
                    {
                        if (method.IsMvcGenericResponse)
                        {
                            lineValue = ($"        public virtual Stream {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)})");
                        }
                        else
                        {
                            lineValue = ($"        public virtual {method.GetOutputParameterTypeWithModuleNamePrefix(isTS)} {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)})");
                        }
                    }

                    sb.AppendLine(lineValue);
                    lineValue = ($"        {{");
                    sb.AppendLine(lineValue);

                    if (method.IsMvcGenericResponse)
                    {
                        iLineValue = ($"        Stream {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)});");
                    }
                    else
                    {
                        iLineValue = ($"        {method.GetOutputParameterTypeWithModuleNamePrefix(isTS)} {method.MethodName}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters, isTest)});");
                    }


                    isb.AppendLine(iLineValue);

                    var space = "           ";
                    if (method.CallType == MethodCallType.Get)
                    {
                        if (method.InputParameters.Count == 0)
                        {
                            if (isTest)
                            {
                                if (method.IsMvcGenericResponse)
                                {
                                    lineValue = ($"{space}return env.GetWithNewTokenClientAsync<Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                                else
                                {
                                    lineValue = ($"{space}return env.GetAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                            }
                            else
                            {
                                if (method.IsMvcGenericResponse)
                                {
                                    lineValue = ($"{space}return RestClientUtils.Download(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{tokenVar});");
                                }
                                else
                                {
                                    lineValue = ($"{space}return RestClientUtils.Get{listSuffix}<{modelName}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{tokenVar});");
                                }
                            }

                            sb.AppendLine(lineValue);
                        }
                        else
                        {
                            var inputParamType = assembly.GetType(method.InputParameters[0].TypeFullName);

                            if (inputParamType == null)
                                inputParamType = Type.GetType(method.InputParameters[0].TypeFullName);

                            if (method.InputParameters[0].Key == "Nullable`1")
                            {
                                if (method.InputParameters[0].Value == "123~!@#")
                                    continue;
                            }

                            var qry = CodeGenUtils.GetTSFunctionQueryParametersAsString(isTS, method.InputParameters);
                            if (method.InputParameters.Count == 1)
                            {
                                var p = method.InputParameters[0].Value;
                                var s = "";
                                var k = method.InputParameters[0].Key;
                                var actualType = method.InputParameters[0].Key;
                                if (!CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                {
                                    if (method.InputParameters[0].Key == "List`1")
                                    {
                                        if (inputParamType != null)
                                        {
                                            k = $"List<{inputParamType.GetGenericArguments()[0].Name}>";
                                        }
                                        //method.InputParameters.
                                        //actualType = inputParamType.;
                                    }
                                    s = "";
                                }
                                else
                                {
                                    p = "new { " + p + " }";
                                    k = "Object";
                                }

                                if (isTest)
                                {
                                    if (method.IsMvcGenericResponse)
                                    {
                                        lineValue = ($"{space}return env.GetWithNewTokenClientAsync<{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                    }
                                    else
                                    {
                                        lineValue = ($"{space}return env.GetAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                    }
                                }
                                else
                                {
                                    if (method.IsMvcGenericResponse)
                                    {
                                        lineValue = ($"{space}return RestClientUtils.Download(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},{tokenVar});");
                                    }
                                    else
                                    {
                                        lineValue = ($"{space}return RestClientUtils.Get{listSuffix}<{modelName}{s}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},{tokenVar});");
                                    }
                                }
                                sb.AppendLine(lineValue);
                            }
                            else
                            {
                                if (isTest)
                                {
                                    if (method.IsMvcGenericResponse)
                                    {
                                        lineValue = ($"{space}return env.GetWithNewTokenClientAsync<Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{qry},appHeader);");
                                    }
                                    else
                                    {
                                        lineValue = ($"{space}return env.GetAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{qry},appHeader);");
                                    }
                                }
                                else
                                {
                                    if (method.IsMvcGenericResponse)
                                    {
                                        lineValue = ($"{space}return RestClientUtils.Download(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{qry},{tokenVar});");
                                    }
                                    else
                                    {
                                        lineValue = ($"{space}return RestClientUtils.Get{listSuffix}<{modelName}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{qry},{tokenVar});");
                                    }
                                }

                                sb.AppendLine(lineValue);
                            }
                        }
                    }
                    else if (method.CallType == MethodCallType.Post)
                    {
                        if (method.ApiMethodName.IsSameWithCaseIgnoreTrimmed("savesearchconfiguration"))
                        {
                            if (method.ApiMethodName == "Savesearchconfiguration" + "X")
                            {
                                continue;
                            }
                        }

                        if (method.InputParameters.Count > 1)
                            throw new Exception($"Post Not supported with multiple input parameter - Method - {method.MethodName}");
                        if (method.InputParameters.Count == 0)
                        {
                            if (isTest)
                            {
                                if (method.IsMvcGenericResponse)
                                {
                                    lineValue = ($"{space}return env.PostWithNewTokenClientAsync<Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                                else
                                {
                                    lineValue = ($"{space}return env.PostAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                            }
                            else
                                lineValue = ($"{space}return RestClientUtils.Post{listSuffix}<{modelName}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{tokenVar});");
                            sb.AppendLine(lineValue);
                        }
                        else
                        {
                            var p = method.InputParameters[0].Value;
                            var s = ",object";
                            var k = method.InputParameters[0].Key;
                            var inputParamType = assembly.GetType(method.InputParameters[0].TypeFullName);

                            if (inputParamType == null)
                                inputParamType = Type.GetType(method.InputParameters[0].TypeFullName);

                            if (!CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                            {
                                if (method.InputParameters[0].Key == "List`1")
                                {
                                    if (inputParamType != null)
                                    {
                                        k = $"List<{inputParamType.GetGenericArguments()[0].Name}>";
                                    }
                                    s = "," + k;
                                }
                                else
                                    s = "," + method.InputParameters[0].Key;
                            }
                            else
                            {
                                //p = "new { " + p + " }";
                                //k = "Object";
                            }

                            if (isTest)
                            {
                                var httpFunction = "";
                                if (method.HasFormFile(assembly))
                                {
                                    if (CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                    {
                                        httpFunction = "PostAsStringAndReadAsJsonWithNewTokenClientAsync";
                                    }
                                    else
                                    {
                                        httpFunction = "PostAsFormAndReadAsJsonWithNewTokenClientAsync";
                                    }
                                }
                                else
                                {
                                    if (CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                    {
                                        httpFunction = "PostAsStringAndReadAsJsonWithNewTokenClientAsync";
                                    }
                                    else
                                    {
                                        httpFunction = "PostAndReadAsJsonWithNewTokenClientAsync";
                                    }
                                }

                                if (CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                    lineValue = ($"{space}return env.{httpFunction}<Return{listSuffix}Model<{modelName}>,{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                else
                                    lineValue = ($"{space}return env.{httpFunction}<Return{listSuffix}Model<{modelName}>,{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                            }
                            else
                                lineValue = ($"{space}return RestClientUtils.Post{listSuffix}WithModel<{modelName}{s}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},{tokenVar});");
                            sb.AppendLine(lineValue);
                        }
                    }
                    else if (method.CallType == MethodCallType.Put)
                    {
                        if (method.InputParameters.Count > 1)
                        {
                            //CodeGenUtils.MethodParamsCount(method.InputParameters);
                            if (!CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                                throw new Exception($"Put Not supported with multiple complex input parameter - Method - {method.MethodName}");
                        }

                        if (method.InputParameters.Count == 0)
                        {
                            if (isTest)
                            {
                                if (method.IsMvcGenericResponse)
                                {
                                    lineValue = ($"{space}return env.PutWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                                else
                                {
                                    lineValue = ($"{space}return env.PutAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,Object>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),null,appHeader);");
                                }
                            }
                            else
                                lineValue = ($"{space}return RestClientUtils.Put{listSuffix}<{modelName}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{tokenVar});");
                            sb.AppendLine(lineValue);
                        }
                        else
                        {
                            var p = "";
                            var s = "";
                            var k = "";
                            var pLst = new List<string>();

                            foreach (var ip in method.InputParameters)
                            {
                                if (!CodeGenUtils.IsBasicType(ip.Key))
                                {
                                    var inputParamType = assembly.GetType(ip.TypeFullName);

                                    if (inputParamType == null)
                                        inputParamType = Type.GetType(ip.TypeFullName);

                                    if (ip.Key == "List'1")
                                    {
                                        if (inputParamType != null)
                                        {
                                            k = $"List<{inputParamType.GetGenericArguments()[0].Name}>";
                                        }
                                        s = "," + k;
                                    }
                                    else
                                    {
                                        k = ip.Key;
                                        p = ip.Value;
                                        s = "," + ip.Key;
                                    }
                                }
                                else
                                {
                                    if (method.InputParameters.Count == 1)
                                    {
                                        p = ip.Value;
                                        k = ip.Key;
                                    }
                                    else
                                    {
                                        pLst.Add(ip.Value);
                                        k = "Object";
                                        s = ",object";
                                    }
                                }
                            }

                            if (method.InputParameters.Count > 1)
                            {
                                p = "new { " + string.Join(",", pLst) + " }";
                            }

                            if (isTest)
                            {
                                if (method.IsMvcGenericResponse)
                                {
                                    if (method.InputParameters.Count == 1 && CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                        lineValue = ($"{space}return env.PutAsStringWithNewTokenClientAsync<{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                    else
                                        lineValue = ($"{space}return env.PutWithNewTokenClientAsync<{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                }
                                else
                                {
                                    if (method.InputParameters.Count == 1 && CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                                        lineValue = ($"{space}return env.PutAsStringAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                    else
                                        lineValue = ($"{space}return env.PutAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>,{k}>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                                }
                            }
                            else
                                lineValue = ($"{space}return RestClientUtils.Put{listSuffix}WithModel<{modelName}{s}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},{tokenVar});");
                            sb.AppendLine(lineValue);
                        }
                    }
                    else if (method.CallType == MethodCallType.Delete)
                    {
                        if (method.InputParameters.Count > 1)
                            throw new Exception($"Delte Not supported with multiple input parameter  - Method - {method.MethodName}");
                        if (method.InputParameters.Count == 0)
                        {
                            if (isTest)
                                lineValue = ($"{space}return env.DeleteAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),appHeader);");
                            else
                                lineValue = ($"{space}return RestClientUtils.Delete{listSuffix}<{modelName}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{tokenVar});");

                            sb.AppendLine(lineValue);
                        }
                        else
                        {
                            var p = method.InputParameters[0].Value;
                            var s = "";
                            var inputParamType = assembly.GetType(method.InputParameters[0].TypeFullName);

                            if (inputParamType == null)
                                inputParamType = Type.GetType(method.InputParameters[0].TypeFullName);

                            if (!CodeGenUtils.IsBasicType(method.InputParameters[0].Key))
                            {
                                if (method.InputParameters[0].Key == "List'1")
                                {
                                    var k = method.InputParameters[0].Key;
                                    if (inputParamType != null)
                                    {
                                        k = $"List<{inputParamType.GetGenericArguments()[0].Name}>";
                                    }
                                    s = "," + k;
                                }
                                else
                                {
                                    s = "," + method.InputParameters[0].Key;
                                }
                            }
                            else
                                p = "new { " + p + " }";

                            if (isTest)
                                lineValue = ($"{space}return env.DeleteAndReadAsJsonWithNewTokenClientAsync<Return{listSuffix}Model<{modelName}>>(token,Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},appHeader);");
                            else
                                lineValue = ($"{space}return RestClientUtils.Delete{listSuffix}<{modelName}{s}>(Url(\"/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\"),{p},{tokenVar});");
                            sb.AppendLine(lineValue);
                        }
                    }
                    lineValue = ($"        }}");
                    sb.AppendLine(lineValue);

                    lineValue = ($"");
                    sb.AppendLine(lineValue);
                }

                if (isTest)
                {
                    CodeGenUtils.CleanAndWriteAndOpenFile(GetTestGeneratedTestsDirectory() + $"\\{funcInfo.ClassName}_GenrTests.txt", testSB);
                }

            }

            lineValue = ($"    }}");
            sb.AppendLine(lineValue);
            lineValue = ($"}}");
            sb.AppendLine(lineValue);

            iLineValue = ($"    }}");
            isb.AppendLine(iLineValue);
            iLineValue = ($"}}");
            isb.AppendLine(iLineValue);

            if (isTest)
            {
                CodeGenUtils.CleanAndWriteAndOpenFile(GetTestHttpCommonDirectory() + "\\HttpService.cs", sb);
            }
            else
            {
                CodeGenUtils.CleanAndWriteAndOpenFile(GetReactHttpApiDirectory() + "\\SelectService.cs", ssb);
                CodeGenUtils.CleanAndWriteAndOpenFile(GetReactHttpApiDirectory() + "\\HttpService.cs", sb);
                CodeGenUtils.CleanAndWriteAndOpenFile(GetReactInterfaceDirectory() + "\\IHttpService.cs", isb);
            }
        }

        private static void GenerateTestFunctions(StringBuilder testSB, string methodName, bool isTS, List<InputParam> inputParameters, bool isTest)
        {
            if (!(isTest && !isTS))
                return;

            testSB.AppendLine($"public void {methodName}_WithValidData_ReturnsValidData()");
            testSB.AppendLine($"{{");
            if (inputParameters.Count == 0)
            {
                //testSB.AppendLine();
            }
            else if (inputParameters.Count == 1)
            {
                testSB.AppendLine($"    var model = new {inputParameters[0].Key}();");
            }
            else
            {

            }

            testSB.AppendLine($"    var r = {methodName}();");
            testSB.AppendLine($"}}");

            testSB.AppendLine($"public void {methodName}_WithValidInvalidToken_ReturnsUnAuthorizedStatus()");
            testSB.AppendLine($"{{");
            testSB.AppendLine($"    ");
            testSB.AppendLine($"}}");
        }

        private static string GetTestGeneratedTestsDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Tests\\IntegrationTests\\Api\\GeneratedTest";
        }

        private static string GetTestHttpApiDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Tests\\IntegrationTests\\Api\\";
        }

        private static string GetTestHttpCommonDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Tests\\Common\\";
        }

        private static string GetTestModelDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\Backend\\Tests\\IntegrationTests\\Api\\";
        }

        private static string GetReactInterfaceDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-office\\Source\\WordApp\\Shared\\Interfaces\\";
        }

        private static string GetReactHttpApiDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-office\\Source\\WordApp\\Shared\\Services\\";
        }

        private static string GetReactModelDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-office\\Source\\WordApp\\Shared\\Models\\";
        }
    }
}
