using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebReader.Csv
{
    /// <summary>
    /// A dynamic csv file reader into a generic type of object.
    /// If a Dictionary is used then currently it supports saving the columns in order of the presence of the
    /// keys, e.g.,
    /// The store - Dictionary{key1, Dictionary{key2, value1}}
    /// The csv file - d1,d2,d3,d4,
    /// The way the data is stored is:
    /// key1 = d1, key2 = d2, value1 = d3, d4 is skipped
    /// </summary>
    public class CSVReader : DynamicReader
    {
        /// <summary>
        /// This is a protected method which is overriden in derived class
        /// </summary>
        protected override void ReadLineOverride(string line)
        {
            int keyIndx = 0;
            string[] split = line.Split(new char[] { ',' });

            SetValues(Store, split, keyIndx);
        }

        /// <summary>
        /// A recursive set value function
        /// </summary>
        /// <param name="objStore"></param>
        /// <param name="split"></param>
        /// <param name="keyIndx"></param>
        private void SetValues(object store, string[] split, int keyIndx)
        {
            // If the current object is Dictionary
            if (store is IDictionary)
            {
                SetValueToDictionary(store as IDictionary, split, keyIndx);
            }
            else
            {
                SetValueToClass(store, split, keyIndx);
            }
        }

        /// <summary>
        /// Set the value to a class type
        /// </summary>
        /// <param name="objStore"></param>
        /// <param name="split"></param>
        /// <param name="keyIndx"></param>
        private void SetValueToClass(object objStore, string[] split, int keyIndx)
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

        /// <summary>
        /// Set the value to the dictionary object
        /// </summary>
        /// <param name="store"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetValueToDictionary(IDictionary dictObjStore, string[] split, int keyIndx)
        {
            // The type of dictionary object
            Type dictObjStoreType = dictObjStore.GetType();

            // The dictionary generic arguments type
            Type[] genericArguments = dictObjStoreType.GetGenericArguments();

            // The Dictionary value
            object objValueStore = null;

            // Get the existing dictionary object
            if (dictObjStore.Contains(split[keyIndx])) objValueStore = dictObjStore[split[keyIndx]];
            else if (genericArguments[1] is IDictionary || (!genericArguments[1].IsValueType &&
                genericArguments[1] != typeof(string)))
            {
                // If the value type is a class type which is not a dictionary or known value type
                // then create a value type using Activator
                objValueStore = Activator.CreateInstance(genericArguments[1]);
                dictObjStore.Add(split[keyIndx], objValueStore);
            }
            else
            {
                // For a value type (a terminal node in this recusrsive call)
                if (split.Length >= keyIndx + 1)
                    objValueStore = ChangeType(split[keyIndx + 1], genericArguments[1]);
                dictObjStore.Add(
                    ChangeType(split[keyIndx], genericArguments[0]),
                    objValueStore);
            }

            if (objValueStore != null)
            {
                SplitIndexAttribute splitIndex = dictObjStoreType.GetCustomAttribute<SplitIndexAttribute>();
                int childkeyIndx = (splitIndex != null) ? splitIndex.Index : keyIndx + 1;

                SetValues(objValueStore, split, childkeyIndx);
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
