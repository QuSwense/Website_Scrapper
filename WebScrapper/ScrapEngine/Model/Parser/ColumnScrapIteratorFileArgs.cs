namespace ScrapEngine.Model.Parser
{
    public class ColumnScrapIteratorFileArgs : ColumnScrapIteratorArgs
    {
        public string FileLine { get; set; }

        private string[] splitData;

        public override void PreProcess()
        {
            ScrapCsvElement ScrapConfigCsv = Parent as ScrapCsvElement;
            if (ScrapConfigCsv != null)
            {
                splitData = FileLine.Split(new char[] { ScrapConfigCsv.Delimiter[0] });
            }
        }

        public override string GetDataIterator(ColumnElement columnConfig)
        {
            return splitData[columnConfig.Index];
        }
    }
}
