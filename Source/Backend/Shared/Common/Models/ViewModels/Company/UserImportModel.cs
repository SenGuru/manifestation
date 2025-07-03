#pragma warning disable S2094 // Classes should not be empty
/*
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see https://opensource.org/licenses/BSD-3-Clause.
*/
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    public class UserImportModel : UserCreateModel
    {
        public List<UserCreateModel> Users { get; set; }
    }
}
#pragma warning restore S2094 // Classes should not be empty