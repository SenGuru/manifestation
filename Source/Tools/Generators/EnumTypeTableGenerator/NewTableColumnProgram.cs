using System;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class NewTableColumnProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Parameter should be <TableName> <ColumnName>");
                return;
            }

            var tableName = args[1];
            var colName = args[2];
            //Things we need to display
            Console.WriteLine($"Generating Code for {tableName} with Column - {colName}");
            Console.WriteLine($"//Mig_001");
            Console.WriteLine($"///////////////////////////////////////");

            var migTypeName = ".AsString(100)";
            var fkTable = "";
            var isFkTable = false;
            var isEnumFkTable = false;
            var colWithOutId = colName.Replace("Id", "");
            var smallColWithOutId = colWithOutId.MakeFirstCharAsLower();
            var smallFirstTableName = tableName.MakeFirstCharAsLower();
            var fkColName = colName;

            if (colName.EndsWith("Id") || colName.Equals("Position", StringComparison.CurrentCultureIgnoreCase) || colName.EndsWith("Position") || colName.EndsWith("Size") || colName.Equals("Size", StringComparison.CurrentCultureIgnoreCase))
            {
                if (colName.Contains("Predefined"))
                {
                    fkTable = "EnumType";
                    isEnumFkTable = true;
                    fkColName = "EnumTypeId";
                }
                else
                {
                    fkTable = colName.Replace("Id", "");
                    isEnumFkTable = false;
                }

                isFkTable = true;

                migTypeName = ".AsInt64()";
            }
            else if (colName.EndsWith("Guid"))
            {
                migTypeName = ".AsGuid()";
            }

            Console.WriteLine($".WithColumn(\"{colName}\"){migTypeName}.Nullable()");
            Console.WriteLine($"");
            Console.WriteLine($"");
            Console.WriteLine($"Create.ForeignKey(\"FK_{tableName}_{colName}\").FromTable(\"{tableName}\").ForeignColumns(\"{colName}\").ToTable(\"{fkTable}\").PrimaryColumns(\"{fkColName}\");");
            Console.WriteLine($"Create.Index(\"IX_{tableName}_{colName}\").OnTable(\"{tableName}\").OnColumn(\"{colName}\");");
            Console.WriteLine($"///////////////////////////////////////");

            Console.WriteLine($"//Repository");
            Console.WriteLine($"///////////////////////////////////////");
            Console.WriteLine($"//IQueruable Function");
            if (isFkTable)
            {
                Console.WriteLine($"{colWithOutId}Name = xu.{fkTable}Name,");
            }

            Console.WriteLine($"{colName} = item.{colName},");
            Console.WriteLine($"");
            Console.WriteLine($"join oxu in appDBManager.GetTable<{fkTable}>() on item.{colName} equals oxu.{fkColName} into xuTemp");
            Console.WriteLine($"from xu in xuTemp.DefaultIfEmpty()");
            if (isFkTable)
            {
                Console.WriteLine($"///////////////////////////////////////");
                Console.WriteLine($"//Search Functions");
                Console.WriteLine($"                if (search.{colName}s != null && search.{colName}s.Count > 0)");
                Console.WriteLine($"                    sql1 = sql1.Where(x => search.{colName}s.Contains(x.{colName}));");
                Console.WriteLine($"");
            }

            Console.WriteLine($"///////////////////////////////////////");

            Console.WriteLine($"//Service");
            Console.WriteLine($"///////////////////////////////////////");
            Console.WriteLine($"//Create/ Update Function");

            Console.WriteLine($"//Check Function");

            if (isFkTable)
            {
                Console.WriteLine($"            if ({smallFirstTableName}Model.{colName} != null)");
                Console.WriteLine($"            {{");

                if (isEnumFkTable)
                {
                    Console.WriteLine($"            if (!ValidationUtils.IsEnumValid<{colWithOutId}s, long>({smallFirstTableName}Model.{colName}))");
                    Console.WriteLine($"            {{");
                    Console.WriteLine($"                result.Add(new ReturnErrorItem(\"{colName}\", \"{colName} is not valid\"));");
                    Console.WriteLine($"            }}");
                }
                else
                {
                    Console.WriteLine($"            var {smallColWithOutId}Item = await Get{colWithOutId}ByIdForReadAsync({smallFirstTableName}Model.{colName});");
                    Console.WriteLine($"            if (!{smallColWithOutId}Item.IsSuccess)");
                    Console.WriteLine($"            {{");
                    Console.WriteLine($"                if ({smallColWithOutId}Item.ErrorHolder != null)");
                    Console.WriteLine($"                    result.Add(new ReturnErrorItem(\"\", {smallColWithOutId}Item.ErrorHolder.FriendlyMessage));");
                    Console.WriteLine($"                else");
                    Console.WriteLine($"                    result.Add(new ReturnErrorItem(\"\", \"Unable to Get the {colName} details\"));");
                    Console.WriteLine($"                return result;");
                    Console.WriteLine($"            }}");
                    Console.WriteLine($"");
                    Console.WriteLine($"            if ({smallColWithOutId}Item.Model.CompanyId != {smallFirstTableName}Model.CompanyId)");
                    Console.WriteLine($"            {{");
                    Console.WriteLine($"                result.Add(new ReturnErrorItem(\"\", $\"You dont have access to this {colName} Information\"));");
                    Console.WriteLine($"                return result;");
                    Console.WriteLine($"            }}");
                }
                Console.WriteLine($"            }}");
                Console.WriteLine($"");
            }
            else
            {
                Console.WriteLine($"            if ({smallFirstTableName}Model.{colName} != null && {smallFirstTableName}Model.{colName}.Length > {tableName}Model.COLUMN_SIZE_{colName})");
                Console.WriteLine($"            {{");
                Console.WriteLine($"                result.Add(new ReturnErrorItem(\"{colName}\", $\"{colName} size should not exceed {{{tableName}Model.COLUMN_SIZE_{colName}}}\"));");
                Console.WriteLine($"            }}");
                Console.WriteLine($"");
                Console.WriteLine($"            if ({smallFirstTableName}Model.{colName} == null)");
                Console.WriteLine($"            {{");
                Console.WriteLine($"                result.Add(new ReturnErrorItem(\"{colName}\", \"{colName} is required \"));");
                Console.WriteLine($"            }}");
            }
        }
    }
}