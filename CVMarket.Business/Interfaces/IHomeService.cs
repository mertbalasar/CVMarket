using System;
using System.Collections.Generic;
using System.Text;
using CVMarket.Business.Base;
using CVMarket.Core.Responses;

namespace CVMarket.Business.Interfaces
{
    public interface IHomeService
    {
        ServiceResponse<string> DisplayWelcome();
    }
}
