using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.NATO
{
    [QSReference("NATO standardization is the development and implementation of concepts, doctrines and procedures to achieve and maintain the required levels of compatibility, interchangeability or commonality needed to achieve interoperability. A Standardization Agreement (STANAG) is a NATO standardization document that specifies the agreement of member nations to implement a standard.")]
    [QSURIDataSource(Online = "http://www.nato.int/structur/AC/135/main/links/codsp3-alt.htm")]
    [ReferenceCollection("//table[@id='sorting1']//tr[position() > 1]")]
    public class NATOSTANAG1059Country
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Name { get; set; }

        [About("Unique 3-letter codes for use within NATO from 1 April 2004 to distinguish geographical entities, nations and countries.")]
        [ReferenceSingle("td[position() = 2]")]
        public string Alpha3 { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        [About(@"MOE (Measure of Effectiveness) Code structure: A two character alpha code, the first character of which is Z, Y, W or V. These codes are also used as :
- REFERENCE NUMBER ACTION ACTIVITY CODES(DRN 2900) ;
-SUBMITTER CODES(DRN 3720) ;
-DESTINATION ACTIVITY CODES(DRN 3880) ;
-ORIGINATOR CODES(DRN 4210).
The NATO MOE RULE No(DRN 8290) for international exchange of codification data is a four character code consisting of two alphabetic characters as given in the table above followed by a two digit non - significant number 01.")]
        public string MOE { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        [About("NCB stands for National Codification Bureau. The NCB is the organisation, typically a government agency, in charge of maintaining the NCS (NATO Supply Class) database within a given country. The National Codification Bureau (NCB) code is a two-digit code that occupies the fifth and sixth position of a NATO stock number. This code identifies the NATO country that originally cataloged the item of supply.")]
        public string NCB { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        [About(@"NCAGE (NATO Commercial And Government Entity Code) is a code which identifies the manufacturer, supplier or agent (provider) in NATO codification system. NCAGE consists of five characters. NCAGE will be assigned by NSPA. NCAGE Code structure explanation:
# = numeral
* = alpha / numerical")]
        public string NCAGE { get; set; }
    }
}
