using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Core.Requests
{
    public class GetCvRequest
    {
        public string UserId { get; set; }
        public string CVId { get; set; }
        public int? Rate { get; set; }
    }
}
