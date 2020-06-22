using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Exceptions
{
    public sealed class NavigationException : Exception
    {
        public NavigationException()
        {
            Source = "NavigationService";
        }
        public NavigationException(string message) : base($"Navigation Error: {message}")
        {
            Source = "NavigationService";
        }
        public NavigationException(string message, Exception exception) : base($"Navigation Error: {message}", exception)
        {
            Source = "NavigationService";
        }
    }
}
