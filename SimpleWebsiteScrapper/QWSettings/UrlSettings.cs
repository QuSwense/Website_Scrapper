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
    public partial class UrlSettings
    {

        private byte doGenerateOfflineUriFromOnlineField;

        private byte useOfflineLinkPreferenceField;

        /// <remarks/>
        public byte DoGenerateOfflineUriFromOnline
        {
            get
            {
                return this.doGenerateOfflineUriFromOnlineField;
            }
            set
            {
                this.doGenerateOfflineUriFromOnlineField = value;
            }
        }

        /// <remarks/>
        public byte UseOfflineLinkPreference
        {
            get
            {
                return this.useOfflineLinkPreferenceField;
            }
            set
            {
                this.useOfflineLinkPreferenceField = value;
            }
        }
    }


}
