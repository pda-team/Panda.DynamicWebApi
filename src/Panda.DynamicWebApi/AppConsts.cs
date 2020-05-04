using Panda.DynamicWebApi.Helpers;
using System;
using System.Collections.Generic;

namespace Panda.DynamicWebApi
{
    public static class AppConsts
    {
        public static string DefaultHttpVerb { get; set; }

        public static string DefaultAreaName { get; set; } 
 
        public static string DefaultApiPreFix { get; set; }

        public static List<string> ControllerPostfixes { get; set; }
        public static List<string> ActionPostfixes { get; set; }

        public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

        public static Dictionary<string,string> HttpVerbs { get; set; }

        public static Func<string, string> GetActionName { get; set; }

        static AppConsts()
        {
            HttpVerbs=new Dictionary<string, string>()
            {
                ["add"]="POST",
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

            GetActionName = (actionName) =>
            {
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
            };
        }
    }
}