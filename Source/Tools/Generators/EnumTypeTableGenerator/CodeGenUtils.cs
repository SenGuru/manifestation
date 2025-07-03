/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using CodeGenerator.Models;
using Framework.Infrastructure.Attributes;
using Framework.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeGenerator
{
    public static class CodeGenUtils
    {
        public static void CleanAndWriteAndOpenFile(string fileName, StringBuilder sb)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.AppendAllText(fileName, sb.ToString());
            //Process.Start("notepad.exe", fileName);
        }


        public static CombinedClass GetParsedStructure(Assembly assembly)
        {
            var combined = new CombinedClass();
            combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "Object", ModelType = typeof(Object), CanIgnore = true });
            combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "object", ModelType = typeof(object), CanIgnore = true });
            //combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "MethodBase", ModelType = typeof(MethodBase), CanIgnore = true });
            //combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "Exception", ModelType = typeof(Exception), CanIgnore = true });
            //combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "ReturnException", ModelType = typeof(ReturnException), CanIgnore = false });
            //combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "DasaModel", ModelType = typeof(DasaModel), CanIgnore = false });
            //combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = "IntPtr", ModelType = typeof(IntPtr), CanIgnore = false });

            foreach (var aClass in assembly.GetTypes())
            {
                if (!typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(aClass))
                    continue;

                var cItem = new TClass { ClassName = aClass.Name };
                combined.FunctionInfoList.Add(cItem);

                var methodInfos = aClass.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

                if (methodInfos.Length == 0)
                    continue;

                foreach (var method in methodInfos)
                {
                    var ignoredAttribute = method.GetCustomAttribute<IgnoreForCodeGenerationAttribute>();
                    if (ignoredAttribute != null)
                        continue;
                    //if (method.Name == "UpdateEntityProfileFields")
                    //{
                    //    if (method.Name + "X" == "UpdateEntityProfileFields")
                    //        continue;
                    //}

                    MethodCallType callType;

                    var methodParams = method.GetParameters();
                    var returnType = method.ReturnType;

                    if (method.GetCustomAttribute(typeof(HttpPostAttribute)) != null)
                    {
                        if (!DoesMethodParamsOneOrNoBody(methodParams))
                        {
                            throw new NotImplementedException($"This Post scenario is not implemented - {aClass.Name} - {method.Name}");
                        }

                        callType = MethodCallType.Post;
                    }
                    else if (method.GetCustomAttribute(typeof(HttpGetAttribute)) != null)
                    {
                        callType = MethodCallType.Get;
                        if (DoesMethodParamsAnyBody(methodParams))
                            throw new NotImplementedException($"This get scenario is not implemented - {aClass.Name} - {method.Name}");
                    }
                    else if (method.GetCustomAttribute(typeof(HttpPutAttribute)) != null)
                    {
                        if (!DoesMethodParamsOneOrNoBody(methodParams))
                        {
                            throw new NotImplementedException($"This put scenario is not implemented - {aClass.Name} - {method.Name}");
                        }
                        callType = MethodCallType.Put;
                    }
                    else if (method.GetCustomAttribute(typeof(HttpDeleteAttribute)) != null)
                    {
                        if (DoesMethodParamsAnyBody(methodParams))
                            throw new NotImplementedException($"This delete scenario is not implemented - {aClass.Name} - {method.Name}");
                        callType = MethodCallType.Delete;
                    }
                    else
                    {
                        throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");
                    }

                    var rName = returnType.Name;
                    var underlysingReturnType = returnType;

                    if (returnType.Name == "Guid[]")
                    {
                        throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");
                    }

                    if (returnType.BaseType == typeof(System.Threading.Tasks.Task))
                    {
                        (rName, underlysingReturnType) = CodeGenUtils.ProcessGenericVariable(combined, returnType);
                    }
                    else
                    {
                        if (returnType.IsGenericType)
                            (rName, underlysingReturnType) = CodeGenUtils.ProcessGenericVariable(combined, returnType);
                    }

                    var mItem = new TMethod { MethodName = method.Name, CallType = callType, OutputParameterType = rName, IsMvcGenericResponse = CodeGenUtils.IsGenericMvcResponseType(underlysingReturnType), CanIgnore = CodeGenUtils.CanIgnoreReturnType(underlysingReturnType) };
                    cItem.Methods.Add(mItem);

                    //var inputParam = "";
                    foreach (var param in methodParams)
                    {
                        var ppType = param.ParameterType;
                        //if (param.ParameterType.Name == "Guid[]")
                        if (ppType.IsGenericType)
                        {
                            if (ppType.Name == "Guid[]")
                                throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");

                            if (ppType.GetGenericArguments().Length > 0)
                            {
                                if (!combined.TypescriptModelClassInfoList.Any(x => x.Name == param.ParameterType.GetGenericArguments()[0].Name))
                                {
                                    combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = param.ParameterType.GetGenericArguments()[0].Name, ModelType = param.ParameterType.GetGenericArguments()[0], CanIgnore = CanIgnoreReturnType(param.ParameterType.GetGenericArguments()[0]) });
                                }
                            }

                            var genicClass = GetGenericClassName(ppType.Name);
                            if (!(genicClass == "List" || genicClass == "Array" || genicClass == "IEnumerable" || genicClass == "IList"))
                            {
                                //throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");
                            }
                        }
                        else if (ppType.IsArray)
                        {
                            if (ppType.Name == "Guid[]")
                                throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");

                            var genicClass = GetGenericClassName(ppType.Name);
                            if (!(genicClass == "List" || genicClass == "Array" || genicClass == "IEnumerable" || genicClass == "IList"))
                            {
                                //throw new NotImplementedException($"This scenario is not implemented-{aClass.Name} - {method.Name}");
                            }
                        }

                        if (!combined.TypescriptModelClassInfoList.Any(x => x.Name == ppType.Name))
                            combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = ppType.Name, ModelType = ppType, CanIgnore = CanIgnoreReturnType(ppType) });

                        mItem.InputParameters.Add(new InputParam { Key = ppType.Name, Value = param.Name, Type = ppType, TypeFullName = ppType.FullName, InputType = GetInputType(param) });
                    }
                }
            }
            return combined;

        }

        private static string GetPrefixedSpecialNames(bool isTS, string paramName)
        {
            if (isTS)
                return paramName;

            if (paramName == "event")
            {
                return "@" + paramName;
            }
            return paramName;
        }

        private static InputTypes GetInputType(ParameterInfo param)
        {
            var lst = param.GetCustomAttributes(typeof(FromBodyAttribute));
            if (lst != null || lst.Any())
                return InputTypes.Body;

            lst = param.GetCustomAttributes(typeof(FromQueryAttribute));
            if (lst != null || lst.Any())
                return InputTypes.Body;

            throw new Exception($"Unable to find suitable Parameter type for - {param.Name}");
        }

        public static (int QryCount, int BdyCount) MethodParamsCount(ParameterInfo[] methodParams)
        {
            if (methodParams == null || methodParams.Length == 0)
            {
                return (0, 0);
            }

            var bodyCount = 0;
            var queryCount = 0;

            foreach (var met in methodParams)
            {
                var lst = met.GetCustomAttributes(typeof(FromQueryAttribute));
                if (lst != null)
                    queryCount = lst.Count();

                lst = met.GetCustomAttributes(typeof(FromBodyAttribute));
                if (lst != null)
                    bodyCount = lst.Count();
            }

            return (queryCount, bodyCount);
        }

        public static bool DoesMethodParamsAnyBody(ParameterInfo[] methodParams)
        {
            return (MethodParamsCount(methodParams).BdyCount > 0);
        }

        public static bool DoesMethodParamsOneOrNoBody(ParameterInfo[] methodParams)
        {
            if (MethodParamsCount(methodParams).BdyCount <= 1)
            {
                return true;
            }

            return false;
        }

        public static string GetModelPropertyNullOption(bool isTS, Type propertyType, bool isNull = false)
        {

            if (!isTS)
                return "";

            var modelType = GetModelProperty(isTS, propertyType);
            if (IsFormFile(modelType))
            {
                return " | null";
            }

            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                return "";
            }
            else
            {
                if (propertyType.IsGenericType)
                {
                    var genicClass = GetGenericClassName(propertyType.Name);
                    if (genicClass == "List" || genicClass == "Array" || genicClass == "IEnumerable" || genicClass == "IList")
                    {
                        return " | null";
                    }
                    else
                        return "";
                }
                else if (propertyType.IsArray)
                {
                    return " | null";
                }
                else
                {
                    return "";
                }
            }
        }

        public static object GetModelPropertyDefaultValue(bool isTS, Type propertyType, bool isNull = false, bool isDart = false)
        {
            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                if (propertyType.GetGenericArguments()[0].IsEnum)
                    return "0";

                return GetTSTypeDefaultValue(isTS, propertyType.GetGenericArguments()[0], true, isDart : isDart);
            }
            else
            {
                if (propertyType.IsEnum)
                    return "0";

                if (propertyType.IsGenericType)
                {
                    var genicClass = GetGenericClassName(propertyType.Name);
                    if (genicClass == "List" || genicClass == "Array" || genicClass == "IEnumerable" || genicClass == "IList")
                    {
                        if (isTS)
                            return isNull ? "null" : "[]";
                        else
                            return "null";
                    }
                    else
                        return GetTSTypeDefaultValue(isTS, propertyType.GetGenericArguments()[0], false, isDart: isDart);
                }
                else if (propertyType.IsArray)
                {
                    var pName = propertyType.Name.Substring(0, propertyType.Name.Length - 2);
                    if (pName.ToLower() == "byte")
                        return "null";

                    if (isTS)
                        return "null";
                    else
                        return "null";
                }
                else
                {
                    return GetTSTypeDefaultValue(isTS, propertyType, false, isDart: isDart);
                }
            }
        }

        public static string GetActualPropertyTypeName(PropertyInfo prop)
        {
            var r = GetActualPropertyTypeNameAndInternalType(prop);
            return r.name;
        }

        public static Type GetActualPropertyTypeInternalType(PropertyInfo prop)
        {
            var r = GetActualPropertyTypeNameAndInternalType(prop);
            return r.type;
        }

        public static (string name,Type type) GetActualPropertyTypeNameAndInternalType(PropertyInfo prop)
        {
            if (prop == null)
                return (null,null);

            if (!(IsArrayType(prop.PropertyType) || IsNullable(prop.PropertyType) || prop.PropertyType.IsGenericType))
                return (prop.PropertyType.Name, prop.PropertyType);

            string actualPropName;
            Type actualPropType;
            if (prop.PropertyType.IsArray)
            {
                if (!prop.PropertyType.Name.EndsWith("[]"))
                {
                    throw new Exception($"Array type does not end with [] for property {prop.PropertyType.Name}");
                }
                actualPropName = prop.PropertyType.Name.Substring(0, prop.PropertyType.Name.Length - 2);
                actualPropType = prop.PropertyType;
            }
            else
            {
                var newPropType = prop.PropertyType.GetGenericArguments()[0];
                actualPropName = newPropType.Name;
                actualPropType = newPropType;
            }

            return (actualPropName,actualPropType);
        }

        public static void ProcessDependentModelSubProperty(Type modelType, ref CombinedClass combined, PropertyInfo prop, ref List<TypescriptModelClass> missingClasses)
        {
            var ignoredProp = prop.GetCustomAttribute<JsonIgnoreAttribute>();
            if (ignoredProp != null)
            {
                return;
            }

            Console.WriteLine($"Inspecting :  Model = {modelType.Name} , Prop Reflected - {prop.ReflectedType.FullName} , Prop Name - {prop.Name} , Prop Type - {prop.PropertyType.FullName}");

            if (modelType.Name == "BudgetDivisionModel")
            {
                if (prop.PropertyType.IsGenericType)
                {
                    if (prop.PropertyType.Name == "")
                    {
                        return;
                    }
                }
            }
            
            if (modelType.Name == "CustomerHoroscopeModel")
            {
                if (prop.Name == "SubDasas")
                {
                    if (prop.PropertyType.Name == "")
                    {
                        return;
                    }
                }

                if (prop.PropertyType.IsGenericType)
                {
                    if (prop.PropertyType.Name == "")
                    {
                        return;
                    }
                }
            }

            if (prop.PropertyType.IsGenericType)
            {
                if (prop.PropertyType.Name == "List`1" || prop.PropertyType.Name == "IList" || prop.PropertyType.Name == "IList`1")
                {
                    var innerType = prop.PropertyType.GetGenericArguments()[0];
                    if (innerType.Name == "")
                        return;
                }
            }
            /*
            if (modelType.Name == prop.PropertyType.Name)
            {
                return;
            }

            if (prop.PropertyType.IsGenericType)
            {
                var innerTypeName = GetGenericInnerType(prop.PropertyType.Name);
                if (innerTypeName == modelType.Name)
                    return;

                if (prop.PropertyType.GetGenericArguments().Count == 1)
                {
                    var innerType = prop.PropertyType.GetGenericArguments()[0];

                    if (IsBasicType(innerType.Name))
                    {
                        return;
                    }
                }
            }
            else
            {
                if (IsBasicType(prop.PropertyType.Name))
                {
                    return;
                }
            }
            */

            if (IsBasicType(prop.PropertyType.Name))
            {
                return;
            }

            if (missingClasses.Any(x => x.Name == prop.PropertyType.Name) || combined.TypescriptModelClassInfoList.Any(x => x.Name == prop.PropertyType.Name))
            {
                foreach (var subprop in prop.PropertyType.GetProperties())
                {
                    if (modelType.Name == subprop.PropertyType.Name)
                    {
                        continue;
                    }

                    if (subprop.PropertyType.IsGenericType && (subprop.PropertyType.GetGenericArguments()[0].Name == modelType.Name))
                    {
                        continue;
                    }

                    ProcessDependentModelSubProperty(modelType, ref combined, subprop, ref missingClasses);
                }
                return;
            }

            if (IsArrayType(prop.PropertyType) || IsNullable(prop.PropertyType) || prop.PropertyType.IsGenericType)
            {
                var actualPropName = "";
                Type actualPropType = null;
                if (prop.PropertyType.IsArray)
                {
                    if (!prop.PropertyType.Name.EndsWith("[]"))
                    {
                        throw new Exception($"Array type does not end with [] for property {prop.PropertyType.Name}");
                    }
                    actualPropName = prop.PropertyType.Name.Substring(0, prop.PropertyType.Name.Length - 2);
                    actualPropType = prop.PropertyType;
                }
                else
                {
                    var newPropType = prop.PropertyType.GetGenericArguments()[0];
                    actualPropName = newPropType.Name;
                    actualPropType = newPropType;
                }

                if (IsBasicType(actualPropName))
                {
                    return;
                }

                if (missingClasses.Any(x => x.Name == actualPropName) || combined.TypescriptModelClassInfoList.Any(x => x.Name == actualPropName))
                {
                    foreach (var subprop in actualPropType.GetProperties())
                    {
                        if (modelType.Name == subprop.PropertyType.Name)
                        {
                            continue;
                        }

                        if (subprop.PropertyType.IsGenericType && (subprop.PropertyType.GetGenericArguments()[0].Name == modelType.Name))
                        {
                            continue;
                        }

                        ProcessDependentModelSubProperty(actualPropType, ref combined, subprop, ref missingClasses);
                    }
                    return;
                }

                foreach (var subprop in actualPropType.GetProperties())
                {
                    if (modelType.Name == subprop.PropertyType.Name)
                    {
                        continue;
                    }

                    if (subprop.PropertyType.IsGenericType && (subprop.PropertyType.GetGenericArguments()[0].Name == modelType.Name))
                    {
                        continue;
                    }

                    ProcessDependentModelSubProperty(modelType, ref combined, subprop, ref missingClasses);
                }

                missingClasses.Add(new TypescriptModelClass { CanIgnore = false, Name = actualPropName, ModelType = actualPropType });
                return;
            }

            foreach (var subprop in prop.PropertyType.GetProperties())
            {
                if (modelType.Name == subprop.PropertyType.Name)
                {
                    continue;
                }

                if (subprop.PropertyType.IsGenericType && (subprop.PropertyType.GetGenericArguments()[0].Name == modelType.Name))
                {
                    continue;
                }

                ProcessDependentModelSubProperty(modelType, ref combined, subprop, ref missingClasses);
            }

            missingClasses.Add(new TypescriptModelClass { CanIgnore = false, Name = prop.PropertyType.Name, ModelType = prop.PropertyType });

        }


        public static void AddDependentModels(ref CombinedClass combined)
        {
            var missingClasses = new List<TypescriptModelClass>();
            foreach (var cls in combined.TypescriptModelClassInfoList)
            {
                foreach (var prop in cls.ModelType.GetProperties())
                {
                    if (prop.Name == "ProfileImage")
                    {
                        if (prop.PropertyType.Name == "")
                            continue;
                    }

                    var ignoredProp = prop.GetCustomAttribute<JsonIgnoreAttribute>();
                    if (ignoredProp != null)
                    {
                        continue;
                    }

                    Console.WriteLine($"Processing Property : {cls.ModelType.Name + "." + prop.Name}");
                    if (cls.ModelType.Name == prop.PropertyType.Name)
                    {
                        continue;
                    }

                    if (prop.PropertyType.IsGenericType && (prop.PropertyType.GetGenericArguments()[0].Name == cls.ModelType.Name))
                    {
                        continue;
                    }

                    ProcessDependentModelSubProperty(cls.ModelType, ref combined, prop, ref missingClasses);
                }
            }

            combined.TypescriptModelClassInfoList.AddRange(missingClasses);
        }

        public static bool IsArrayType(Type propertyType)
        {
            if (!(propertyType.IsGenericType || propertyType.IsArray))
                return false;

            if (IsNullable(propertyType))
                return false;

            var gen = GetGenericClassName(propertyType.Name).ToLower();
            if (gen.StartsWith("list`") || gen == "ilist" || gen == "list" || gen == "array" || gen == "ienumerable" || gen.EndsWith("[]"))
            {
                return true;
            }

            throw new NotImplementedException($"{gen} is not implemented");
        }

        public static (string, Type) ProcessGenericVariable(CombinedClass combined, Type returnType)
        {
            string rName;
            var args = returnType.GenericTypeArguments;
            if (args.Length != 1)
            {
                throw new NotImplementedException("This scenario is not implemented");
            }

            var innerType = "";
            rName = args[0].Name;

            if (args[0].IsGenericType)
            {
                var innerArg = args[0].GenericTypeArguments;
                if (innerArg.Length != 1)
                {
                    throw new NotImplementedException("This scenario is not implemented");
                }
                innerType = innerArg[0].Name ?? "";

                if (!combined.TypescriptModelClassInfoList.Any(x => x.Name == innerType))
                    combined.TypescriptModelClassInfoList.Add(new TypescriptModelClass { Name = innerType, ModelType = innerArg[0], CanIgnore = CanIgnoreReturnType(innerArg[0]) });

                rName = args[0].GetGenericTypeDefinition().Name;
                rName = GetGenericInnerType(rName);
            }
            else if (args[0].IsArray)
            {
                if (args[0].Name == "")
                    throw new Exception("This will never be caught");
            }

            if (!innerType.IsNullOrEmpty())
                rName = rName + "<" + innerType + ">";
            return (rName, args[0]);
        }

        public static string GetGenericInnerType(string str)
        {
            var iBacktick = str.IndexOf('`');
            if (iBacktick > 0)
            {
                str = str.Remove(iBacktick);
            }
            return str;
        }

        public static bool IsAllAppParamBasicDataType(List<InputParam> inputParameters)
        {
            foreach (var item in inputParameters)
            {
                if (!CodeGenUtils.IsBasicType(item.Key))
                    return false;
            }

            return true;
        }

        public static string GetGenericClassName(string str)
        {
            var iBacktick = str.IndexOf('`');
            if (iBacktick > 0)
            {
                str = str.Substring(0, iBacktick);
            }
            return str;
        }
        public static string GetTSType(bool isTS, string csTypeName, bool isAngular = true, bool isDart = false)
        {
            return GetTSType(isTS, csTypeName, null, isAngular, isDart);
        }

        public static string GetTSType(bool isTS, Type csType, bool isAngular = true, bool isDart = false)
        {
            return GetTSType(isTS, null, csType, isAngular, isDart);

        }
        public static string GetTSType(bool isTS, string csTypeName, Type csType, bool isAngular = true, bool isDart = false)
        {
            var typeName = csType != null ? csType.Name : (csTypeName != null ? csTypeName:"");

            var lowerTypeName = typeName.ToLower();


            if (!isTS)
            {
                if (lowerTypeName == "iformfile")
                    return "FileStreamModel" + (isDart ? "?" : "");

                if (!isDart)
                {
                    return typeName;
                }                
            }

            var result = typeName;

            if (lowerTypeName == "string" || lowerTypeName == "guid" || lowerTypeName == "char")
                result = isDart ? "String" : "string";
            else if (lowerTypeName == "int" || lowerTypeName == "long" || lowerTypeName == "int32" || lowerTypeName == "int64" || lowerTypeName == "single")
                result = isDart ? "int" : "number";
            else if (lowerTypeName == "float" || lowerTypeName == "double")
                result = isDart ? "double" : "number";
            else if (lowerTypeName == "decimal")
                result = isDart ? "double" : "number";
            if (lowerTypeName == "bool" || lowerTypeName == "boolean")
                result = isDart ? "bool" : "boolean";
            if (lowerTypeName == "datetime" || lowerTypeName == "datetimeoffset")
                result = isDart ? "DateTime" : "Date";
            if (lowerTypeName == "byte")
                result = isDart ? "String" : "string";
            if (lowerTypeName == "iformfile")
                result = isAngular ? "File | null" : "FormData";

            return result;
        }

        public static bool IsFormFile(string csTypeName)
        {
            var lowerTypeName = csTypeName.ToLower();
            if (lowerTypeName == "iformfile" || lowerTypeName == "file")
                return true;
            return false;
        }

        public static string GetTSType(bool isTS, InputParam item)
        {
            if (item.Key == "List`1" || item.Key == "IList`1" || item.Key == "IList")
            {
                if (isTS)
                {
                    var i = Type.GetType(item.TypeFullName).GetGenericArguments()[0];
                    return GetTSType(isTS, i) + "[]";
                }
                else
                {
                    var i = Type.GetType(item.TypeFullName).GetGenericArguments()[0];
                    return "List<" + GetTSType(isTS, i) + ">";
                }
            }
            return GetTSType(isTS, item.Key);
        }

        public static string GetTSTypeDefaultValue(bool isTS, Type csType, bool isNull = false, bool isDart = false)
        {
            if (!isTS)
            {
                if (!isDart)
                    return "default(" + csType.Name + ")";
            }

            var lowerTypeName = csType.Name.ToLower();
            if (lowerTypeName == "string" || lowerTypeName == "guid" || lowerTypeName == "char" || lowerTypeName == "byte")
                return isNull ? "null" : "\'\'";
            else if (lowerTypeName == "int" || lowerTypeName == "long" || lowerTypeName == "int8" || lowerTypeName == "int16" || lowerTypeName == "int32" || lowerTypeName == "int64" || lowerTypeName == "single" || lowerTypeName == "float" || lowerTypeName == "double" || lowerTypeName == "decimal")
                return isNull ? "null" : "0";
            if (lowerTypeName == "bool" || lowerTypeName == "boolean")
                return "false";
            if (lowerTypeName == "datetime" || lowerTypeName == "datetimeoffset")
                return isNull ? "null" : (isDart ? "new DateTime(1970)" : "new Date()");
            if (lowerTypeName == "iformfile")
                return "null";
            return "new " + csType.Name + "()";
        }


        public static string GetTSFunctionQueryParametersAsString(bool isTS, List<InputParam> inputParameters)
        {
            var stLst = new List<string>();
            foreach (var item in inputParameters)
            {
                //if (isTS)
                //    stLst.Add($"{item.Value} : {item.Value}");
                //else
                stLst.Add($"{GetPrefixedSpecialNames(isTS, item.Value)}");
            }
            var p = "{" + string.Join(", ", stLst) + "}";
            if (isTS)
                return p;
            else
                return "new " + p;
        }

        public static object GetTSFunctionParameter(bool isTS, List<InputParam> tsParameters, bool isTest = false, bool isAngular = true, bool isMvResponse = false)
        {
            var stLst = new List<string>();
            foreach (var item in tsParameters)
            {
                if (isTS)
                    stLst.Add($"{item.Value}: {ModelPrefixedClassName(isTS, GetTSType(isTS, item))}");
                else
                    stLst.Add($"{ModelPrefixedClassName(isTS, GetTSType(isTS, item))} {GetPrefixedSpecialNames(isTS, item.Value)}");
            }

            if (!isTS)
            {
                if (isTest)
                {
                    stLst.Add("string token = null");
                    stLst.Add("AppHeaderModel appHeader = null");
                }
            }
            else
            {
                if (!isMvResponse)
                {
                    if (isAngular)
                        stLst.Add("headers ?: HttpHeaders");
                    else
                        stLst.Add("headers ?: Headers");
                }
                else
                {
                    stLst.Add("accessToken ?: string");
                }
            }

            return string.Join(", ", stLst);
        }

        public static bool IsGenericMvcResponseType(Type returnType)
        {
            if (returnType == typeof(ActionResult))
                return true;
            if (returnType == typeof(ActionResult<>))
                return true;
            if (returnType == typeof(HttpResponseMessage))
                return true;

            if (returnType.IsAssignableFrom(typeof(IActionResult)))
                return true;

            return false;
        }

        public static bool CanIgnoreReturnType(Type returnType)
        {
            if (returnType.IsValueType)
                return true;
            if (returnType == typeof(string))
                return true;
            if (returnType == typeof(string))
                return true;
            if (returnType == typeof(ActionResult))
                return true;
            if (returnType == typeof(List<>))
                return true;
            if (returnType == typeof(ActionResult<>))
                return true;
            if (returnType == typeof(HttpResponseMessage))
                return true;

            if (returnType.IsAssignableFrom(typeof(IActionResult)))
                return true;

            return false;
        }

        public static string GetModelProperty(bool isTS, Type propertyType, bool isAngular = true, bool isDart = false)
        {
            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                var prefix = "";
                var suffix = "";
                if (propertyType.GetGenericArguments()[0].IsEnum)
                {
                    prefix = "AppEnums.";
                    suffix = " | number";
                }

                var r = GetTSType(isTS, propertyType.GetGenericArguments()[0].Name, isAngular, isDart);
                if (isTS)
                    return prefix+r + " | null" + suffix;
                else
                    return r + "?";
            }
            else
            {
                var prefix = "";
                var suffix = "";

                if (propertyType.IsEnum)
                {
                    prefix = "AppEnums.";
                    suffix = " | number";
                }

                if (propertyType.IsGenericType)
                {
                    var genicClass = GetGenericClassName(propertyType.Name);

                    if (genicClass == "List" || genicClass == "Array" || genicClass == "IEnumerable" || genicClass == "IList")
                    {
                        if (propertyType.GetGenericArguments()[0].IsEnum)
                        {
                            prefix = "AppEnums.";
                            suffix = " | number";
                        }

                        var r = GetTSType(isTS, propertyType.GetGenericArguments()[0].Name, isAngular, isDart);
                        if (Nullable.GetUnderlyingType(propertyType.GetGenericArguments()[0]) != null)
                        {
                            var t = Nullable.GetUnderlyingType(propertyType.GetGenericArguments()[0]);

                            if (t.IsEnum)
                            {
                                prefix = "AppEnums.";
                                suffix = " | number";
                            }

                            r = GetTSType(isTS, t.Name, isAngular, isDart);
                            if (isTS)
                                r = prefix + r + " | null" + suffix;
                            else
                                r = r + "?";
                        }

                        if (isTS)
                            return prefix + r + "[] | null";
                        else
                            return "List<" + r + ">" + (isDart ? "?" : "");
                    }
                    else
                    {
                        prefix = "";
                        suffix = "";

                        if (propertyType.GetGenericArguments()[0].IsEnum)
                        {
                            prefix = "AppEnums.";
                            suffix = " | number";
                        }

                        if (!isTS)
                        {
                            prefix = "";
                            suffix = "";
                        }

                        return prefix + GetTSType(isTS, propertyType.GetGenericArguments()[0].Name, isAngular, isDart) + suffix;
                    }
                }
                else if (propertyType.IsArray)
                {
                    prefix = "";
                    suffix = "";

                    if (propertyType.IsEnum)
                    {
                        prefix = "AppEnums.";
                        suffix = " | number";
                    }

                    var pName = propertyType.Name.Substring(0, propertyType.Name.Length - 2);
                    if (pName.ToLower() == "byte")
                        return "string";

                    string r;

                    if (propertyType.GetGenericArguments() == null || propertyType.GetGenericArguments().Count() == 0)
                    {
                        r = GetTSType(isTS, pName, isAngular,isDart);
                    }
                    else
                    {
                        if (propertyType.GetGenericArguments()[0].IsEnum)
                        {
                            prefix = "AppEnums.";
                            //suffix = " | number";
                        }
                        else
                        {
                            prefix = "";
                        }

                        r = GetTSType(isTS, propertyType.GetGenericArguments()[0].Name, isAngular, isDart);

                        if (Nullable.GetUnderlyingType(propertyType.GetGenericArguments()[0]) != null)
                        {
                            var t = Nullable.GetUnderlyingType(propertyType.GetGenericArguments()[0]);
                            if (t != null && t.IsEnum)
                            {
                                prefix = "AppEnums.";
                                //suffix = " | number";
                            }
                            else
                            {
                                prefix = "";
                            }

                            r = GetTSType(isTS, t, isAngular, isDart);
                            if (isTS)
                                r = prefix + r + " | null";
                            else
                                r = r + "?";
                        }
                    }

                    if (isTS)
                        return prefix + r + "[] | null";
                    else
                        return "List<" + r + ">" + (isDart ? "?" : "");
                }
                else
                {
                    if (!isTS)
                    {
                        prefix = "";
                        suffix = "";
                    }

                    return prefix + GetTSType(isTS, propertyType, isAngular, isDart) + suffix;
                }
            }
        }

        public static bool IsNullable(Type propertyType)
        {
            return (Nullable.GetUnderlyingType(propertyType) != null);
        }

        public static bool IsBasicType(string name)
        {
            name = name.ToLower();
            return (name == "short" || name == "byte" || name == "boolean" || name == "timespan" || name == "datetimeoffset" || name == "datetime" || name == "string" || name == "char" || name == "guid" || name == "long" || name == "int32" || name == "int64" || name == "int" || name == "int16" || name == "decimal" || name == "double" || name == "single" || name == "float" || name == "short" || name == "boolean" || name == "bool");
        }

        public static bool IsTSBasicTypeWithArray(string name)
        {
            name = name.ToLower();
            return (name == "date[]" || name == "number[]" || name == "string[]" || name == "boolean[]");
        }

        public static bool IsTSBasicType(string name)
        {
            name = name.ToLower();
            return (name == "date" || name == "number" || name == "string" || name == "boolean" || name == "boolean");
        }

        public static string LowerFirstCharacter(string str)
        {
            if (str == null || str.Trim() == "")
                return str;
            return str[0].ToString().ToLower() + str.Substring(1);
        }


        public static (string, string) GetModelPrefixedClassNameInfo(bool isTS, string pType)
        {
            if (pType == null || pType.Trim() == "")
                return (pType, null);

            var modelName = pType;
            var outerClassName = "";
            if (pType.StartsWith("ReturnModel<"))
            {
                pType = pType.Substring("ReturnModel<".Length);
                modelName = pType.Substring(0, pType.Length - 1);
                outerClassName = "ReturnModel";
            }
            else if (pType.StartsWith("ReturnListModel<"))
            {
                pType = pType.Substring("ReturnListModel<".Length);
                modelName = pType.Substring(0, pType.Length - 1);
                outerClassName = "ReturnListModel";
            }

            if (!IsBasicType(modelName))
            {
                if (isTS)
                {
                    if (IsTSBasicTypeWithArray(modelName) || IsTSBasicType(modelName))
                        modelName = GetTSType(isTS, modelName);
                    else
                        modelName = "models." + modelName;
                }
            }
            else
            {
                if (isTS)
                    modelName = GetTSType(isTS, modelName);
            }

            //if (outerClassName != "")
            //    modelName = outerClassName + "<" + modelName + ">";

            return (modelName, outerClassName);
        }

        public static string ModelPrefixedClassName(bool isTS, string pType)
        {
            var (modelName, outerClass) = GetModelPrefixedClassNameInfo(isTS, pType);
            if (modelName == null)
                return null;

            if (outerClass != "")
                modelName = outerClass + "<" + modelName + ">";
            //if(modelName == "List`1")
            //    return "<"+modelName + ">";
            return modelName;
        }

        public static bool IsEnumAvailable(string enumName)
        {
            var t = Type.GetType(enumName);
            if (t == null)
                return false;
            return t.IsEnum;
        }

        public static bool IsPropertyIgnoreForSearchFilter(string propertyName)
        {
            return (propertyName.IsSameWithCaseIgnoreTrimmed("createdByUserId") || propertyName.IsSameWithCaseIgnoreTrimmed("modifiedByUserId") || propertyName.IsSameWithCaseIgnoreTrimmed("companyIdX"));
        }

        public static Type GetClassFromAssembly(Assembly assembly, string className)
        {
            var lst = new List<string>();
            Type result = null;
            foreach (var aClass in assembly.GetTypes())
            {
                lst.Add(aClass.Name);

                if (!aClass.Name.Equals(className, StringComparison.CurrentCultureIgnoreCase))
                    continue;
                result = aClass;
                break;
            }
            return result;
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

    }

    public enum InputTypes
    {
        Body,
        Query
    }
}
