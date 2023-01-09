using LinqKit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CVMarket.Core.Responses;
using CVMarket.DAL.Models.Configures;
using CVMarket.Entities.Attributes;
using CVMarket.Entities.Base;

namespace CVMarket.DAL.Repository
{
    public class MongoRepository<TCollection> : IMongoRepository<TCollection> where TCollection : ICollectionBase
    {
        private IMongoCollection<TCollection> _mongoCollection;

        public MongoRepository(MongoSettings mongoSettings)
        {
            var connection = new MongoClient(mongoSettings.ConnectionString);
            var database = connection.GetDatabase(mongoSettings.DatabaseName);

            var collectionAttribute = typeof(TCollection).GetCustomAttributes(false).Where(e => e.GetType() == typeof(CollectionNameAttribute)).FirstOrDefault() as CollectionNameAttribute;
            if (collectionAttribute != null)
            {
                _mongoCollection = database.GetCollection<TCollection>(collectionAttribute.CollectionName);
            }
        }

        #region [ Processes ]
        public ServiceResponse<IAggregateFluent<TCollection>> Aggregate(AggregateOptions options = null)
        {
            var response = new ServiceResponse<IAggregateFluent<TCollection>>();

            try
            {
                var aggregate = _mongoCollection.Aggregate(options);
                response.Result = aggregate;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<TCollection>> InsertOneAsync(TCollection record)
        {
            var response = new ServiceResponse<TCollection>();

            try
            {
                await _mongoCollection.InsertOneAsync(record);
                response.Result = record;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<TCollection>>> InsertManyAsync(List<TCollection> records)
        {
            var response = new ServiceResponse<List<TCollection>>();

            try
            {
                await _mongoCollection.InsertManyAsync(records);
                response.Result = records;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<TCollection>> FindByIdAsync(string id)
        {
            var response = new ServiceResponse<TCollection>();

            try
            {
                var filter = PredicateBuilder.New<TCollection>(true);
                filter = filter.And(x => x.Id.Equals(id));

                var res = await _mongoCollection.FindAsync(filter);
                var ent = res.ToList().FirstOrDefault();
                if (ent != null)
                {
                    response.Result = ent;
                }
                else
                {
                    response.Code = 404;
                    response.Message = "The given id can not found";
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<TCollection>>> FindManyAsync(Expression<Func<TCollection, bool>> filter)
        {
            var response = new ServiceResponse<List<TCollection>>();

            try
            {
                var res = await _mongoCollection.FindAsync(filter);
                response.Result = res.ToList();
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<TCollection>> DeleteByIdAsync(string id)
        {
            var response = new ServiceResponse<TCollection>();

            try
            {
                var filter = PredicateBuilder.New<TCollection>(true);
                filter = filter.And(x => x.Id.Equals(id));

                var res = await _mongoCollection.FindOneAndDeleteAsync(filter);
                if (res != null)
                {
                    response.Result = res;
                }
                else
                {
                    response.Code = 404;
                    response.Message = "The given id can not found";
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<DeleteResult>> DeleteManyAsync(Expression<Func<TCollection, bool>> filter)
        {
            var response = new ServiceResponse<DeleteResult>();

            try
            {
                var res = await _mongoCollection.DeleteManyAsync(filter);
                if (res.IsAcknowledged)
                {
                    response.Result = res;
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Delete request failed";
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<ReplaceOneResult>> UpdateOneAsync(TCollection record)
        {
            var response = new ServiceResponse<ReplaceOneResult>();

            try
            {
                var filter = PredicateBuilder.New<TCollection>(true);
                filter = filter.And(x => x.Id.Equals(record.Id));

                var res = await _mongoCollection.ReplaceOneAsync(filter, record);
                if (res.IsAcknowledged)
                {
                    response.Result = res;
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Update request failed";
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }
        #endregion
    }
}
