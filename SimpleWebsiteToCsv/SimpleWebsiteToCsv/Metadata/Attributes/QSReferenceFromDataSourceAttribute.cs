using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    /// <summary>
    /// If not URL is present then take the curent URL from 'QSURIDataSource'
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class QSReferenceFromDataSourceAttribute : ReferenceSingleAttribute
    {
        public string OnlineUrl { get; set; }
        public int StartSentence { get; set; }
        public int CountSentences { get; set; }

        public QSReferenceFromDataSourceAttribute(string xpath = "") : base(xpath)
        {
            StartSentence = 0;
            CountSentences = 1;
        }
    }
}
