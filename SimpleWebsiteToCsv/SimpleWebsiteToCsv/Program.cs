using SimpleWebsiteToCsv.RawParser;

namespace SimpleWebsiteToCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            CIAWorldFactbookParser parserObj = new CIAWorldFactbookParser();
            parserObj.Process();
        }
    }
}
