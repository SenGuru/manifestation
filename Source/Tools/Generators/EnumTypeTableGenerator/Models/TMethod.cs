/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
namespace CodeGenerator.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class TMethod
    {
        public MethodCallType CallType { get; set; } = MethodCallType.None;
        public string MethodName { get; set; } = "";
        public string ApiMethodName { get { if (MethodName == null) return MethodName; if (!MethodName.EndsWith("Async")) return MethodName; return MethodName.Substring(0, MethodName.Length - 5); } }

        public List<InputParam> InputParameters { get; set; } = new List<InputParam>();
        public string OutputParameterType { get; set; } = "";
        public string GetOutputParameterTypeWithModuleNamePrefix(bool isTS, bool isAngular = true)
        {
            var item = CodeGenUtils.ModelPrefixedClassName(isTS, OutputParameterType) ?? "";
            if (isTS)
            {
                if (item.Trim() == "")
                    return isAngular ? "Observable" : "Promise";
                else
                    return (isAngular ? "Observable<" : "Promise<") + item + ">";
            }
            else
            {
                if (item.Trim() == "")
                    return "void";
                else
                    return item;
            }
        }

        public bool HasFormFile(Assembly assembly)
        {
            foreach (var ipaaram in InputParameters)
            {
                if (ipaaram.InputType != InputTypes.Body)
                    continue;

                var inputParamType = assembly.GetType(ipaaram.TypeFullName);

                if (inputParamType == null)
                    inputParamType = Type.GetType(ipaaram.TypeFullName);

                if (inputParamType == null)
                    inputParamType = ipaaram.Type;

                if (inputParamType == null)
                    continue;

                foreach (var prop in inputParamType.GetProperties())
                {
                    if (prop.PropertyType.Name == "IFormFile")
                        return true;
                }
            }
            return false;
        }

        public bool CanIgnore { get; set; }

        public bool IsMvcGenericResponse { get; set; }
    }
}
