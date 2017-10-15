using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.scrap
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ScrapWebDataConfig
    {

        private ScrapWebDataConfigScrap[] scrapField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Scrap")]
        public ScrapWebDataConfigScrap[] Scrap
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
    public partial class ScrapWebDataConfigScrap
    {

        private ScrapWebDataConfigScrapColumn[] columnField;

        private string nameField;

        private string urlField;

        private string xpathField;

        private string typeField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapColumn
    {

        private ScrapWebDataConfigScrapColumnManipulate manipulateField;

        private string nameField;

        private string xpathField;

        private byte ispkField;

        private bool ispkFieldSpecified;

        private byte indexField;

        private bool indexFieldSpecified;

        /// <remarks/>
        public ScrapWebDataConfigScrapColumnManipulate Manipulate
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScrapWebDataConfigScrapColumnManipulate
    {

        private ScrapWebDataConfigScrapColumnManipulateSplit splitField;

        /// <remarks/>
        public ScrapWebDataConfigScrapColumnManipulateSplit Split
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
    public partial class ScrapWebDataConfigScrapColumnManipulateSplit
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


}
