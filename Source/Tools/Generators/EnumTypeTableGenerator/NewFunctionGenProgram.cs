using System;
using System.Text;
using Framework.Infrastructure.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeGenerator
{
    public static class NewFunctionGenProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 6)
            {
                Console.WriteLine("Parameter should be <http_method> <return_type> <item_or_list> <function_name> <parameter>");
                return;
            }

            var httpMethod = args[1].ToLower().MakeFirstCharAsUpper();
            var returnType = args[2];
            var itemOrList = args[3];
            var functionName = args[4];
            var parameterType = args[5];
            var returnModel = itemOrList.IsSameWithCaseIgnoreTrimmed("item") ? "ReturnModel" : "ReturnListModel";            
            var bindType = httpMethod.IsSameWithCaseIgnoreTrimmed("GET") ? "FromQuery" : "FromBody";
            var dbReturnModel = itemOrList.IsSameWithCaseIgnoreTrimmed("item") ? returnType : "List<"+ returnType + ">";

            //Things we need to display
            Console.WriteLine($"Controller function");
            Console.WriteLine($"/////////////");
            Console.WriteLine($"[Http{httpMethod}]");
            Console.WriteLine($"[SwaggerOperation(Tags = new[] {{ \"ToTest\" }})]");
            Console.WriteLine($"public Task<{returnModel}<{returnType}>> {functionName}([{bindType}] {parameterType} model)");
            Console.WriteLine($"{{");
            Console.WriteLine($"    return {functionName}Async(model);");
            Console.WriteLine($"}}");


            Console.WriteLine($"Service function");
            Console.WriteLine($"/////////////");
            Console.WriteLine($"public Task<{returnModel}<{returnType}>> {functionName}Async({parameterType} model)");
            Console.WriteLine($"{{");
            Console.WriteLine(GetServiceFunction(httpMethod, returnType, itemOrList, functionName, parameterType, returnModel, bindType));
            Console.WriteLine($"}}");

            Console.WriteLine($"Repo function");
            Console.WriteLine($"/////////////");
            Console.WriteLine($"public Task<{dbReturnModel}> {functionName}Async({parameterType} model)");
            Console.WriteLine($"{{");
            Console.WriteLine(GetRepoFunction(httpMethod, returnType, itemOrList, functionName, parameterType, returnModel, bindType));
            Console.WriteLine($"}}");
        }

        static string GetServiceFunction(string httpMethod, string returnType, string itemOrList, string functionName, string parameterType, string returnModel, string bindType)
        {
            var sb = new StringBuilder();
            Console.WriteLine($"          try");
            Console.WriteLine($"          {{");
            Console.WriteLine($"              var movie = await companyRepository.{functionName}Async(model);");
            Console.WriteLine($"              return ReturnModel<bool>.Success(true);");
            Console.WriteLine($"          }}");
            Console.WriteLine($"          catch (Exception ex)");
            Console.WriteLine($"          {{");
            Console.WriteLine($"              return log.ErrorAndReturnModel<bool>($\"Unexpected error when changing the Approval Status\", ex);");
            Console.WriteLine($"          }}");
            
            return sb.ToString();
        }

        static string GetRepoFunction(string httpMethod, string returnType, string itemOrList, string functionName, string parameterType, string returnModel, string bindType)
        {
            var sb = new StringBuilder();
            if (httpMethod == "POST")
            Console.WriteLine($"          return await appDBManager.GetTable<{returnType}>().UpdateAsync(model) > 0;");

            if (httpMethod == "PUT")
                Console.WriteLine($"          return appDBManager.GetTable<{returnType}>().InsertAsync(model);");

            if (httpMethod == "GET")
                Console.WriteLine($"          return appDBManager.GetTable<{returnType}>().Where(x => x.XYZ).ToListAsync();");

            return sb.ToString();
        }
    }
}