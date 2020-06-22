﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class MasterDetailModal : IModal
    {
        public MasterDetailPage MasterDetailPage { get; set; }
        public IModal DetailModal { get; set; }
        public IModal MasterModal { get; set; }
        public object Parameter { get; set; }
        public string Name { get; set; }
    }
}
