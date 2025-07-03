/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using MessagePack;

namespace Framework.Infrastructure.Utils
{
    public static class SerializationUtils
    {
        public static byte[] ToByteArray<T>(this T obj)
        {
            if (obj == null)
            {
                return (byte[])null;
            }

            return MessagePackSerializer.Serialize<T>(obj, MessagePack.Resolvers.ContractlessStandardResolver.Options);
        }

        public static T FromByteArray<T>(this byte[] byteArray)
            where T : class
        {
            if (byteArray == null)
            {
                return default(T);
            }

            return MessagePackSerializer.Deserialize<T>(byteArray, MessagePack.Resolvers.ContractlessStandardResolver.Options);
        }
    }
}
