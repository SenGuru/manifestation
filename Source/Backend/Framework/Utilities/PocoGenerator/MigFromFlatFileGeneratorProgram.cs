/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Utilities.PocoGenerator.Utilities;

namespace Framework.Utilities.PocoGenerator
{
    public static class MigFromFlatFileGeneratorProgram
    {
        public static int MainApp(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return -1;
            }

            if (!(args.IsParamValueAvailable("--flat-file")
                    && args.IsParamValueAvailable("--mig-file-location")))
            {
                PrintUsage();
                return -1;
            }

            var flatFile = args.GetParamValueAsString("--flat-file");
            var migFile = args.GetParamValueAsString("--mig-file-location");
            Generate(flatFile, migFile);

            return 0;
        }

        private static void Generate(string flatFile, string migFile)
        {
            var lines = File.ReadAllLines(flatFile);
            var tableMarkerFound = false;
            var sbTable = new StringBuilder();
            var sbIndx = new StringBuilder();
            var sbEnum = new StringBuilder();
            var sbEnumSum = new StringBuilder();
            var sbCiLine = new StringBuilder();

            int currentLineNumber = 0;

            foreach (var line in lines)
            {
                currentLineNumber++;

                if (line == null)
                    continue;

                if (line.Trim().Equals("Tables", StringComparison.CurrentCultureIgnoreCase))
                {
                    tableMarkerFound = true;
                    continue;
                }

                if (tableMarkerFound)
                {
                    if (line.Trim().Equals("Types", StringComparison.CurrentCultureIgnoreCase))
                        break;

                    if (line.Trim().Equals("", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    var r = PrintTableFluentMigratorAndReturnIndexFKeyItems(line, currentLineNumber);
                    sbTable.AppendLine(r.tbl);
                    sbIndx.AppendLine(r.idx);
                }
            }

            int runningNumber = 34;
            var typeMarkerFound = false;
            currentLineNumber = 0;

            foreach (var line in lines)
            {
                currentLineNumber++;

                if (line == null)
                    continue;

                if (line.Trim().Equals("Types", StringComparison.CurrentCultureIgnoreCase))
                {
                    typeMarkerFound = true;
                    continue;
                }

                if (typeMarkerFound)
                {
                    if (line.Trim().Equals("", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    runningNumber++;
                    var r = PrintEnum(line, runningNumber, currentLineNumber);
                    sbEnum.AppendLine(r.enumBody);
                    sbEnumSum.AppendLine(r.enumNameWithNumber);
                    sbCiLine.AppendLine(r.ciLine);
                }
            }

            File.WriteAllText(migFile, sbTable.ToString());
            File.AppendAllLines(migFile, new string[] { "" });
            File.AppendAllText(migFile, sbIndx.ToString());
            File.AppendAllLines(migFile, new string[] { "" });
            File.AppendAllText(migFile, sbEnum.ToString());
            File.AppendAllText(migFile, "");
            File.AppendAllText(migFile, sbEnumSum.ToString());
            File.AppendAllText(migFile, "");
            File.AppendAllText(migFile, sbCiLine.ToString());

            ///Console.WriteLine(sbTable.ToString());
            ///Console.WriteLine(sbIndx.ToString());
            ///Console.WriteLine("////////////////////////////////////////////");
            ///Console.WriteLine(sbEnum.ToString());
        }

        private static (string tbl, string idx) PrintTableFluentMigratorAndReturnIndexFKeyItems(string tableLine, int actualLinePositionInFile)
        {
            var tblResult = new StringBuilder();
            var idxResult = new StringBuilder();

            var r = GetItemAndChildItems(tableLine, actualLinePositionInFile,isForTables: true);

            tblResult.AppendLine($"Create.Table(\"{r.name}\")");
            tblResult.AppendLine($"    .WithColumn(\"{r.name + "Id"}\").AsInt64().PrimaryKey().Identity()");
            foreach (var px in r.parameters)
            {
                var rName = r.name;
                var pName = px.col;

                if (rName + "Id" == pName)
                    continue;

                var nValue = "NotNullable";
                if (px.isNull)
                {
                    nValue = "Nullable";
                }

                tblResult.AppendLine($"    .WithColumn(\"{pName}\").{GetTypeName(pName)}.{nValue}()");

                var toTable = pName.Replace("Id", "");
                var toColumn = pName;

                if (pName.EndsWith("TypeId"))
                {
                    toTable = "EnumType";
                    toColumn = "EnumTypeId";
                }
                else if (pName.EndsWith("UserId"))
                {
                    toTable = "User";
                    toColumn = "UserId";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("To"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "To".Length);
                    toColumn = toTable + "Id";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("From"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "From".Length);
                    toColumn = toTable + "Id";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("New"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "New".Length);
                    toColumn = toTable + "Id";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("Old"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "Old".Length);
                    toColumn = toTable + "Id";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("CreatedBy"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "CreatedBy".Length);
                    toColumn = toTable + "Id";
                }
                else if (pName.EndsWith("Id") && pName.StartsWith("ModifiedBy"))
                {
                    toTable = pName.Replace("Id", "").Remove(0, "ModifiedBy".Length);
                    toColumn = toTable + "Id";
                }

                if (pName.EndsWith("Id") || pName.EndsWith("Guid"))
                {
                    idxResult.AppendLine($"Create.Index(\"IX_{rName}_{pName}\").OnTable(\"{rName}\").OnColumn(\"{pName}\");");

                    if (pName.EndsWith("Id"))
                    {
                        idxResult.AppendLine($"Create.ForeignKey(\"FK_{rName}_{pName}\").FromTable(\"{rName}\").ForeignColumns(\"{pName}\").ToTable(\"{toTable}\").PrimaryColumns(\"{toColumn}\");");
                    }
                }
            }

            tblResult.AppendLine($";");

            return (tblResult.ToString(), idxResult.ToString());
        }

        private static string GetTypeName(string line)
        {
            if (line.EndsWith("Id"))
                return "AsInt64()";

            if (line.EndsWith("Date") || line.EndsWith("DateTime") || line.EndsWith("Time"))
                return "AsDateTime()";

            if (line.StartsWith("Is") || line.StartsWith("Can") || line.StartsWith("Use") || line.StartsWith("Does"))
                return "AsBoolean()";

            if (line.Contains("Point") || line.Contains("Size") || line.EndsWith("Size") || line.Contains("Position") || line.StartsWith("Position"))
                return "AsInt64()";

            if (line.Contains("Amount"))
                return "AsFloat()";

            if (line.Contains("Guid"))
                return "AsGuid()";

            return "AsString()";
        }

        private static (string enumBody, string enumNameWithNumber, string ciLine) PrintEnum(string enumLine, int lineNum, int actualLinePositionInFile)
        {
            int runningNumber = 0;
            var result = new StringBuilder();
            var r = GetItemAndChildItems(enumLine, actualLinePositionInFile,isForTables: false);
            result.AppendLine($"public enum {r.name}");
            result.AppendLine($"{{");
            foreach (var p in r.parameters)
            {
                runningNumber++;
                result.AppendLine($"    {p.col} = {(lineNum * 100000) + runningNumber},");
            }

            result.AppendLine($"}}");
            return (result.ToString(), r.name + " = " + lineNum + ",", $"call ci crud --table={r.name}");
        }

        private static (string name, List<(string col,bool isNull)> parameters) GetItemAndChildItems(string line, int actualLinePositionInFile, bool isForTables)
        {
            var cis = new List<(string,bool)>();

            var v = line.Split('-');
            if (v.Length == 0)
            {
                throw new Exception($"Line - {actualLinePositionInFile} contains v is null - line = {line}");
            }

            if (v.Length != 2)
            {
                throw new Exception($"Line - {actualLinePositionInFile} contains v.length = {v.Length} - line = {line}");
            }

            var i = v[0];
            var vChilds = v[1].Split(',');

            if (vChilds.Length == 0)
            {
                throw new Exception($"Line - {actualLinePositionInFile} contains vChilds is null or empty - line = {line}");
            }

            foreach (var vc in vChilds)
            {
                var nullableValue = false;

                var colRawValue = vc.Trim();
                var colName = vc.Trim();
                if (colRawValue.Contains(":N") || colRawValue.Contains(":N:"))
                {
                    nullableValue = true;
                    var splits = colRawValue.Split(":");
                    if (splits.Length == 0)
                        throw new Exception("Colon value was in the Column Definition but it is not found in split function");
                    colName = splits[0];
                }

                cis.Add((colName, nullableValue));
            }

            if (isForTables)
            {
                if (!cis.Any(x => x.Item1 == "CreatedDate"))
                    cis.Add(("CreatedDate", false));

                if (!cis.Any(x => x.Item1 == "CreatedByUserId"))
                    cis.Add(("CreatedByUserId", true));

                if (!cis.Any(x => x.Item1 == "ModifiedDate"))
                    cis.Add(("ModifiedDate", false));

                if (!cis.Any(x => x.Item1 == "ModifiedByUserId"))
                    cis.Add(("ModifiedByUserId", true));
            }

            return (i.Trim(), cis);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\t -new  <config_file.json> : Create new configuration file");
            Console.WriteLine("\t ---flat-file <db-spec.txt>        : Specify the table information from the file ");
            Console.WriteLine("--mig-file-location <Mig_001_cs PATH> : Specify the ");
        }
    }
}
