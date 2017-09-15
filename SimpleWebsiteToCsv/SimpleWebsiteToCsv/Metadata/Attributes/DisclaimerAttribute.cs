using System;

namespace SimpleWebsiteToCsv.Common
{
    public class DisclaimerAttribute : Attribute
    {
        public string Disclaimer { get; set; }

        public DisclaimerAttribute(string disclaimer)
        {
            Disclaimer = disclaimer;
        }
    }
}
