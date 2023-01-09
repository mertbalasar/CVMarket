using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using CVMarket.Business.Models.Configures;
using CVMarket.Entities.Models;

namespace CVMarket.Business.Base
{
    public abstract class ServiceBase
    {
        private readonly IHttpContextAccessor _httpAccessor;

        protected ServiceBase(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }

        protected User User { get => (User)_httpAccessor.HttpContext.Items["SessionUser"]; }
        protected IMapper AutoMapper { get => (IMapper)_httpAccessor.HttpContext.RequestServices.GetService(typeof(IMapper)); }
        protected AppSettings AppSettings { get => (AppSettings)_httpAccessor.HttpContext.RequestServices.GetService(typeof(AppSettings)); }
        protected VersionInfo Version { get => (VersionInfo)_httpAccessor.HttpContext.RequestServices.GetService(typeof(VersionInfo)); }
    }
}
