using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using WebCommon.Extn;

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
        /// Constructor default
        /// </summary>
        public CSVReader() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullfile"></param>
        /// <param name="store"></param>
        public CSVReader(string fullfile, object store) : base(fullfile, store) { }

        /// <summary>
        /// This is a protected method which is overriden in derived class
        /// </summary>
        protected override void ReadLineOverride(string line)
        {
            int keyIndx = 0;
            List<string> splits = new List<string>();

            var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
            foreach (Match m in regex.Matches(line))
            {
                splits.Add(m.Value);
            }

            SetValues(Store, splits, keyIndx);
        }

        /// <summary>
        /// A recursive set value function
        /// </summary>
        /// <param name="objStore"></param>
        /// <param name="split"></param>
        /// <param name="keyIndx"></param>
        private void SetValues(object store, List<string> splits, int keyIndx)
        {
            // If the current object is Dictionary
            if (store is IDictionary)
            {
                SetValueToDictionary(store as IDictionary, splits, keyIndx);
            }
            else
            {
                SetValueToClass(store, splits, keyIndx);
            }
        }

        /// <summary>
        /// Set the value to a class type
        /// </summary>
        /// <param name="objStore"></param>
        /// <param name="split"></param>
        /// <param name="keyIndx"></param>
        private void SetValueToClass(object objStore, List<string> splits, int keyIndx)
        {
            PropertyInfo[] props = objStore.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propOfObjValue in props)
            {
                SplitIndexAttribute splitIndex = propOfObjValue.GetCustomAttribute<SplitIndexAttribute>();
                if (propOfObjValue.PropertyType == typeof(IDictionary))
                {
                    SetValues(propOfObjValue.PropertyType, splits, splitIndex.Index);
                }
                else
                {
                    propOfObjValue.SetValue(objStore,
                        propOfObjValue.PropertyType.ChangeType(splits[splitIndex.Index]));
                }
            }
        }

        /// <summary>
        /// Set the value to the dictionary object
        /// </summary>
        /// <param name="store"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetValueToDictionary(IDictionary dictObjStore, List<string> splits, int keyIndx)
        {
            // The type of dictionary object
            Type dictObjStoreType = dictObjStore.GetType();

            // The dictionary generic arguments type
            Type[] genericArguments = dictObjStoreType.GetGenericArguments();
            if(genericArguments != null && genericArguments.Length <= 0)
            {
                if(dictObjStoreType.BaseType.IsGenericType &&
                    dictObjStoreType.BaseType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    dictObjStoreType = dictObjStoreType.BaseType;
                genericArguments = dictObjStoreType.GetGenericArguments();
            }

            // The Dictionary value
            object objValueStore = null;

            // Get the existing dictionary object
            if (dictObjStore.Contains(splits[keyIndx])) objValueStore = dictObjStore[splits[keyIndx]];
            else if (genericArguments[1] is IDictionary || (!genericArguments[1].IsValueType &&
                genericArguments[1] != typeof(string)))
            {
                // If the value type is a class type which is not a dictionary or known value type
                // then create a value type using Activator
                objValueStore = Activator.CreateInstance(genericArguments[1]);
                dictObjStore.Add(splits[keyIndx], objValueStore);
            }
            else
            {
                // For a value type (a terminal node in this recusrsive call)
                if (splits.Count >= keyIndx + 1)
                    dictObjStore.Add(
                        genericArguments[0].ChangeType(splits[keyIndx]),
                        genericArguments[1].ChangeType(splits[keyIndx + 1]));
            }

            if (objValueStore != null)
            {
                SplitIndexAttribute splitIndex = dictObjStoreType.GetCustomAttribute<SplitIndexAttribute>();
                int childkeyIndx = (splitIndex != null) ? splitIndex.Index : keyIndx + 1;

                SetValues(objValueStore, splits, childkeyIndx);
            }
        }

        /// <summary>
        /// A static helper functio nto read a file and save it to data structure
        /// </summary>
        /// <param name="path"></param>
        public static T Read<T>(string path) where T: new()
        {
            T fileObj = new T();
            using (CSVReader reader = new CSVReader(path, fileObj)) reader.Read();
            return fileObj;
        }
    }
}
