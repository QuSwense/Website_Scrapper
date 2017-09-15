﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Metadata.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class QSReferenceFieldAttribute : QSReferenceAttribute
    {
        public string FieldName { get; set; }

        public QSReferenceFieldAttribute(string field)
        {

        }
    }
}
