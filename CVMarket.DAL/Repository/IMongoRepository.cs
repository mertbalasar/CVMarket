using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CVMarket.Core.Responses;

namespace CVMarket.DAL.Repository
{
    public interface IMongoRepository<TCollection>
    {
        ServiceResponse<IAggregateFluent<TCollection>> Aggregate(AggregateOptions options = null);
        Task<ServiceResponse<TCollection>> InsertOneAsync(TCollection record);
        Task<ServiceResponse<List<TCollection>>> InsertManyAsync(List<TCollection> records);
        Task<ServiceResponse<TCollection>> FindByIdAsync(string id);
        Task<ServiceResponse<List<TCollection>>> FindManyAsync(Expression<Func<TCollection, bool>> filter);
        Task<ServiceResponse<TCollection>> DeleteByIdAsync(string id);
        Task<ServiceResponse<DeleteResult>> DeleteManyAsync(Expression<Func<TCollection, bool>> filter);
        Task<ServiceResponse<ReplaceOneResult>> UpdateOneAsync(TCollection record);
    }
}
