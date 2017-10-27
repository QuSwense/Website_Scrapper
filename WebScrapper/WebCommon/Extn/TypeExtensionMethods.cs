using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebCommon.Extn
{
    public static class TypeExtensionMethods
    {
        public static T GetCustomAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        public static T GetCustomAttribute<T>(this PropertyInfo prop)
        {
            return (T)prop.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        public static void SetValue(this PropertyInfo prop, object valueobj, object value)
        {
            prop.SetValue(valueobj, value, null);
        }

        public static object ChangeType(this Type type, string value)
        {
            object result = null;
            if (type == typeof(bool))
            {
                if (string.IsNullOrEmpty(value))
                {
                    result = false;
                }
                else
                {
                    double d;
                    string s = value.ToString().Trim();
                    // t/f
                    // true/false
                    // y/n
                    // yes/no
                    // <>0/0
                    if (string.Compare("False", s, true) == 0 || string.Compare("No", s, true) == 0)
                    {
                        result = false;
                    }
                    else if (double.TryParse(s, out d) && d == 0) // numeric zero
                    {
                        result = false;
                    }
                    else if (string.Compare("True", s, true) == 0 || string.Compare("Yes", s, true) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            else if (type.IsEnum)
            {
                result = Enum.Parse(type, value.ToString(), true);
            }
            else if (type == typeof(int))
            {
                if (string.IsNullOrEmpty(value.ToString())) result = 0;
                else result = Convert.ChangeType(value, type);
            }
            else if (type == typeof(double))
            {
                if (string.IsNullOrEmpty(value.ToString())) result = 0.0;
                else result = Convert.ChangeType(value, type);
            }
            else
            {
                result = Convert.ChangeType(value, type);
            }

            return result;
        }
    }
}
