﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class WebDataConfigTrim
    {
        [XmlAttribute("data")]
        public string Data { get; set; }
    }
}
