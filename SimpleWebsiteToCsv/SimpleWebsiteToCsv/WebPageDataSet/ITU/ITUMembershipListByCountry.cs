using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    [QSReferenceFromDataSource("//div[@class='ms-rtestate-field']/div[position() = 1]/div[position() = 1]",
        OnlineUrl = "https://www.itu.int/en/membership/Pages/overview.aspx")]
    [QSUriRTTIDataSource("ITUMemberStates", "NameURL")]
    public class ITUMembershipDataByCountry
    {
        public List<ITUMembershipListByCountry> Memberships { get; set; }

        public List<ITUMembershipMission> Missions { get; set; }
    }

    [ReferenceCollection("//div[@class='container']/div[@class='content']/table[position() < last()]//tr[position() > 1]")]
    public class ITUMembershipListByCountry
    {
        [ReferenceSingle("td[position() = 1]")]
        [QSRegexSplit(Pattern = "((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))")]
        public NameCombined Name { get; set; }

        /// <summary>
        /// ITU is made up of three sectors, covering specific areas of ICT activity
        /// Radiocommunication (ITU-R)
        /// </summary>
        [ReferenceSingle("td[position() = 2]")]
        [QSReferenceFromDataSource("//div[@id='MSOZoneCell_WebPartWPQ1']//div[@class='ms-rtestate-field']/div[position() = 1]",
        OnlineUrl = "https://www.itu.int/en/ITU-R/information/Pages/default.aspx")]
        public string ITU_R { get; set; }

        /// <summary>
        /// Telecommunication Standards (ITU-T)
        /// </summary>
        [ReferenceSingle("td[position() = 3]")]
        [QSReferenceFromDataSource("//div[@id='ctl00_PlaceHolderMain_ctl00__ControlWrapper_RichHtmlField']",
        OnlineUrl = "https://www.itu.int/en/ITU-T/about/Pages/default.aspx")]
        public string ITU_T { get; set; }

        /// <summary>
        /// Telecommunication Development (ITU-D).
        /// </summary>
        [ReferenceSingle("td[position() = 4]")]
        [QSReferenceFromDataSource("//div[@id='ctl00_PlaceHolderMain_ctl00__ControlWrapper_RichHtmlField']",
        OnlineUrl = "https://www.itu.int/en/ITU-D/Pages/About.aspx")]
        public string ITU_D { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        [StringSplit(",/")]
        public List<string> Categories { get; set; }
    }

    public class NameCombined
    {
        [QSIndex(0)]
        public string Name { get; set; }

        [QSIndex(1)]
        public string Place { get; set; }
    }

    [ReferenceCollection("//div[@class='container']/div[@class='content']/table[position() = last()]//tr[position() > 1]")]
    public class ITUMembershipMission
    {
        [ReferenceSingle("td[position() = 1]")]
        [QSRegexSplit(Pattern = "((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))")]
        public NameCombined Name { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        [StringSplit(",/")]
        public List<string> Categories { get; set; }
    }
}
