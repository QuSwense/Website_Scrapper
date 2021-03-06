﻿using WebReader.Csv;

namespace SqliteDatabase.Model
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
