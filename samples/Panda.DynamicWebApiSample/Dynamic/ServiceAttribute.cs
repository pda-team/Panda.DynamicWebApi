using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Other.Controller;
using Panda.DynamicWebApi;
using ServiceAbsAttribute;
using System;
using System.Reflection;

namespace Panda.DynamicWebApiSample.Dynamic
{
    internal class ServiceActionRouteFactory : IActionRouteFactory
    {
        public string CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var controllerType = action.ActionMethod.DeclaringType;
            var serviceAttribute = controllerType.GetCustomAttribute<ServiceAttribute>();

            var _controllerName = serviceAttribute.ServiceName == string.Empty ? controllerName.Replace("Service", "") : serviceAttribute.ServiceName.Replace("Service", "");

            return $"api/{_controllerName}/{action.ActionName.Replace("Async", "")}";
        }
    }
}