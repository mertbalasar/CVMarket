using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Requests;
using CVMarket.Core.Responses;
using CVMarket.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Business.Interfaces
{
    public interface IMarketService
    {
        Task<ServiceResponse<Market>> AddToMarket(Files file);
        Task<ServiceResponse> ReviewCv(ReviewCvRequest request);
        Task<ServiceResponse<List<MarketModel>>> GetFromMarket(GetCvRequest request);
    }
}
