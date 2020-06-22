using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public class TabbedModal : IModal   
    {
        public BaseTabbedPage TabbedPage { get; set; }
        public ICollection<IModal> Modals { get; set; }
        public object Parameter { get; set; }
        public string Name { get; set; }
    }
}
