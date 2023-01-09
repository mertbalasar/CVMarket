using CVMarket.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CVMarket.Core.Enums.EnumLibrary;

namespace CVMarket.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CheckUserAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserType _type;

        public CheckUserAttribute(UserType type)
        {
            _type = type;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["SessionUser"] as User;

            if (user == null)
            {
                var result = new JsonResult(new { Message = "You can not access this method without signin" }) { StatusCode = 401 };
                context.Result = result;
            }
            else if (user.Type != _type)
            {
                var result = new JsonResult(new { Message = "You can not access this method with your user title" }) { StatusCode = 401 };
                context.Result = result;
            }
        }
    }
}
