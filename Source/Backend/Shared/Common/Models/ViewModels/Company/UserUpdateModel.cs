#pragma warning disable CS0108,SA1502,S4015,S1144
/*
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see https://opensource.org/licenses/BSD-3-Clause.
*/
using System;

namespace Manifest.Common.Models.ViewModels
{
    public class UserUpdateModel : UserModel
    {
        private new string PredefinedUserTypeName { get; set; }

        private new DateTime CreatedDate { get; set; }

        private new long? CreatedByUserId { get; set; }

        private new DateTime ModifiedDate { get; set; }

        private new long? ModifiedByUserId { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015, S1144