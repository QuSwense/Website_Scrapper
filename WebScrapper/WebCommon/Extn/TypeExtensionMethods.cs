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
    }
}
