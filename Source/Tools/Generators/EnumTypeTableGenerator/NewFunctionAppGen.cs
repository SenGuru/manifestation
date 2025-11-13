using System;
using System.IO;
using Framework.Infrastructure.Utils;

namespace CodeGenerator
{
    public static class NewFunctionAppGenProgram
    {
        public static void MainApp(string[] args)
        {
            var str = Directory.GetCurrentDirectory() + "\\Snippets\\";

            if (args.Length != 4)
            {
                PrintUsage(str);
                return;
            }

            var tableName = args[2];
            var colName = args[3];
            var colNameWithoutId = colName.Replace("Id", ""); ;
            var smallTableName = tableName.MakeFirstCharAsLower();
            var smallColName = colName.MakeFirstCharAsLower();
            var smallColNameWithOutId = smallColName.Replace("Id","");
            var fncType = args[1];


            //Things we need to display
            Console.WriteLine($"Generating Code for {tableName} with Column - {colName}");
            Console.WriteLine($"///////////////////////////////////////");

            var rmeplate = str + fncType + ".template";

            if (!File.Exists(rmeplate))
            {
                PrintUsage(str);
                throw new Exception($"{rmeplate} does not exists");
            }

            var txt = File.ReadAllText(rmeplate);
            txt = txt.Replace("<Table>", tableName);
            txt = txt.Replace("<SmallTable>", smallTableName);

            txt = txt.Replace("<Column>", colName);
            txt = txt.Replace("<ColumnWithoutId>", colNameWithoutId);            
            txt = txt.Replace("<SmallColumn>", smallColName);
            txt = txt.Replace("<SmallColumnWithoutId>", smallColNameWithOutId);

            Console.WriteLine(txt);

            static void PrintUsage(string str)
            {
                Console.WriteLine("Parameter should be <Type ><TableName> <ColumnName>");
                Console.WriteLine("     Available Types are :");
                foreach (var f in Directory.GetFiles(str))
                {
                    Console.WriteLine("         " + Path.GetFileNameWithoutExtension(f));
                }
            }
        }
    }
}