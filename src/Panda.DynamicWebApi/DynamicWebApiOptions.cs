using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Panda.DynamicWebApi
{
    public class DynamicWebApiOptions
    {
        public DynamicWebApiOptions()
        {
            RemoveControllerPostfixes=new List<string>(){ "AppService", "ApplicationService" };
            RemoveActionPostfixes = new List<string>(){ "Async"};
            FormBodyBindingIgnoredTypes=new List<Type>(){typeof(IFormFile)};
        }


        /// <summary>
        /// API HTTP Verb.
        /// <para></para>
        /// Default value is "POST".
        /// </summary>
        public string DefaultHttpVerb { get; set; } = "POST";

        public string DefaultAreaName { get; set; }

        /// <summary>
        /// Routing prefix for all APIs
        /// <para></para>
        /// Default value is "api".
        /// </summary>
        public string DefaultApiPrefix { get; set; } = "api";

        /// <summary>
        /// Remove the dynamic API class(Controller) name postfix.
        /// <para></para>
        /// Default value is {"AppService", "ApplicationService"}.
        /// </summary>
        public List<string> RemoveControllerPostfixes { get; set; }

        /// <summary>
        /// Remove the dynamic API class's method(Action) postfix.
        /// <para></para>
        /// Default value is {"Async"}.
        /// </summary>
        public List<string> RemoveActionPostfixes { get; set; }

        /// <summary>
        /// Ignore MVC Form Binding types.
        /// </summary>
        public List<Type> FormBodyBindingIgnoredTypes { get; set; }

        /// <summary>
        /// Verify that all configurations are valid
        /// </summary>
        public void Valid()
        {
            if (string.IsNullOrEmpty(DefaultHttpVerb))
            {
                throw new ArgumentException($"{nameof(DefaultHttpVerb)} can not be empty.");
            }

            if (string.IsNullOrEmpty(DefaultAreaName))
            {
                DefaultAreaName = string.Empty;
            }

            if (string.IsNullOrEmpty(DefaultApiPrefix))
            {
                DefaultApiPrefix = string.Empty;
            }

            if (FormBodyBindingIgnoredTypes==null)
            {
                throw new ArgumentException($"{nameof(FormBodyBindingIgnoredTypes)} can not be null.");
            }

            if (RemoveControllerPostfixes == null)
            {
                throw new ArgumentException($"{nameof(RemoveControllerPostfixes)} can not be null.");
            }
        }
    }
}