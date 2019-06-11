using System;
using System.Reflection;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class DynamicWebApiAttribute : Attribute
    {
        public string Module { get; set; }

        public static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        public static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        public static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        public static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        public static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        public static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicWebApiAttribute>();
            return remoteServiceAttr != null;
        }
    }
}