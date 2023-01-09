using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Requests;
using CVMarket.Entities.Models;
using CVMarket.Entities.LookUpModels;

namespace CVMarket.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserSignUpRequest>();
            CreateMap<UserSignUpRequest, User>();
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<FilesLookUp, FilesModel>();
            CreateMap<FilesModel, FilesLookUp>();
        }
    }
}
