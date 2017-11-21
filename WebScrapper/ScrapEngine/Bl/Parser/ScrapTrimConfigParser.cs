﻿using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ScrapEngine.Model.ScrapXml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapTrimConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapTrimConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public override ManipulateChildElement Process(XmlNode trimNode, ManipulateElement webConfigObj)
        {
            TrimElement configTrimObj = configParser.XmlConfigReader.ReadElement<TrimElement>(trimNode);
            configTrimObj.Data = Normalize(configTrimObj.Data);
            Assert(configTrimObj);
            return configTrimObj;
        }

        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(result.OriginalValue)) return;

            TrimElement trimElement = (TrimElement)manipulateChild;

            List<string> finalResults = new List<string>();
            for (int i = 0; i < result.Results.Count; ++i)
            {
                if (!string.IsNullOrEmpty(trimElement.Data))
                    finalResults.Add(result.Results[i].Trim(trimElement.Data.ToCharArray()));
                else
                    finalResults.Add(result.Results[i].Trim());
            }
            result.Results = new List<string>(finalResults);
        }

        private void Assert(TrimElement configTrimObj)
        {
            if (string.IsNullOrEmpty(configTrimObj.Data))
                return;
            // For empty Trim consider default string.Trim() method
        }
    }
}