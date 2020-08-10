using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Bases;

namespace WhiteMvvm.Services.Navigation
{
    public class BasicModal : IModal
    {
        public BaseViewModel ViewModel { get; set; }
        public object NavigationParameter { get; set; }
        public string Name { get; set; }
    }
}
