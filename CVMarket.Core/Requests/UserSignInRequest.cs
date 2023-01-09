using CVMarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Core.Requests
{
    public class UserSignInRequest : IRequestBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
