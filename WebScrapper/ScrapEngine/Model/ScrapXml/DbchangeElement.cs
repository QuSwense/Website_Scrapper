using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapEngine.Common;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    public class DbchangeElement : ManipulateChildElement
    {
        [DXmlElement(ScrapXmlConsts.DbchangeExistsNodeName)]
        public DbchangeExistsElement Exists { get; set; }
    }
}
