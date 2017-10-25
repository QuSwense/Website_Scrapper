using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebScrapper.Reader.Meta;
using WebScrapper.Common;

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

        public static object ChangeType(string value, Type type)
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
            else if(type == typeof(int))
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
