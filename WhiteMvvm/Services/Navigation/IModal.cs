using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Services.Navigation
{
    public interface IModal
    {
        object NavigationParameter { get; set; }
        string Name { get; set; }
    }
}
