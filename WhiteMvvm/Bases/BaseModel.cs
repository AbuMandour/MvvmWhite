using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using SQLite;
using WhiteMvvm.Utilities;

namespace WhiteMvvm.Bases
{
    public class BaseModel : NotifiedObject
    {
        public string Id { get; set; }
    }
}
