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
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Responses;

namespace CVMarket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MarketController : ControllersBase
    {
        private readonly IMarketService _marketService;
        private readonly ICacheService _cacheService;

        public MarketController(IMarketService marketService,
            ICacheService cacheService)
        {
            _marketService = marketService;
            _cacheService = cacheService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetCv([FromBody] GetCvRequest request)
        {
            var cacheResult = _cacheService.GetCache<ServiceResponse<List<MarketModel>>>(request);
            if (cacheResult.Code == 200 && cacheResult.Result != null)
            {
                return APIResponse(cacheResult.Result);
            }

            var response = await _marketService.GetFromMarket(request);

            if (cacheResult.Code == 200 && cacheResult.Result == null)
            {
                _cacheService.SetCache(request, response);
            }

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
