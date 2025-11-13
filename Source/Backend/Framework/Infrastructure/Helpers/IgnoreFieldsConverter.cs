using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Infrastructure.Helpers
{
    public class IgnoreFieldsConverter<T> : JsonConverter<T>
    {
        private readonly HashSet<string> _ignoredFields;

        public IgnoreFieldsConverter(IEnumerable<string> ignoredFields)
        {
            _ignoredFields = new HashSet<string>(ignoredFields, StringComparer.OrdinalIgnoreCase);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Implement this if deserialization is needed
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var property in typeof(T).GetProperties())
            {
                if (!_ignoredFields.Contains(property.Name))
                {
                    writer.WritePropertyName(property.Name);
                    JsonSerializer.Serialize(writer, property.GetValue(value), options);
                }
            }

            writer.WriteEndObject();
        }
    }
}