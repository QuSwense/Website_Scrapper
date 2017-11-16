using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;

namespace ScrapEngine.Interfaces
{
    /// <summary>
    /// The main scrapper command class
    /// </summary>
    public interface IScrapperCommand
    {
        #region Load

        /// <summary>
        /// Load a html page from online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        HtmlNode Load(string url);

        /// <summary>
        /// Load a file from the online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string LoadFile(string url);

        #endregion Load

        #region Read

        /// <summary>
        /// Read the html page and extract the node represented by the XPath
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        List<HtmlNodeNavigator> ReadNodes(HtmlNode htmlNode, string xPath);

        #endregion Read
    }
}
