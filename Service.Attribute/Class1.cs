using System;

namespace ServiceAbsAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {
            ServiceName = string.Empty;
        }

        public ServiceAttribute(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }
    }
}
