using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.Core.Responses;

namespace CVMarket.API.Base
{
    public class ControllersBase : ControllerBase
    {
        protected IActionResult APIResponse(ServiceResponse response)
        {
            switch (response.Code)
            {
                case StatusCodes.Status200OK:
                    return Ok();
                default:
                    return StatusCode(response.Code, new { Message = response.Message });
            }
        }

        protected IActionResult APIResponse<T>(ServiceResponse<T> response)
        {
            switch (response.Code)
            {
                case StatusCodes.Status200OK:
                    return Ok(response.Result);
                default:
                    return StatusCode(response.Code, new { Message = response.Message });
            }
        }
    }
}
