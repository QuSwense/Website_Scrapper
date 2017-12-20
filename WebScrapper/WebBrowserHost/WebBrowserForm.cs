using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WebBrowserHost
{
    public partial class WebBrowserForm : Form
    {
        public WebBrowserForm()
        {
            InitializeComponent();

            LoadBrowser("https://nsgreg.nga.mil/genc/browse?register=any&type=ggp&gce=all&field=name&show=all&status=actv&day=19&month=12&year=2017&sort=nameasc?xyzallow");
        }

        ManualResetEvent oSignalEvent = new ManualResetEvent(false);

        public HtmlNode LoadBrowser(string url)
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            webBrowser.Visible = true;
            webBrowser.ScrollBarsEnabled = true;
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.AllowNavigation = true;
            webBrowser.Navigate(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(webBrowser.DocumentText);

            //oSignalEvent.WaitOne();
            //oSignalEvent.Reset();

            return document.DocumentNode;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            oSignalEvent.Set();
        }
    }
}
