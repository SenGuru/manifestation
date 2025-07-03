using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Infrastructure.Helpers
{
    public class IgnoreFieldsAndIdFieldsConverter<T> : JsonConverter<T>
    {
        private readonly HashSet<string> _ignoredFields;

        public IgnoreFieldsAndIdFieldsConverter(IEnumerable<string> ignoredFields)
        {
            _ignoredFields = new HashSet<string>(ignoredFields, StringComparer.OrdinalIgnoreCase);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Implement this if deserialization is needed
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            WriteObject(writer, value, options);
        }

        private void WriteObject(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value is IEnumerable enumerable && !(value is string))
            {
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    if (item != null)
                    {
                        WriteObjectProperties(writer, item, options);
                    }
                    else
                    {
                        writer.WriteNullValue();
                    }
                }

                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartObject();
                WriteObjectProperties(writer, value, options);
                writer.WriteEndObject();
            }
        }

        private void WriteObjectProperties(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            foreach (var property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Skip ignored fields and fields ending with "Id"
                if (_ignoredFields.Contains(property.Name) || property.Name.EndsWith("Id"))
                {
                    continue;
                }

                var propertyValue = property.GetValue(value);

                if (propertyValue == null)
                {
                    writer.WriteNull(property.Name);
                    continue;
                }

                // Write property name
                writer.WritePropertyName(property.Name);

                if (propertyValue is IEnumerable enumerable && !(propertyValue is string))
                {
                    // Write array of objects
                    writer.WriteStartArray();
                    foreach (var item in enumerable)
                    {
                        WriteObject(writer, item, options);
                    }

                    writer.WriteEndArray();
                }
                else
                {
                    // Write single object or value
                    JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                }
            }
        }
    }
}
