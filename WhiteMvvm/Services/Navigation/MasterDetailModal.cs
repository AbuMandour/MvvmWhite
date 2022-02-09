using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class MasterDetailModal : IModal
    {
        public FlyoutPage MasterDetailPage { get; set; } = new FlyoutPage();
        public IModal DetailModal { get; set; }
        public IModal MasterModal { get; set; }
        public object? NavigationParameter { get; set; }
        public string? Name { get; set; }
    }
}
