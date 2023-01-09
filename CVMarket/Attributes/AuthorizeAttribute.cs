using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;

namespace CVMarket.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public AuthorizeAttribute()
        {
            
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User currentUser = context.HttpContext.Items["SessionUser"] as User;

            if (currentUser == null)
            {
                IActionResult result = new JsonResult(new { Message = "You can not access this method without signin" }) { StatusCode = 401 };
                context.Result = result;
            }
        }
    }
}
