using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Exceptions
{
    public sealed class ReflectionResolveException : Exception
    {
        public ReflectionResolveException()
        {
            Source = "ReflectionResolveService";
        }
        public ReflectionResolveException(string message) : base($"Error while resolve: {message}")
        {
            Source = "ReflectionResolveService";
        }
        public ReflectionResolveException(string message, Exception exception) : base($"Error while resolve: {message}", exception)
        {
            Source = "ReflectionResolveService";
        }
    }
}
