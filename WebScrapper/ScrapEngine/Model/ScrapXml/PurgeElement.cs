using ScrapEngine.Common;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    public class PurgeElement : ManipulateChildElement
    {
        [DXmlAttribute(ScrapXmlConsts.IsEmptyOrNullAttributeName)]
        public bool IsEmptyOrNull { get; set; }

        public ColumnElement Parent { get; set; }
    }
}
