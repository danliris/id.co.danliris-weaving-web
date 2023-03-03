using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Orders.DataTransferObjects
{
    public class OrderByUnitAndPeriodDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Period")]
        public DateTime Period { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "TotalGram")]
        public double TotalGram { get; }

        [JsonProperty(PropertyName = "TotalOrder")]
        public double TotalOrder { get; }

        public OrderByUnitAndPeriodDto(OrderDocument orderDocument, FabricConstructionDocument constructionDocument)
        {
            Id = orderDocument.Identity;
            Period = orderDocument.Period;
            OrderNumber = orderDocument.OrderNumber;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalGram = constructionDocument.TotalYarn;
            TotalOrder = orderDocument.AllGrade;
        }
    }
}
