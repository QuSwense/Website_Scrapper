﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Csv;

namespace DynamicDatabase.Config
{
    /// <summary>
    /// This resembles the csv config file for Table metadata
    /// </summary>
    public class ConfigDbTable
    {
        [SplitIndex(1)]
        public string Display { get; set; }

        [SplitIndex(2)]
        public string Reference { get; set; }
    }
}
