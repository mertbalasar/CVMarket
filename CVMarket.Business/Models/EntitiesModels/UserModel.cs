using System;
using System.Collections.Generic;
using System.Text;
using static CVMarket.Core.Enums.EnumLibrary;

namespace CVMarket.Business.Models.EntitiesModels
{
    public class UserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpireAt { get; set; }
        public DateTime? BirthDay { get; set; }
        public UserType Type { get; set; }
    }
}
