/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using Framework.Utilities.PocoGenerator.Utilities;

namespace Framework.Utilities.PocoGenerator
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.IsParamValueAvailable("--poco-generator"))
            {
                return DBGeneratorProgram.MainApp(args);
            }
            else if (args.IsParamValueAvailable("--mig-from-flat-file-generator"))
            {
                return MigFromFlatFileGeneratorProgram.MainApp(args);
            }
            else
            {
                throw new Exception("Poco or Flat File Generator option is not provided");
            }
        }
    }
}
