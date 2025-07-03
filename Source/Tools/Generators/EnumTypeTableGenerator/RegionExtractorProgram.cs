/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.IO;
using RazorLight;
using Framework.Infrastructure.Utils;
using EnumTypeTableGenerator.Models;
using System.Linq;

namespace CodeGenerator
{
    public static class RegionExtractorProgram
    {
        public static void MainApp(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide file paths as arguments.");
                return;
            }

            var argFiles = args[0].Split(";,:".ToArray());

            string outputPath = "Output.cs";
            using (StreamWriter outputFile = new StreamWriter(outputPath, true))
            {
                foreach (string filePath in argFiles)
                {
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"File not found: {filePath}");
                    }

                    string[] lines = File.ReadAllLines(filePath);
                    bool inRegion = false;
                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("#region"))
                        {
                            inRegion = true;
                            continue;
                        }
                            
                        if (inRegion)
                        {
                            outputFile.WriteLine(line);
                        }

                        if (line.Trim().StartsWith("#endregion"))
                        {
                            inRegion = false;
                            continue;
                        }

                    }
                    
                }
            }

            Console.WriteLine($"Contents have been appended to {outputPath}");
        }
    }
}
