﻿using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Default
{
    public class DTable : DynamicTable<
        DynamicRow, 
        DColumnMetadata>
    {
    }
}
