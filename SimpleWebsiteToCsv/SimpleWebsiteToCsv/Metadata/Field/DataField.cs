using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Field
{
    public class DataField<T> : BaseField
    {
        public T Value { get; set; }

        public DataField()
        {
            Value = default(T);
        }
    }
}
