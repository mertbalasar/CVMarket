using CVMarket.Business.Base;
using CVMarket.Business.Interfaces;
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Requests;
using CVMarket.Core.Responses;
using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;
using LinqKit;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Business.Services
{
    public class MarketService : ServiceBase, IMarketService
    {
        private readonly IMongoRepository<Market> _marketRepository;

        public MarketService(IHttpContextAccessor httpAccessor,
            IMongoRepository<Market> marketRepository) : base(httpAccessor)
        {
            _marketRepository = marketRepository;
        }

        public async Task<ServiceResponse<Market>> AddToMarket(Files file)
        {
            var response = new ServiceResponse<Market>();

            try
            {
                var res = await _marketRepository.InsertOneAsync(new Market()
                {
                    UserId = file.UserId,
                    CVId = ObjectId.Parse(file.Id)
                });

                if (res.Code != 200)
                {
                    response.Code = res.Code;
                    response.Message = res.Message;
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse> ReviewCv(ReviewCvRequest request)
        {
            var response = new ServiceResponse();

            try
            {
                if (string.IsNullOrEmpty(request.MarketId))
                {
                    response.Code = 500;
                    response.Message = "MarketId required field";
                    goto exit;
                }

                if (request.StarCount < 1 || request.StarCount > 5)
                {
                    response.Code = 500;
                    response.Message = "StarCount value must be between 1 and 5";
                    goto exit;
                }

                var resMarketInsert = await _marketRepository.FindByIdAsync(request.MarketId);

                if (resMarketInsert.Code != 200)
                {
                    response.Code = resMarketInsert.Code;
                    response.Message = resMarketInsert.Message;
                    goto exit;
                }

                resMarketInsert.Result.StarCount += request.StarCount;
                resMarketInsert.Result.ReviewCount += 1;
                resMarketInsert.Result.Rate = resMarketInsert.Result.StarCount / resMarketInsert.Result.ReviewCount;

                if (!string.IsNullOrEmpty(request.Comment))
                {
                    resMarketInsert.Result.Comments.Add(new Comment
                    {
                        UserId = ObjectId.Parse(User.Id),
                        Content = request.Comment
                    });
                }

                var resMarketUpdate = await _marketRepository.UpdateOneAsync(resMarketInsert.Result);

                if (resMarketUpdate.Code != 200)
                {
                    response.Code = resMarketUpdate.Code;
                    response.Message = resMarketUpdate.Message;
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            exit:;
            return response;
        }

        public async Task<ServiceResponse<List<MarketModel>>> GetFromMarket(GetCvRequest request)
        {
            var response = new ServiceResponse<List<MarketModel>>();

            try
            {
                #region [ Filter ]
                var filter = PredicateBuilder.New<Market>(true);

                if (!string.IsNullOrEmpty(request.UserId))
                {
                    filter = filter.And(x => x.UserId == ObjectId.Parse(request.UserId));
                }

                if (!string.IsNullOrEmpty(request.CVId))
                {
                    filter = filter.And(x => x.CVId == ObjectId.Parse(request.CVId));
                }

                if (request.Rate.HasValue)
                {
                    filter = filter.And(x => x.Rate == request.Rate.Value);
                }
                #endregion

                var res = await _marketRepository.FindManyAsync(filter);

                if (res.Code != 200)
                {
                    response.Code = res.Code;
                    response.Message = res.Message;
                    goto exit;
                }

                var mapped = AutoMapper.Map<List<MarketModel>>(res.Result);
                response.Result = mapped;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            exit:;
            return response;
        }
    }
}
