using CVMarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Core.Requests
{
    public class GetCvRequest : IRequestBase
    {
        public string UserId { get; set; }
        public string CVId { get; set; }
        public int? Rate { get; set; }
    }
}
