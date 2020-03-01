using System;

namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Input : Attribute
    {
        public Input(params object[] parameters)
        {
            Parameters = parameters;
        }

        public object[] Parameters { get; }
    }
}