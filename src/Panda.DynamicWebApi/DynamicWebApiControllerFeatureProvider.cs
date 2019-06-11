using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi
{
    public class DynamicWebApiControllerFeatureProvider: ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();

            if (!typeof(IDynamicWebApi).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }


            var attr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(typeInfo);

            if (attr == null)
            {
                return false;
            }

            if (ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NonDynamicWebApiAttribute>(typeInfo) != null)
            {
                return false;
            }

            return true;
        }
    }
}