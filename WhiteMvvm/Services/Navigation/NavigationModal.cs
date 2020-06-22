using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class NavigationModal : IModal
    {
        public NavigationPage NavigationPage { get; set; }
        public IModal RootModal{ get; set; }
        public object Parameter { get; set; }
        public string Name { get; set; }
    }
}
