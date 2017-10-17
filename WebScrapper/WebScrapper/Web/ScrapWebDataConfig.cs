using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrapper.Web
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ScrapWebDataConfig
    {

        private ScrapWebDataConfig[] scrapField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Scrap")]
        public ScrapWebDataConfig[] Scrap
        {
            get
            {
                return this.scrapField;
            }
            set
            {
                this.scrapField = value;
            }
        }

        private ScrapWebDataConfigScrapColumn[] columnField;

        private string nameField;

        private string urlField;

        private string xpathField;

        private string typeField;

        private byte tableinsfreshField;

        private bool tableinsfreshFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public ScrapWebDataConfigScrapColumn[] Column
        {
            get
            {
                return this.columnField;
            }
            set
            {
                this.columnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte tableinsfresh
        {
            get
            {
                return this.tableinsfreshField;
            }
            set
            {
                this.tableinsfreshField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tableinsfreshSpecified
        {
            get
            {
                return this.tableinsfreshFieldSpecified;
            }
            set
            {
                this.tableinsfreshFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrap
    {

        private ScrapWebDataConfigScrapScrap scrapField;

        

        /// <remarks/>
        public ScrapWebDataConfigScrapScrap Scrap
        {
            get
            {
                return this.scrapField;
            }
            set
            {
                this.scrapField = value;
            }
        }

        
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrap
    {

        private ScrapWebDataConfigScrapScrapScrap scrapField;

        private ScrapWebDataConfigScrapScrapColumn[] columnField;

        private string xpathField;

        /// <remarks/>
        public ScrapWebDataConfigScrapScrapScrap Scrap
        {
            get
            {
                return this.scrapField;
            }
            set
            {
                this.scrapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public ScrapWebDataConfigScrapScrapColumn[] Column
        {
            get
            {
                return this.columnField;
            }
            set
            {
                this.columnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrapScrap
    {

        private ScrapWebDataConfigScrapScrapScrapColumn[] columnField;

        private string typeField;

        private string xpathField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public ScrapWebDataConfigScrapScrapScrapColumn[] Column
        {
            get
            {
                return this.columnField;
            }
            set
            {
                this.columnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrapScrapColumn
    {

        private string nameField;

        private string xpathField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrapColumn
    {

        private ScrapWebDataConfigScrapScrapColumnManipulate manipulateField;

        private string nameField;

        private string xpathField;

        private byte ispkField;

        private bool ispkFieldSpecified;

        /// <remarks/>
        public ScrapWebDataConfigScrapScrapColumnManipulate Manipulate
        {
            get
            {
                return this.manipulateField;
            }
            set
            {
                this.manipulateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte ispk
        {
            get
            {
                return this.ispkField;
            }
            set
            {
                this.ispkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ispkSpecified
        {
            get
            {
                return this.ispkFieldSpecified;
            }
            set
            {
                this.ispkFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrapColumnManipulate
    {

        private ScrapWebDataConfigScrapScrapColumnManipulateSplit splitField;

        /// <remarks/>
        public ScrapWebDataConfigScrapScrapColumnManipulateSplit Split
        {
            get
            {
                return this.splitField;
            }
            set
            {
                this.splitField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapScrapColumnManipulateSplit
    {

        private string dataField;

        private byte indexField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapColumn
    {

        private string nameField;

        private string xpathField;

        private byte ispkField;

        private bool ispkFieldSpecified;

        private byte indexField;

        private bool indexFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte ispk
        {
            get
            {
                return this.ispkField;
            }
            set
            {
                this.ispkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ispkSpecified
        {
            get
            {
                return this.ispkFieldSpecified;
            }
            set
            {
                this.ispkFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexSpecified
        {
            get
            {
                return this.indexFieldSpecified;
            }
            set
            {
                this.indexFieldSpecified = value;
            }
        }
    }


}
