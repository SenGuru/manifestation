using System;
using Framework.Infrastructure.Attributes;

namespace Framework.Infrastructure.Utils
{
    public static class EnumUtils
    {
        public static T GeAttributeForEnum<T>(this Enum value)
            where T : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var enumFieldInfo = enumType.GetField(name);
            var descriptionAttributes = enumFieldInfo.GetCustomAttributes(typeof(T), false);

            if (descriptionAttributes.Length > 0)
            {
                return descriptionAttributes[0] as T;
            }

            return null;
        }

        public static ParentKeyAttribute GetParentKeyAttribute(this Enum value)
        {
            return GeAttributeForEnum<ParentKeyAttribute>(value);
        }

        public static PositionAttribute GetPositionAttribute(this Enum value)
        {
            return GeAttributeForEnum<PositionAttribute>(value);
        }

        public static TagAttribute GetTagAttribute(this Enum value)
        {
            return GeAttributeForEnum<TagAttribute>(value);
        }
    }
}
