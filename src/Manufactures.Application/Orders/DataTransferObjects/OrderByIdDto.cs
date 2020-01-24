using Manufactures.Domain.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Orders.DataTransferObjects
{
    public class OrderByIdDto : OrderListDto
    {
        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; }

        [JsonProperty(PropertyName = "AllGrade")]
        public double AllGrade { get; }

        public OrderByIdDto(OrderDocument orderDocument, string warpOrigin, string weftOrigin) : base(orderDocument, warpOrigin, weftOrigin)
        {
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            YarnType = orderDocument.YarnType;
            AllGrade = orderDocument.AllGrade;
        }
    }
}
