using Manufactures.Domain.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Orders.DataTransferObjects
{
    public class OrderByIdDto : OrderListDto
    {
        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public Guid ConstructionDocumentId { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public int UnitId { get; private set; }

        [JsonProperty(PropertyName = "WarpOriginId")]
        public Guid WarpOriginId { get; private set; }

        [JsonProperty(PropertyName = "WeftOriginId")]
        public Guid WeftOriginId { get; private set; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; }

        [JsonProperty(PropertyName = "AllGrade")]
        public double AllGrade { get; }

        public OrderByIdDto(OrderDocument orderDocument) : base(orderDocument)
        {
            ConstructionDocumentId = orderDocument.ConstructionDocumentId.Value;
            UnitId = orderDocument.UnitId.Value;
            WarpOriginId = orderDocument.WarpOriginId.Value;
            WeftOriginId = orderDocument.WeftOriginId.Value;
            YarnType = orderDocument.YarnType;
            AllGrade = orderDocument.AllGrade;
        }
    }
}
