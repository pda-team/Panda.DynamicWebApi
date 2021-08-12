using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi
{
    public class DynamicWebApiControllerFeatureProvider: ControllerFeatureProvider
    {
        private ISelectController _selectController;

        public DynamicWebApiControllerFeatureProvider(ISelectController selectController)
        {
            _selectController = selectController;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            return _selectController.IsController(typeInfo);
        }
    }
}