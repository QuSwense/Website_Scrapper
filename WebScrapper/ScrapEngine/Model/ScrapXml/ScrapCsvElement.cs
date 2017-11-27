using WebReader.Model;

namespace ScrapEngine.Model
{
    public class ScrapCsvElement : ScrapElement
    {
        /// <summary>
        /// The name of the Element tag
        /// </summary>
        public static string TagName = "ScrapCsv";

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("delimiter")]
        public string Delimiter { get; set; }

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("skipfirst")]
        public int SkipFirstLines { get; set; }

        public ScrapCsvElement()
        {
            SkipFirstLines = 0;
            Delimiter = "\\t";
        }
    }
}
