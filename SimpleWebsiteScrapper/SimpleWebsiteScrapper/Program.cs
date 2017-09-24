using SimpleWebsiteScrapper.Engine;

namespace SimpleWebsiteScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            WikipediaFIFACodeEngine wikiFifaCodeEngine = new WikipediaFIFACodeEngine();
            wikiFifaCodeEngine.Scrap();
        }
    }
}
