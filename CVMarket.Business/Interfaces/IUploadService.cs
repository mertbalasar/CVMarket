using CVMarket.Business.Models.EntitiesModels;
using CVMarket.Core.Responses;
using CVMarket.Entities.LookUpModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Business.Interfaces
{
    public interface IUploadService
    {
        Task<ServiceResponse> UploadFile(IFormFile file);
        ServiceResponse<List<FilesModel>> GetFiles();
    }
}
