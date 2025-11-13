namespace CodeGenerator
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;
    using CodeGenerator.Models;
    using Manifest.Api.All.Controllers;


    public static class ReactModelGeneratorProgram
    {
        public static void MainApp()
        {
            var isTS = true;

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


            var sb = new StringBuilder();
            foreach (var cInfo in combined.TypescriptModelClassInfoList)
            {
                if (cInfo.CanIgnore)
                    continue;

                sb.AppendLine($"export class {cInfo.Name} {{");
                foreach (var prop in cInfo.ModelType.GetProperties())
                {
                    var rlst = prop.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (!(rlst == null || rlst.Length == 0))
                    {
                        continue;
                    }

                    sb.AppendLine($"    {CodeGenUtils.LowerFirstCharacter(prop.Name)} : {CodeGenUtils.GetModelProperty(isTS, prop.PropertyType)} = {CodeGenUtils.GetModelPropertyDefaultValue(isTS, prop.PropertyType)};");
                }

                sb.AppendLine($"}}");
                sb.AppendLine($"");
            }

            CodeGenUtils.CleanAndWriteAndOpenFile(GetReactModelDirectory() + "\\app-model.ts", sb);

            sb = new StringBuilder();
            sb.AppendLine($"import FrameworkHttpClientService from \"app/core/http/FrameworkHttpClientService\";");
            sb.AppendLine($"import {{ ReturnModel }} from \"app/models/return.model\";");
            sb.AppendLine($"import {{ ReturnListModel }} from \"app/models/return-list.model\";");
            sb.AppendLine($"import * as models from \"./app-model\";");
            sb.AppendLine($"");

            sb.AppendLine($"export default {{ ");
            foreach (var funcInfo in combined.FunctionInfoList)
            {

                foreach (var method in funcInfo.Methods)
                {
                    if (method.CanIgnore)
                        continue;

                    var listSuffix = "";
                    if (method.GetOutputParameterTypeWithModuleNamePrefix(isTS, isAngular: false).Contains("ReturnListModel"))
                    {
                        listSuffix = "List";
                    }

                    sb.AppendLine($"    async {CodeGenUtils.LowerFirstCharacter(method.MethodName)}({CodeGenUtils.GetTSFunctionParameter(isTS, method.InputParameters)}): {method.GetOutputParameterTypeWithModuleNamePrefix(isTS)} {{");
                    if (method.CallType == MethodCallType.Get)
                    {
                        if (method.InputParameters.Count == 0)
                        {
                            sb.AppendLine($"        return FrameworkHttpClientService.get{listSuffix}(`/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}`);");
                        }
                        else
                        {

                            var qry = CodeGenUtils.GetTSFunctionQueryParametersAsString(isTS, method.InputParameters);
                            if (method.InputParameters.Count == 1 && !CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                            {
                                qry = method.InputParameters[0].Value;
                            }
                            sb.AppendLine($"        return FrameworkHttpClientService.get{listSuffix}WithModel(`/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}`,{qry});");
                        }
                    }
                    else if (method.CallType == MethodCallType.Post)
                    {
                        if (method.InputParameters.Count > 1)
                        {
                            if (!CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                                throw new Exception($"Post Not supported with multiple complex input parameter  - Method - {method.MethodName}");
                        }

                        if (method.InputParameters.Count == 0)
                        {
                            sb.AppendLine($"        return FrameworkHttpClientService.post{listSuffix}(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\');");
                        }
                        else
                        {
                            var qry = CodeGenUtils.GetTSFunctionQueryParametersAsString(isTS, method.InputParameters);

                            if (method.InputParameters.Count == 1 && !CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                            {
                                qry = method.InputParameters[0].Value;
                            }
                            sb.AppendLine($"        return FrameworkHttpClientService.post{listSuffix}WithModel(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\',{qry});");
                        }
                    }
                    else if (method.CallType == MethodCallType.Put)
                    {
                        if (method.InputParameters.Count > 1)
                        {
                            if (!CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                                throw new Exception($"Put Not supported with multiple complex input parameter  - Method - {method.MethodName}");
                        }

                        if (method.InputParameters.Count == 0)
                        {
                            sb.AppendLine($"        return FrameworkHttpClientService.put{listSuffix}(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\');");
                        }
                        else
                        {
                            var qry = CodeGenUtils.GetTSFunctionQueryParametersAsString(isTS, method.InputParameters);
                            if (method.InputParameters.Count == 1 && !CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                            {
                                qry = method.InputParameters[0].Value;
                            }
                            sb.AppendLine($"        return FrameworkHttpClientService.put{listSuffix}WithModel(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\',{qry});");
                        }
                    }
                    else if (method.CallType == MethodCallType.Delete)
                    {
                        if (method.InputParameters.Count > 1)
                        {
                            if (!CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                                throw new Exception($"Delete Not supported with multiple complex input parameter  - Method - {method.MethodName}");

                        }

                        if (method.InputParameters.Count == 0)
                        {
                            sb.AppendLine($"        return FrameworkHttpClientService.delete{listSuffix}(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\');");
                        }
                        else
                        {
                            var qry = CodeGenUtils.GetTSFunctionQueryParametersAsString(isTS, method.InputParameters);
                            if (method.InputParameters.Count == 1 && !CodeGenUtils.IsAllAppParamBasicDataType(method.InputParameters))
                            {
                                qry = method.InputParameters[0].Value;
                            }
                            sb.AppendLine($"        return FrameworkHttpClientService.delete{listSuffix}WithModel(\'/api/v1/{funcInfo.ApiClassName.ToLower()}/{method.ApiMethodName.ToLower()}\',{qry});");
                        }
                    }
                    else
                    {
                        throw new Exception($"{method.CallType} Not supported  - Method - {method.MethodName}");
                    }

                    sb.AppendLine($"    }},");
                    sb.AppendLine($"");
                }
            }

            sb.AppendLine($"}}");
            CodeGenUtils.CleanAndWriteAndOpenFile(GetReactHttpApiDirectory() + "\\app-http-client.ts", sb);
        }

        private static string GetReactHttpApiDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\react-ui\\src\\app\\services\\API\\";
        }

        private static string GetReactModelDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\..\\app-ui\\react-ui\\src\\app\\services\\API\\";
        }



    }
}
