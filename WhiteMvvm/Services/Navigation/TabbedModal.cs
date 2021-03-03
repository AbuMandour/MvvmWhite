using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class TabbedModal : IModal   
    {
        public TabbedPage TabbedPage { get; set; } = new TabbedPage();
        public ICollection<IModal> Modals { get; set; }
        public object? NavigationParameter { get; set; }
        public string? Name { get; set; }
    }
}
