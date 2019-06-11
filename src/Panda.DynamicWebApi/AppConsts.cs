using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Panda.DynamicWebApi
{
    internal static class AppConsts
    {
        public static string DefaultHttpVerb { get; set; }

        public static string DefaultAreaName { get; set; } 
 
        public static string DefaultApiPreFix { get; set; }

        public static List<string> CommonPostfixes { get; set; }

        public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

        public static Dictionary<string,string> HttpVerbs { get; }

        static AppConsts()
        {
            HttpVerbs=new Dictionary<string, string>()
            {
                ["create"]="POST",
                ["post"]="POST",

                ["get"]="GET",
                ["find"]="GET",
                ["fetch"]="GET",
                ["query"]="GET",

                ["update"]="PUT",
                ["put"]= "PUT",

                ["delete"] = "DELETE",
                ["remove"] = "DELETE",
            };
        }
    }
}