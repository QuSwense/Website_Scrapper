using System;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    public class FieldData
    {
        public object Result { get; set; }
        public Action<object> Parse { get; set; }
    }
}
