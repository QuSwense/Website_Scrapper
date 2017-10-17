using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebScrapper.Reader.Meta;

namespace WebScrapper.Reader
{
    public class CSVReader
    {
        public static void Read(string filePath, object objStore, int keyIndx = 0)
        {
            Type objStoreType = objStore.GetType();
            if(objStore == null)
                objStore = Activator.CreateInstance(objStoreType);
            
            using (var txtreader = new StreamReader(filePath))
            {
                string line = null;
                while ((line = txtreader.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { ',' });

                    SetValues(objStore, split, keyIndx);
                }
            }
        }

        private static void SetValues(object objStore, string[] split, int keyIndx)
        {
            if (objStore is IDictionary)
            {
                IDictionary dictObjStore = objStore as IDictionary;
                Type[] genericArguments = dictObjStore.GetType().GetGenericArguments();
                object objValueStore = null;
                if (dictObjStore.Contains(split[keyIndx])) objValueStore = dictObjStore[split[keyIndx]];
                else if (genericArguments[1] is IDictionary || (!genericArguments[1].IsValueType &&
                    genericArguments[1] != typeof(string)))
                {
                    objValueStore = Activator.CreateInstance(genericArguments[1]);
                    ((IDictionary)objStore).Add(split[keyIndx], objValueStore);
                }
                else
                {
                    dictObjStore.Add(
                        ChangeType(split[keyIndx], genericArguments[0]),
                        ChangeType(split[keyIndx + 1], genericArguments[1]));
                }

                if (objValueStore != null)
                {
                    SplitIndexAttribute splitIndex = objStore.GetType().GetCustomAttribute<SplitIndexAttribute>();
                    int childkeyIndx = (splitIndex != null) ? splitIndex.Index : keyIndx + 1;

                    SetValues(objValueStore, split, childkeyIndx);
                }
            }
            else
            {
                PropertyInfo[] props = objStore.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propOfObjValue in props)
                {
                    SplitIndexAttribute splitIndex = propOfObjValue.GetCustomAttribute<SplitIndexAttribute>();
                    if (propOfObjValue.PropertyType == typeof(IDictionary))
                    {
                        SetValues(propOfObjValue.PropertyType, split, splitIndex.Index);
                    }
                    else
                    {
                        propOfObjValue.SetValue(objStore,
                            ChangeType(split[splitIndex.Index], propOfObjValue.PropertyType));
                    }
                }
            }
        }

        public static object ChangeType(object value, Type type)
        {
            if (type == typeof(bool))
            {
                if (value == null)
                {
                    value = false;
                }
                else if (value is bool)
                {
                    value = (bool)value;
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
                    if (s.StartsWith("F", StringComparison.OrdinalIgnoreCase) || s.StartsWith("N", StringComparison.OrdinalIgnoreCase))
                    {
                        value = false;
                    }
                    else if (double.TryParse(s, out d) && d == 0) // numeric zero
                    {
                        value = false;
                    }
                    else
                    {
                        value = true;
                    }
                }
            }
            else if (type.IsEnum)
            {
                value = Enum.Parse(type, value.ToString(), true);
            }
            else if (type == typeof(Guid))
            {
                // If it's already a guid, return it.
                if (!(value is Guid))
                {
                    if (value is string)
                    {
                        value = new Guid(value.ToString());
                    }
                    else
                    {
                        value = new Guid((byte[])value);
                    }
                }
            }
            else if(type == typeof(int))
            {
                if (string.IsNullOrEmpty(value.ToString())) value = 0;
                else value = Convert.ChangeType(value, type);
            }
            else if (type == typeof(double))
            {
                if (string.IsNullOrEmpty(value.ToString())) value = 0.0;
                else value = Convert.ChangeType(value, type);
            }
            else
            {
                value = Convert.ChangeType(value, type);
            }

            return value;
        }
    }
}
