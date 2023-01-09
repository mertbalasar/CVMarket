using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVMarket.Business.Models.Configures;
using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;

namespace CVMarket.API.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _request;

        public UserMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext httpContext, IMongoRepository<User> userRepository)
        {
            AppSettings appSettings = httpContext.RequestServices.GetService(typeof(AppSettings)) as AppSettings;
            string token = httpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(token))
            {
                var filter = PredicateBuilder.New<User>(true);
                filter = filter.And(x => x.Token.Equals(token));

                var users = await userRepository.FindManyAsync(filter);
                if (users.Code == 200 && users.Result != null && users.Result.Count == 1)
                {
                    httpContext.Items["SessionUser"] = users.Result.FirstOrDefault();
                }
            }
 
            await _request(httpContext);
        }
    }
}
