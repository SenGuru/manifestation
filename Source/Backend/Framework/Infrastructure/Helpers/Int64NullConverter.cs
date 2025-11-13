using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Framework.Infrastructure.Utils;

namespace Framework.Infrastructure.Helpers
{
    public class Int64NullConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }

            try
            {
                return reader.GetInt64();
            }
            catch
            {
                var val = reader.GetString();
                if (!ValidationUtils.IsInteger(val))
                    return 0;
                return Convert.ToInt64(val);
            }
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}