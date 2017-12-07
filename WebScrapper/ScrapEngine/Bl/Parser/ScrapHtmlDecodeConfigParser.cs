﻿using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapHtmlDecodeConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapHtmlDecodeConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Virtual process
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            List<string> finalList = new List<string>();

            for (int i = 0; i < result.Results.Count; i++)
            {
                finalList.Add(HttpUtility.HtmlDecode(result.Results[i]));
            }

            result.Results = new List<string>(finalList);
        }
    }
}