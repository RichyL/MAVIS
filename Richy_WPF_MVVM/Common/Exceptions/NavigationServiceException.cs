using System;

namespace Richy_WPF_MVVM.Common.Exceptions
{
    public class NavigationServiceException : Exception
    {
        public NavigationServiceException(string message) : base(message) { }

    }
}
