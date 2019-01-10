using System;

namespace CastleAOPTest.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LoggingAttribute : Attribute
    {
        public bool IsValid { get; set; } = true;
    }
}