using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Infrastructure.Helpers
{
    public class IgnoreIdFieldsConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Implement this if deserialization is needed
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var property in typeof(T).GetProperties())
            {
                if (!property.Name.EndsWith("Id"))
                {
                    writer.WritePropertyName(property.Name);
                    JsonSerializer.Serialize(writer, property.GetValue(value), options);
                }
            }

            writer.WriteEndObject();
        }
    }
}