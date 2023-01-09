using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Core.Requests
{
    public class ReviewCvRequest
    {
        public string MarketId { get; set; }
        public int StarCount { get; set; }
        public string Comment { get; set; } = null;
    }
}
