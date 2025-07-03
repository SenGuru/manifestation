using System.Text;
using System;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class EntityCanDeleteFunctionGenerator
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine(args[1]);
                Console.WriteLine("Parameter should be <EntityName> <TableNames>");
                return;
            }

            string entityName = args[1];
            string[] tableNames = args[2].Split(':');

            Console.WriteLine(entityName);
            Console.WriteLine(tableNames);
            Console.WriteLine(GenerateDeleteFunctions(entityName, tableNames));
        }

        private static string GenerateDeleteFunctions(string entityName, string[] tableNames)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GenerateCanDeleteFunction(entityName, tableNames));
            sb.AppendLine(GenerateIsItemExistsInOtherTableFunction(entityName, tableNames));
            sb.AppendLine(GenerateCanDeleteSnippet(entityName));

            return sb.ToString();
        }

        private static string GenerateCanDeleteFunction(string entityName, string[] tableNames)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"public async Task<ReturnModel<CanDeleteModel>> CanDelete{entityName}ItemAsync(long {entityName.MakeFirstCharAsLower()}Id)");
            sb.AppendLine("{");
            sb.AppendLine("    try");
            sb.AppendLine("    {");
            sb.AppendLine($"        var existing{entityName}Model = await Get{entityName}ByIdForWriteAsync({entityName.MakeFirstCharAsLower()}Id);");
            sb.AppendLine($"        if (!existing{entityName}Model.IsSuccess)");
            sb.AppendLine("                {");
            sb.AppendLine($"                    return log.ErrorAndReturnModel<CanDeleteModel>(existing{entityName}Model);");
            sb.AppendLine("                }");
            sb.AppendLine();

            sb.AppendLine("        var toCheckTables = new List<PredefinedResourceTypes>");
            sb.AppendLine("                {");

            foreach (var tableName in tableNames)
            {
                sb.AppendLine($"                    PredefinedResourceTypes.{tableName},");
            }

            sb.AppendLine("                };");
            sb.AppendLine();

            sb.AppendLine("        var canDeleteModel = new CanDeleteModel");
            sb.AppendLine("        {");
            sb.AppendLine("            CanDelete = true,");
            sb.AppendLine("            ReferencingTables = new List<ReturnErrorItem>()");
            sb.AppendLine("        };");
            sb.AppendLine();

            sb.AppendLine("        foreach (var table in toCheckTables)");
            sb.AppendLine("        {");
            sb.AppendLine($"            bool result = await Is{entityName}ItemExistsInOtherTableAsync(table, {entityName.MakeFirstCharAsLower()}Id);");
            sb.AppendLine("            if (result)");
            sb.AppendLine("            {");
            sb.AppendLine("                canDeleteModel.ReferencingTables.Add(new ReturnErrorItem { Value = table.ToString().SplitCamelCase() });");
            sb.AppendLine("                canDeleteModel.CanDelete = false;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        return new ReturnModel<CanDeleteModel>(canDeleteModel);");
            sb.AppendLine("    }");
            sb.AppendLine("    catch (Exception ex)");
            sb.AppendLine("    {");
            sb.AppendLine($"        return log.ErrorAndReturnModel<CanDeleteModel>(\"Unexpected error when marking {entityName} as not deleted\", new {{ {entityName.MakeFirstCharAsLower()}Id }}, ex);");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GenerateIsItemExistsInOtherTableFunction(string entityName, string[] tableNames)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"public async Task<bool> Is{entityName}ItemExistsInOtherTableAsync(PredefinedResourceTypes otherTableName, long {entityName.MakeFirstCharAsLower()}Id)");
            sb.AppendLine("{");
            sb.AppendLine("    switch (otherTableName)");
            sb.AppendLine("    {");

            foreach (var tableName in tableNames)
            {
                sb.AppendLine($"        case PredefinedResourceTypes.{tableName}:");
                var repo = tableName.StartsWith("Movie", StringComparison.OrdinalIgnoreCase) ? "movieRepository" : "companyRepository";
                var criteria = $"{tableName}SearchCriteria";

                sb.AppendLine($"            var {tableName.MakeFirstCharAsLower()}s = await {repo}.Get{tableName}sAsync(new {criteria}");
                sb.AppendLine("            {");
                sb.AppendLine($"                {entityName}Ids = new List<long> {{ {entityName.MakeFirstCharAsLower()}Id }},");
                sb.AppendLine($"                {entityName}Id = {entityName.MakeFirstCharAsLower()}Id");
                sb.AppendLine("            });");
                sb.AppendLine("");
                sb.AppendLine($"            return {tableName.MakeFirstCharAsLower()}s.Result.IsNotNullOrEmpty();");
                sb.AppendLine("");
            }

            sb.AppendLine("        default:");
            sb.AppendLine("            return false;");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GenerateCanDeleteSnippet(string entityName)
        {
            return @$"
var canDelete = await CanDelete{entityName}ItemAsync({entityName.MakeFirstCharAsLower()}Id);
if (!canDelete.IsSuccess)
{{
    return log.ErrorAndReturnModel<bool>(canDelete);
}}

if (!canDelete.Model.CanDelete)
{{
    return log.ErrorAndReturnModel<bool>($""This {entityName.SplitCamelCase()} is being used in other places, so you have to delete them first in order to delete this record"", new {{ {entityName.MakeFirstCharAsLower()}Id }}, errorList: canDelete.Model.ReferencingTables);
}}";
        }
    }
}