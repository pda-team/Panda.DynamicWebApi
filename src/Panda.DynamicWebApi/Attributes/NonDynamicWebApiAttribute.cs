using System;

namespace Panda.DynamicWebApi.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicWebApiAttribute:Attribute
    {
        
    }
}