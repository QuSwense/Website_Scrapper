using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebCommon.Extn
{
    /// <summary>
    /// A class to define extension methods on class <see cref="Type"/>
    /// In .NEt 4.0 many extension methods are not supported that is defined in .NET 4.5 +
    /// </summary>
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Get the custom attribute by the type T for class <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        /// <summary>
        /// Get the custom attribute by the type T for class <see cref="PropertyInfo"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this PropertyInfo prop)
        {
            return (T)prop.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        /// <summary>
        /// This extension method checks if there are any Custom attributes presetn foir a Property type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool HasCustomAttributes<T>(this PropertyInfo prop)
            where T: Attribute
        {
            List<T> attrAttributes = prop.GetCustomAttributes<T>().ToList();
            return attrAttributes != null && attrAttributes.Count > 0;
        }

        /// <summary>
        /// Set the value to the object property for class <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="valueobj"></param>
        /// <param name="value"></param>
        public static void SetValue(this PropertyInfo prop, object valueobj, object value)
        {
            prop.SetValue(valueobj, value, null);
        }

        /// <summary>
        /// Check if the type is a generic list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericListType(this Type type)
        {
            return type.IsGenericType &&
                    (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        /// Convert method to convert from one type to another for class <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ChangeType(this Type type, object value)
        {
            string valueString = string.Empty;
            if (value != null) valueString = value.ToString();
            return ChangeType(type, valueString);
        }

        /// <summary>
        /// Convert method to convert from one type to another for class <see cref="Type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
