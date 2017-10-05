using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class CIAWorldFactbookCountryGovernmentModel : ScrapObject<object>
    {
        public ScrapString CountryConventionalLongName { get; set; }
        public ScrapString CountryConventionalShortName { get; set; }
        public ScrapString CountryLocalLongForm { get; set; }
        public ScrapString CountryLocalShortForm { get; set; }
        public ScrapString CountryFormer { get; set; }
        public ScrapString CountryEtymology { get; set; }
        public ScrapString GovernmentType { get; set; }
        public ScrapString CapitalName { get; set; }
        public ScrapString CapitalCoordinates { get; set; }
        public ScrapString TimeDifference { get; set; }
        public ScrapString AdministrativeDivisions { get; set; }
        public ScrapString Independence { get; set; }
        public ScrapString ChiefofState { get; set; }
        public ScrapString HeadOfGovernment { get; set; }
        public ScrapString Cabinet { get; set; }
        public ScrapString InternationalOrganizationParticipation { get; set; }
        public ScrapString DiplomaticInUSChiefofMission { get; set; }
        public ScrapString DiplomaticInUSChancery { get; set; }
        public ScrapString DiplomaticInUSTelephone { get; set; }
        public ScrapString DiplomaticInUSFAX { get; set; }
        public ScrapString DiplomaticInUSConsulateGeneral { get; set; }
        public ScrapString DiplomaticFromUSChiefofMission { get; set; }
        public ScrapString DiplomaticFromUSEmbassy { get; set; }
        public ScrapString DiplomaticFromUSMailingAddress { get; set; }
        public ScrapString DiplomaticFromUSTelephone { get; set; }
        public ScrapString DiplomaticFromUSFAX { get; set; }
        public ScrapString NationalAnthemName { get; set; }
        public ScrapString NationalAnthemLyricsMusic { get; set; }
    }
}
