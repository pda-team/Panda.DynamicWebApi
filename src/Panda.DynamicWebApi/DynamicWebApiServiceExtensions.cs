using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi
{
    /// <summary>
    /// Add Dynamic WebApi
    /// </summary>
    public static class DynamicWebApiServiceExtensions
    {
        /// <summary>
        /// Use Dynamic WebApi to Configure
        /// </summary>
        /// <param name="application"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDynamicWebApi(this IApplicationBuilder application, Action<IServiceProvider,DynamicWebApiOptions> optionsAction)
        {
            var options = new DynamicWebApiOptions();

            optionsAction?.Invoke(application.ApplicationServices,options);

            options.Valid();

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.GetRestFulActionName = options.GetRestFulActionName;
            AppConsts.AssemblyDynamicWebApiOptions = options.AssemblyDynamicWebApiOptions;

            var partManager = application.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            // Add a custom controller checker
            var featureProviders = application.ApplicationServices.GetRequiredService<DynamicWebApiControllerFeatureProvider>();
            partManager.FeatureProviders.Add(featureProviders);

            foreach(var assembly in options.AssemblyDynamicWebApiOptions.Keys)
            {
                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);

                foreach(var part in partFactory.GetApplicationParts(assembly))
                {
                    partManager.ApplicationParts.Add(part);
                }
            }


            var mvcOptions = application.ApplicationServices.GetRequiredService<IOptions<MvcOptions>>();
            var dynamicWebApiConvention = application.ApplicationServices.GetRequiredService<DynamicWebApiConvention>();

            mvcOptions.Value.Conventions.Add(dynamicWebApiConvention);

            return application;
        }

        public static IServiceCollection AddDynamicWebApiCore<TSelectController, TActionRouteFactory>(this IServiceCollection services)
            where TSelectController: class,ISelectController
            where TActionRouteFactory: class, IActionRouteFactory
        {
            services.AddSingleton<ISelectController, TSelectController>();
            services.AddSingleton<IActionRouteFactory, TActionRouteFactory>();
            services.AddSingleton<DynamicWebApiConvention>();
            services.AddSingleton<DynamicWebApiControllerFeatureProvider>();
            return services;
        }

        /// <summary>
        /// Add Dynamic WebApi to Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, DynamicWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.GetRestFulActionName = options.GetRestFulActionName;
            AppConsts.AssemblyDynamicWebApiOptions = options.AssemblyDynamicWebApiOptions;

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new DynamicWebApiControllerFeatureProvider(options.SelectController));

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new DynamicWebApiConvention(options.SelectController, options.ActionRouteFactory));
            });

            return services;
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
        {
            return AddDynamicWebApi(services, new DynamicWebApiOptions());
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, Action<DynamicWebApiOptions> optionsAction)
        {
            var dynamicWebApiOptions = new DynamicWebApiOptions();

            optionsAction?.Invoke(dynamicWebApiOptions);

            return AddDynamicWebApi(services, dynamicWebApiOptions);
        }

    }
}