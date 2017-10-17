﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.Db
{
    public class EnumConfigModel
    {
        public string Name { get; set; }
        public Dictionary<int, EnumValueConfigModel> Values { get; set; }

        public EnumConfigModel()
        {
            Values = new Dictionary<int, EnumValueConfigModel>();
        }
    }
}
