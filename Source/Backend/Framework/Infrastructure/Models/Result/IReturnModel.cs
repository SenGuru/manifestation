/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Exceptions;

namespace Framework.Infrastructure.Models.Result
{
    public interface IReturnModel
    {
        string ActivityId { get; set; }

        DateTime ActivityDateTime { get; set; }

        string Server { get; set; }

        int ProcessId { get; set; }

        [JsonIgnore]
        int HttpCode { get; set; }

        [JsonIgnore]
        bool AlreadyWrittenToLog { get; }

        bool IsSuccess { get; set; }

        string SuccessMessage { get; set; }

        ReturnError ErrorHolder { get; set; }
    }
}
