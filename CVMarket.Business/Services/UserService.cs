using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CVMarket.Business.Base;
using CVMarket.Business.Interfaces;
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Requests;
using CVMarket.Core.Responses;
using CVMarket.DAL.Repository;
using CVMarket.Entities.Models;
using LinqKit;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CVMarket.Business.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IMongoRepository<User> _userRepository;

        public UserService(IHttpContextAccessor httpAccessor,
            IMongoRepository<User> userRepository) : base(httpAccessor)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<UserModel>> SignUp(UserSignUpRequest request)
        {
            var response = new ServiceResponse<UserModel>();

            try
            {
                var checkUserName = await CheckUserName(request.UserName);
                if (!checkUserName)
                {
                    response.Code = 401;
                    response.Message = "The given username is exist. Please try another username";
                    response.Result = null;
                    goto exit;
                }

                HashPassword(ref request);

                var user = AutoMapper.Map<User>(request);
                var res = await _userRepository.InsertOneAsync(user);

                if (res.Code == 200)
                {
                    response.Result = AutoMapper.Map<UserModel>(res.Result);
                }
                else
                {
                    response.Result = null;
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

        public async Task<ServiceResponse<UserModel>> SignIn(UserSignInRequest request)
        {
            var response = new ServiceResponse<UserModel>();

            try
            {
                HashPassword(ref request);

                var filter = PredicateBuilder.New<User>(true);
                filter = filter.And(x => x.UserName.Equals(request.UserName) && x.Password.Equals(request.Password));

                var res = await _userRepository.FindManyAsync(filter);

                if (res.Code == 200 && res.Result.Count == 1)
                {
                    var user = res.Result.First();

                    user.Token = GenerateJwtToken(user);
                    user.TokenExpireAt = DateTime.UtcNow.AddDays(1);
                    var updateRes = await _userRepository.UpdateOneAsync(user);

                    if (updateRes.Code != 200)
                    {
                        response.Code = updateRes.Code;
                        response.Message = updateRes.Message;
                        response.Result = null;
                        goto exit;
                    }

                    response.Result = AutoMapper.Map<UserModel>(user);
                }
                else
                {
                    response.Code = 401;
                    response.Message = "Username or password are not correct";
                    response.Result = null;
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

        public async Task<ServiceResponse> LogOut()
        {
            var response = new ServiceResponse();

            try
            {
                if (User != null)
                {
                    User.Token = null;
                    User.TokenExpireAt = null;
                    var updateRes = await _userRepository.UpdateOneAsync(User);

                    if (updateRes.Code != 200)
                    {
                        response.Code = updateRes.Code;
                        response.Message = updateRes.Message;
                    }
                }
                else
                {
                    response.Code = 401;
                    response.Message = "The given User can not found";
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceResponse> DeleteUser(string userId)
        {
            var response = new ServiceResponse();

            try
            {
                var user = await _userRepository.DeleteByIdAsync(userId);

                if (user.Code != 200)
                {
                    response.Code = user.Code;
                    response.Message = user.Message;
                }
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
            }

            return response;
        }

        #region [ Helpers ]
        private async Task<bool> CheckUserName(string userName)
        {
            var filter = PredicateBuilder.New<User>(true);
            filter = filter.And(x => x.UserName.Equals(userName));

            var result = await _userRepository.FindManyAsync(filter);
            return result.Result.Count == 0 ? true : false;
        }

        private void HashPassword(ref UserSignUpRequest request)
        {
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(request.Password));

            var converter = new ASCIIEncoding();
            request.Password = converter.GetString(hash);
        }

        private void HashPassword(ref UserSignInRequest request)
        {
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(request.Password));

            var converter = new ASCIIEncoding();
            request.Password = converter.GetString(hash);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
