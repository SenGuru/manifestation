/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
namespace CodeGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new System.Exception("No Parameter sent");
            if (args[0] == "crud")
            {
                CrudGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "migration")
            {
                MigrationProgram.MainApp();
            }
            else if (args[0] == "new-function")
            {
                NewFunctionAppGenProgram.MainApp(args);
            }
            else if (args[0] == "react" || args[0] == "angular")
            {
                if (args.Length != 2)
                {
                    throw new System.Exception("You need to pass 2 parameters - <angular|react> <WebFolder>");
                }
                
                AngularModelGeneratorProgram.MainApp(args[0] == "angular", args[1]);
            }
            else if (args[0] == "flutter")
            {
                if (args.Length != 2)
                {
                    throw new System.Exception("You need to pass 2 parameters - <flutter> <WebFolder>");
                }

                FlutterModelGeneratorProgram.MainApp(args[1]);
            }            
            else if (args[0] == "testapi")
            {
                ApiModelGeneratorProgram.MainApp(isTest: true);
            }
            else if (args[0] == "wordapi")
            {
                ApiModelGeneratorProgram.MainApp(isTest: false);
            }
            else if (args[0] == "analystics")
            {
                AnalyticsGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "analystics-batch-test")
            {
                AnalyticsGeneratorProgram.BatchTest();
            }
            else if (args[0] == "ui")
            {
                UIGeneratorProgram.MainApp(args, false);
            }
            else if (args[0] == "fui")
            {
                FlutterUIGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "cred-list")
            {
                UIGeneratorProgram.MainApp(args, true);
            }
            else if (args[0] == "cred-ui")
            {
                CredUIGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "ui-enum-pill")
            {
                UIEnumPillGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "ui-input-block")
            {
                UIHtmlInputBlockGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "db-fkey")
            {
                FkeyGeneratorProgram.MainApp(args);
            }
            else if (args[0] == "gen-mappers")
            {
                MapperGeneratorForAllClassesProgram.MainApp();
            }            
            else if (args[0] == "all-db-fkeys")
            {
                FkeyGeneratorForAllClassesProgram.MainApp();
            }
            else if (args[0] == "selected-db-fkeys")
            {
                if (args.Length != 2)
                {
                    throw new System.Exception("You need to pass 2 parameters");
                }

                FkeyGeneratorForAllClassesProgram.MainApp(true, args[1]);
            }
            else if (args[0] == "generate-entity-can-delete-function")
            {
                EntityCanDeleteFunctionGenerator.MainApp(args);
            }
            else if (args[0] == "new-column")
            {
                NewTableColumnProgram.MainApp(args);
            }
            else if (args[0] == "new-function")
            {
                NewFunctionGenProgram.MainApp(args);
            }
            else if (args[0] == "region-extractor")
            {
                RegionExtractorProgram.MainApp(args);
            }            
            else
            {
                throw new System.Exception("No Matching Parameter sent");
            }
        }
    }
}
