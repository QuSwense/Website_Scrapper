using ScrapEngine.Model.Scrap;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// Column scrap iterator arguments for Csv File type
    /// </summary>
    public class ColumnScrapIteratorFileArgs : ColumnScrapStateModel
    {
        public string FileLine { get; set; }

        private string[] splitData;

        public override void PreProcess()
        {
            // By default take the last parent scrap element
            if (ColumnElementConfig.Level < 0)
            {
                ScrapCsvElement ScrapConfigCsv = Parent.ScrapConfigObj as ScrapCsvElement;
                if (ScrapConfigCsv != null)
                {
                    splitData = FileLine.Split(new char[] { ScrapConfigCsv.Delimiter[0] });
                }
            }
            else
                base.PreProcess();
        }

        public override string GetDataIterator()
        {
            ScrapCsvElement ScrapConfigCsv = Parent.ScrapConfigObj as ScrapCsvElement;
            return splitData[ColumnElementConfig.Index];
        }
    }
}
