/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System.Collections.Generic;

namespace Framework.Infrastructure.Exceptions
{
    public class ReturnErrorItem
    {
        public ReturnErrorItem()
        {
        }

        public ReturnErrorItem(string key, string value, List<string> tags = null)
        {
            Key = key;
            Value = value;

            if (tags == null)
            {
                Tags = new List<string>();
            }
            else
            {
                Tags = tags;
            }
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public List<string> Tags { get; set; }

        public static List<ReturnErrorItem> AsReturnErrorList(string key, string value, List<string> tags = null)
        {
            return new List<ReturnErrorItem>
            {
                new ReturnErrorItem(key, value, tags)
            };
        }
    }
}
