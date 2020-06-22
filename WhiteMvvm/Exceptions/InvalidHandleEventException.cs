using System;
using System.Reflection;

namespace WhiteMvvm.Exceptions
{
    public class InvalidHandleEventException : Exception
    {
        public InvalidHandleEventException(string message, TargetParameterCountException targetParameterCountException) : base(message, targetParameterCountException)
        {
        }
    }
}
