﻿using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class NavigationModal : IModal
    {
        public NavigationPage NavigationPage { get; set; } = new NavigationPage();
        public IModal RootModal{ get; set; }
        public object? NavigationParameter { get; set; }
        public string? Name { get; set; }
    }
}
