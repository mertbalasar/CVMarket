using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Core.Responses
{
    public class ServiceResponse
    {
        public int Code { get; set; } = StatusCodes.Status200OK;
        public string Message { get; set; } = string.Empty;
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Result { get; set; }
    }
}
