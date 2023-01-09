using CVMarket.Core.Interfaces;
using CVMarket.Core.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Business.Interfaces
{
    public interface ICacheService
    {
        ServiceResponse<TOut> GetCache<TOut>(IRequestBase request);
        ServiceResponse<TOut> SetCache<TOut>(IRequestBase request, TOut value);
    }
}
