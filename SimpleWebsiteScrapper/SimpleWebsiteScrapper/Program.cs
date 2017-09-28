using QWScrapEngine.Wikipedia.FIFA;

namespace SimpleWebsiteScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            MembersAndListOfCodes wikiFifaCodeEngine = new MembersAndListOfCodes();
            wikiFifaCodeEngine.Parse();
        }
    }
}
