using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi
{
    /// <summary>
    /// Add Dynamic WebApi
    /// </summary>
    public static class DynamicWebApiServiceExtensions
    {
        /// <summary>
        /// Add Dynamic WebApi to Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services,DynamicWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPreFix;
            AppConsts.CommonPostfixes = options.ApiRemovePostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new DynamicWebApiControllerFeatureProvider());

            // Register the assembly to the checklist
            foreach (var asmType in options.ApiAssemblies)
            {
                RegisterApplicationAssembly(partManager, asmType);
            }
            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new DynamicWebApiConvention(services));
            });

            return services;
        }


        /// <summary>
        /// Inject all Controller, Application Service in the assembly where type T resides into the container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        private static void RegisterControllersAndApplicationServices<T>(IServiceCollection services)
            where T : class
        {
            var controllerType = (typeof(Controller));
            var applicationServiceType = typeof(IDynamicWebApi);

            var allTypes = typeof(T).Assembly.GetTypes();
            foreach (var item in allTypes)
            {
                if (!item.IsPublic || item.IsSealed || item.IsAbstract)
                {
                    continue;
                }

                if (item.IsSubclassOf(controllerType))
                {
                    services.AddTransient(item);
                }
                if (applicationServiceType.IsAssignableFrom(item))
                {
                    services.AddTransient(item);
                }
            }
        }

        /// <summary>
        /// Inject the assembly of type T into the scan list
        /// </summary>
        private static void RegisterApplicationAssembly(ApplicationPartManager applicationPartManager, Assembly asm)
        {
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(asm));
        }
    }
}