using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWSettings
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PathSettings
    {

        private string unwantedPathCharsField;

        private string webOfflineRootFolderField;

        private PathSettingsDns[] mapPathField;

        /// <remarks/>
        public string UnwantedPathChars
        {
            get
            {
                return this.unwantedPathCharsField;
            }
            set
            {
                this.unwantedPathCharsField = value;
            }
        }

        /// <remarks/>
        public string WebOfflineRootFolder
        {
            get
            {
                return this.webOfflineRootFolderField;
            }
            set
            {
                this.webOfflineRootFolderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Dns", IsNullable = false)]
        public PathSettingsDns[] MapPath
        {
            get
            {
                return this.mapPathField;
            }
            set
            {
                this.mapPathField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PathSettingsDns
    {

        private string nameRegexField;

        private string replaceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nameRegex
        {
            get
            {
                return this.nameRegexField;
            }
            set
            {
                this.nameRegexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string replace
        {
            get
            {
                return this.replaceField;
            }
            set
            {
                this.replaceField = value;
            }
        }
    }


}
