using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.DataTransferObjects.EstimationsProduct
{
    public class EstimationByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "EstimatedNumber")]
        public string EstimatedNumber { get; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; }

        [JsonProperty(PropertyName = "Unit")]
        public UnitId Unit { get; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimationProductValueObject> EstimationProducts { get; }

        public EstimationByIdDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimatedNumber = document.EstimatedNumber;
            Period = document.Period;
            Unit = document.UnitId;
            EstimationProducts = new List<EstimationProductValueObject>();

            foreach (var product in document.EstimationProducts)
            {
                var order = product.OrderDocument.Deserialize<OrderDocumentValueObject>();
                var productGrade = product.ProductGrade.Deserialize<ProductGrade>();
                var obj = new EstimationProductValueObject(order.Construction.ConstructionNumber,
                                                           order.Construction.TotalYarn,
                                                           order.DateOrdered,
                                                           order.OrderNumber,
                                                           order.AllGrade,
                                                           product.TotalGramEstimation,
                                                           productGrade.GradeA,
                                                           productGrade.GradeB,
                                                           productGrade.GradeC,
                                                           productGrade.GradeD);

                EstimationProducts.Add(obj);
            }
        }
    }
}
