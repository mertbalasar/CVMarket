using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using CVMarket.Business.Base;
using CVMarket.Business.Interfaces;
using CVMarket.Core.Responses;
using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;

namespace CVMarket.Business.Services
{
    public class HomeService : ServiceBase, IHomeService
    {
        public HomeService(IHttpContextAccessor httpAccessor) : base(httpAccessor)
        {
            
        }

        public ServiceResponse<string> DisplayWelcome()
        {
            try
            {
                return new ServiceResponse<string>()
                {
                    Result = "Welcome To CVMarket"
                };
            } 
            catch (Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Result = null,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }
    }
}
