﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Db.Meta;

namespace WebScrapper.Db
{
    [DDTable("tblcols")]
    public class TableColumnsModel
    {
        [DDPrimaryKey]
        [DDColumn("tnm")]
        public string TableName { get; set; }

        [DDPrimaryKey]
        [DDColumn("col")]
        public string ColumnName { get; set; }

        [DDColumn("rurl")]
        public string ReferenceUrl { get; set; }

        [DDColumn("rxpath")]
        public string ReferenceXPath { get; set; }

        [DDColumn("typ")]
        public string TypeOfDataExtract { get; set; }
    }
}