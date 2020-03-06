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
        public string Unit { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "WarpOriginId")]
        public Guid WarpOriginId { get; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; }

        [JsonProperty(PropertyName = "WarpCompositionPoly")]
        public double WarpCompositionPoly { get; }

        [JsonProperty(PropertyName = "WarpCompositionCotton")]
        public double WarpCompositionCotton { get; }

        [JsonProperty(PropertyName = "WarpCompositionOthers")]
        public double WarpCompositionOthers { get; }

        [JsonProperty(PropertyName = "WeftOriginId")]
        public Guid WeftOriginId { get; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; }

        [JsonProperty(PropertyName = "WeftCompositionPoly")]
        public double WeftCompositionPoly { get; }

        [JsonProperty(PropertyName = "WeftCompositionCotton")]
        public double WeftCompositionCotton { get; }

        [JsonProperty(PropertyName = "WeftCompositionOthers")]
        public double WeftCompositionOthers { get; }

        public OrderListDto(OrderDocument orderDocument, string warpOrigin, string weftOrigin)
        {
            Id = orderDocument.Identity;
            OrderNumber = orderDocument.OrderNumber;
            Period = orderDocument.Period;
            WarpOriginId = orderDocument.WarpOriginIdOne.Value;
            WarpOrigin = warpOrigin;
            WarpCompositionPoly = orderDocument.WarpCompositionPoly;
            WarpCompositionCotton = orderDocument.WarpCompositionCotton;
            WarpCompositionOthers = orderDocument.WarpCompositionOthers;
            WeftOriginId = orderDocument.WeftOriginIdOne.Value;
            WeftOrigin = weftOrigin;
            WeftCompositionPoly = orderDocument.WeftCompositionPoly;
            WeftCompositionCotton = orderDocument.WeftCompositionCotton;
            WeftCompositionOthers = orderDocument.WeftCompositionOthers;
        }

        public void SetUnit(string unit)
        {
            Unit = unit;
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            ConstructionNumber = constructionNumber;
        }
    }
}
