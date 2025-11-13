/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System.IO;
using System.Xml.Serialization;

namespace Framework.Infrastructure.Utils
{
    public static class XmlUtils
    {
        public static string SerializeObject<T>(T dataObject)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, dataObject);
                return stringWriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string xml)
            where T : new()
        {
            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
