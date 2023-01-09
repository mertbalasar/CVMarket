using CVMarket.Business.Interfaces;
using CVMarket.Core.Interfaces;
using CVMarket.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpAccessor;

        public CacheService(IMemoryCache cache, IHttpContextAccessor httpAccessor)
        {
            _cache = cache;
            _httpAccessor = httpAccessor;
        }

        public ServiceResponse<TOut> GetCache<TOut>(IRequestBase request)
        {
            var response = new ServiceResponse<TOut> { };

            try
            {
                var key = KeyGenerator(request);

                if (key.Code != 200)
                {
                    response.Code = key.Code;
                    response.Message = key.Message;
                    goto exit;
                }

                TOut value;
                _cache.TryGetValue(key.Result, out value);

                response.Result = value;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            exit:;
            return response;
        }

        public ServiceResponse<TOut> SetCache<TOut>(IRequestBase request, TOut value)
        {
            var response = new ServiceResponse<TOut> { };

            try
            {
                var key = KeyGenerator(request);

                if (key.Code != 200)
                {
                    response.Code = key.Code;
                    response.Message = key.Message;
                    goto exit;
                }

                var result = _cache.Set(key.Result, value);

                response.Result = result;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            exit:;
            return response;
        }

        private ServiceResponse<string> KeyGenerator(IRequestBase request)
        {
            var response = new ServiceResponse<string> { };

            try
            {
                var result = JsonConvert.SerializeObject((_httpAccessor.HttpContext.Request.Path, request));
                response.Result = result;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
