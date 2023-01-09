using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Requests;
using CVMarket.Core.Responses;
using CVMarket.Entities.Models;

namespace CVMarket.Business.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<UserModel>> SignUp(UserSignUpRequest request);
        Task<ServiceResponse<UserModel>> SignIn(UserSignInRequest request);
        Task<ServiceResponse> LogOut();
        Task<ServiceResponse> DeleteUser(string userId);
    }
}
