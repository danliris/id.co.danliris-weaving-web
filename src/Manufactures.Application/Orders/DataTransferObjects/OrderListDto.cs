using Manufactures.Domain.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Orders.DataTransferObjects
{
    public class OrderListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "Period")]
        public DateTime Period { get; }

        [JsonProperty(PropertyName = "Unit")]
        public string Unit { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "WarpCompositionPoly")]
        public double WarpCompositionPoly { get; }

        [JsonProperty(PropertyName = "WarpCompositionCotton")]
        public double WarpCompositionCotton { get; }

        [JsonProperty(PropertyName = "WarpCompositionOthers")]
        public double WarpCompositionOthers { get; }

        [JsonProperty(PropertyName = "WeftCompositionPoly")]
        public double WeftCompositionPoly { get; }

        [JsonProperty(PropertyName = "WeftCompositionCotton")]
        public double WeftCompositionCotton { get; }

        [JsonProperty(PropertyName = "WeftCompositionOthers")]
        public double WeftCompositionOthers { get; }

        public OrderListDto(OrderDocument orderDocument, string unit, string constructionNumber)
        {
            Id = orderDocument.Identity;
            OrderNumber = orderDocument.OrderNumber;
            Period = orderDocument.AuditTrail.CreatedDate.DateTime;
            Unit = unit;
            ConstructionNumber = constructionNumber;
            WarpCompositionPoly = orderDocument.WarpCompositionPoly;
            WarpCompositionCotton = orderDocument.WarpCompositionCotton;
            WarpCompositionOthers = orderDocument.WarpCompositionOthers;
            WeftCompositionPoly = orderDocument.WeftCompositionPoly;
            WeftCompositionCotton = orderDocument.WeftCompositionCotton;
            WeftCompositionOthers = orderDocument.WeftCompositionOthers;
        }
    }
}
