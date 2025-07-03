/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;

namespace Framework.Infrastructure.Utils
{
    public static class PagingUtils
    {
        public static long GetPageCount(long totalRowCount, long pageSize)
        {
            long pageCount = totalRowCount % pageSize == 0 ? totalRowCount / pageSize : (totalRowCount / pageSize) + 1;
            return Math.Max(pageCount, 1);
        }
    }
}
