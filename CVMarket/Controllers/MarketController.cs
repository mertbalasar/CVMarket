using CVMarket.API.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.API.Attributes;
using CVMarket.Business.Interfaces;
using static CVMarket.Core.Enums.EnumLibrary;
using CVMarket.Core.Requests;

namespace CVMarket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MarketController : ControllersBase
    {
        private IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetCv([FromBody] GetCvRequest request)
        {
            var response = await _marketService.GetFromMarket(request);

            return APIResponse(response);
        }

        [CheckUser(UserType.Reviewer)]
        [HttpPut, Route("review")]
        public async Task<IActionResult> ReviewCv([FromBody] ReviewCvRequest request)
        {
            var response = await _marketService.ReviewCv(request);

            return APIResponse(response);
        } 
    }
}
