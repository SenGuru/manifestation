using System;
using System.Reflection;

public static class ConfigUtils
{
    public static bool IsParamAvailable(this string[] args, string paramName)
    {
        if (args == null)
        {
            return false;
        }

        for (var i = 0; i < args.Length; ++i)
        {
            var argItem = args[i].ToLower().Trim();
            paramName = paramName.ToLower().Trim();

            if (argItem == paramName || argItem == "/" + paramName || argItem == "\\" + paramName
                || argItem == "--" + paramName || argItem == "-" + paramName)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsParamValueAvailable(this string[] args, string paramName)
    {
        if (args == null)
        {
            return false;
        }

        for (var i = 0; i < args.Length; ++i)
        {
            var argItem = args[i].ToLower().Trim();
            paramName = paramName.ToLower().Trim();

            if (argItem == paramName || argItem == "/" + paramName || argItem == "\\" + paramName
                || argItem == "--" + paramName || argItem == "-" + paramName)
            {
                if (i + 1 <= args.Length)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static T GetEnvironmentVariableOrDefault<T>(string[] args, string environmentVar, T defaultValue)
	{
		var resultStr = args.GetParamValueAsString(environmentVar);
		if (resultStr == null || resultStr == "")
		{
			resultStr = Environment.GetEnvironmentVariable(environmentVar);
			if (resultStr == null || resultStr == "")
			{
				return GetValueFromString<T>(resultStr, defaultValue);
			}
		}

		return GetValueFromString<T>(resultStr, defaultValue);
	}

	public static string GetParamValueAsString(this string[] args, string paramName, string defaultValue = "")
	{
		if (args == null)
		{
			return defaultValue;
		}

		for (var i = 0; i < args.Length; ++i)
		{
			var argItem = args[i].ToLower().Trim();
			paramName = paramName.ToLower().Trim();

			if (argItem == paramName || argItem == "/" + paramName || argItem == "\\" + paramName
				|| argItem == "--" + paramName || argItem == "-" + paramName)
			{
				if (i + 1 < args.Length)
				{
					var val = args[i + 1];
					return val;
				}
			}
		}

		return defaultValue;
	}

	public static T GetValueFromString<T>(string value, T defaultValue)
	{
		if (value == null)
		{
			return defaultValue;
		}

		var ptype = typeof(T);

		if (ptype == typeof(string))
		{
			return (T)Convert.ChangeType(value, typeof(T));
		}
		else if (ptype == typeof(short))
		{
			return (T)Convert.ChangeType(SafeUtils.Short(value), typeof(T));
		}
		else if (ptype == typeof(ushort))
		{
			return (T)Convert.ChangeType(SafeUtils.UShort(value), typeof(T));
		}
		else if (ptype == typeof(int))
		{
			return (T)Convert.ChangeType(SafeUtils.Int(value), typeof(T));
		}
		else if (ptype == typeof(long))
		{
			return (T)Convert.ChangeType(SafeUtils.Long(value), typeof(T));
		}
		else if (ptype == typeof(float))
		{
			return (T)Convert.ChangeType(SafeUtils.Float(value), typeof(T));
		}
		else if (ptype == typeof(double))
		{
			return (T)Convert.ChangeType(SafeUtils.Double(value), typeof(T));
		}
		else if (ptype == typeof(bool))
		{
			return (T)Convert.ChangeType(SafeUtils.Bool(value), typeof(T));
		}
		else if (ptype == typeof(Guid))
		{
			return (T)Convert.ChangeType(SafeUtils.Guid(value), typeof(T));
		}
		else if (ptype == typeof(Enum))
		{
			return (T)Convert.ChangeType(value, typeof(T));
		}
		else if (ptype.GetTypeInfo().BaseType == typeof(Enum))
		{
			var propType = ptype;
			var safeValue = SafeUtils.Enum(propType, value, null);
			if (safeValue != null)
			{
				return (T)Convert.ChangeType(safeValue, typeof(T));
			}
		}

		return defaultValue;
	}
}
