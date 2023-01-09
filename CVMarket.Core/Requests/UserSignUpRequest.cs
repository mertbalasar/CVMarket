using CVMarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static CVMarket.Core.Enums.EnumLibrary;

namespace CVMarket.Core.Requests
{
    public class UserSignUpRequest : IRequestBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public UserType Type { get; set; }
    }
}
