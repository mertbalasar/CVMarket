using CVMarket.Business.Base;
using CVMarket.Business.Interfaces;
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Responses;
using CVMarket.DAL.Repository;
using CVMarket.Entities.LookUpModels;
using CVMarket.Entities.Models;
using LinqKit;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Business.Services
{
    public class UploadService : ServiceBase, IUploadService
    {
        private readonly IMongoRepository<Files> _filesRepository;

        public UploadService(IHttpContextAccessor httpAccessor,
            IMongoRepository<Files> filesRepository) : base(httpAccessor)
        {
            _filesRepository = filesRepository;
        }

        public async Task<ServiceResponse> UploadFile(IFormFile file)
        {
            var response = new ServiceResponse { };

            try
            {
                var path = "C:/Users/" + Environment.UserName + "/CVMarketFiles";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filePath = path + "/" + DateTime.UtcNow.Ticks.ToString() + "_" + file.FileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var res = await _filesRepository.InsertOneAsync(new Files
                {
                    UserId = ObjectId.Parse(User.Id),
                    FilePath = filePath
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

        public ServiceResponse<List<FilesModel>> GetFiles()
        {
            var response = new ServiceResponse<List<FilesModel>>();

            try
            {
                var filter = PredicateBuilder.New<FilesLookUp>(true);
                filter = filter.And(x => x.UserId == ObjectId.Parse(User.Id));
                var match = Builders<FilesLookUp>.Filter.Where(filter);

                var aggregate = _filesRepository.Aggregate();
                if (aggregate.Code != 200)
                {
                    response.Code = aggregate.Code;
                    response.Message = aggregate.Message;
                    goto exit;
                }

                AggregateUnwindOptions<FilesLookUp> unwindOptions = new AggregateUnwindOptions<FilesLookUp>() { PreserveNullAndEmptyArrays = true };
                var projection = Builders<FilesLookUp>.Projection
                            .Exclude("User.Token")
                            .Exclude("User.TokenExpireAt");

                var files = aggregate.Result
                    .Lookup<Files, FilesLookUp>("user", "UserId", "_id", "User")
                    .Project<FilesLookUp>(projection)
                    .Unwind(x => x.User, unwindOptions)
                    .As<FilesLookUp>()
                    .Match(match)
                    .ToList();

                var mapped = AutoMapper.Map<List<FilesModel>>(files);
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
