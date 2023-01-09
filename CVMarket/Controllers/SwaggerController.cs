using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.API.Base;

namespace CVMarket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SwaggerController : ControllersBase
    {
        public SwaggerController()
        {

        }

        [HttpGet, Route("")]
        public IActionResult RedirectSwagger()
        {
            return Redirect("~/swagger/index.html");
        }
    }
}
