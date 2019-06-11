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
            ApiRemovePostfixes=new List<string>(){ "AppService", "ApplicationService" };
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
        public string DefaultApiPreFix { get; set; } = "api";

        /// <summary>
        /// Remove the dynamic API class name suffix.
        /// </summary>
        public List<string> ApiRemovePostfixes { get; set; }

        /// <summary>
        /// Ignore MVC Form Binding types.
        /// </summary>
        public List<Type> FormBodyBindingIgnoredTypes { get; set; }


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

            if (string.IsNullOrEmpty(DefaultApiPreFix))
            {
                DefaultApiPreFix = string.Empty;
            }

            if (FormBodyBindingIgnoredTypes==null)
            {
                throw new ArgumentException($"{nameof(FormBodyBindingIgnoredTypes)} can not be null.");
            }

            if (ApiRemovePostfixes == null)
            {
                throw new ArgumentException($"{nameof(ApiRemovePostfixes)} can not be null.");
            }
        }
    }
}