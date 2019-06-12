using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi
{
    public class DynamicWebApiConvention: IApplicationModelConvention
    {
        private readonly IServiceCollection _services;
        public DynamicWebApiConvention(IServiceCollection services)
        {
            this._services = services;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();

                var dynamicWebApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(type.GetTypeInfo());
                if (typeof(IDynamicWebApi).GetTypeInfo().IsAssignableFrom(type))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());
                    ConfigureArea(controller, dynamicWebApiAttr);
                    ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                }
                else
                {
                    if (dynamicWebApiAttr != null)
                    {
                        ConfigureArea(controller, dynamicWebApiAttr);
                        ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                    }
                }
            }
        }

        private void ConfigureArea(ControllerModel controller,DynamicWebApiAttribute attr)
        {
            if (attr == null)
            {
                throw new ArgumentException(nameof(attr));
            }

            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (!string.IsNullOrEmpty(attr.Module))
                {
                    controller.RouteValues["area"] = attr.Module;
                }
                else if (!string.IsNullOrEmpty(AppConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = AppConsts.DefaultAreaName;
                }
            }

        }

        private void ConfigureDynamicWebApi(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }

        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                foreach (var para in action.Parameters)
                {
                    if (para.BindingInfo != null)
                    {
                        continue;
                    }

                    if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                    {
                        if (CanUseFormBodyBinding(action, para))
                        {
                            para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                        }
                    }
                }
            }
        }


        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {
                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #region ConfigureApiExplorer

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }

        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion

        private void ConfigureSelector(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            RemoveEmptySelectors(controller.Selectors);

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var areaName = string.Empty;

            if (controllerAttr != null)
            {
                areaName = controllerAttr.Module;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureSelector(areaName,controller.ControllerName, action);
            }
        }

        private void ConfigureSelector(string areaName, string controllerName, ActionModel action)
        {
            RemoveEmptySelectors(action.Selectors);

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicWebApiAttribute>(action.ActionMethod);

            if (nonAttr != null)
            {
                return;
            }

            if (!action.Selectors.Any())
            {
                AddAppServiceSelector(areaName,controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(areaName,controllerName, action);
            }
        }

        private void AddAppServiceSelector(string areaName, string controllerName, ActionModel action)
        {
            string verb;
            var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();
            verb = AppConsts.HttpVerbs.ContainsKey(verbKey) ? AppConsts.HttpVerbs[verbKey] : AppConsts.DefaultHttpVerb;

            action.ActionName = GetRestFulActionName(action.ActionName);
            var appServiceSelectorModel = new SelectorModel
            {
                AttributeRouteModel = CreateActionRouteModel(areaName, controllerName, action.ActionName)
            };

            appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));

            action.Selectors.Add(appServiceSelectorModel);
        }

        /// <summary>
        /// Processing action name
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string GetRestFulActionName(string actionName)
        {
            // Remove Postfix
            actionName = actionName.RemovePostFix(AppConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();
            if (AppConsts.HttpVerbs.ContainsKey(verbKey))
            {
                if (actionName.Length == verbKey.Length)
                {
                    return "";
                }
                else
                {
                    return actionName.Substring(verbKey.Length);
                }
            }
            else
            {
                return actionName;
            }
        }

        private static void NormalizeSelectorRoutes(string areaName, string controllerName, ActionModel action)
        {
            action.ActionName = GetRestFulActionName(action.ActionName);
            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ? 
                    CreateActionRouteModel(areaName, controllerName, action.ActionName) : 
                    AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(areaName,controllerName,""), selector.AttributeRouteModel);
            }
        }

        private static AttributeRouteModel CreateActionRouteModel(string areaName, string controllerName, string actionName)
        {
            var routeStr =
                $"{AppConsts.DefaultApiPreFix}/{areaName}/{controllerName}/{actionName}".Replace("//", "/");
            return new AttributeRouteModel(new RouteAttribute(routeStr));
        }

        private static void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            selectors
                .Where(IsEmptySelector)
                .ToList()
                .ForEach(s => selectors.Remove(s));
        }

        private static bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null && selector.ActionConstraints.IsNullOrEmpty();
        }
    }
}