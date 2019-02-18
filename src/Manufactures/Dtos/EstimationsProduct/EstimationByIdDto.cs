using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos.EstimationsProduct
{
    public class EstimationByIdDto
    {
        public Guid Id { get; }
        public string EstimatedNumber { get; }
        public Period Period { get; }
        public WeavingUnit Unit { get; }
        public List<EstimationProductValueObject> EstimationProducts { get; }

        public EstimationByIdDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimatedNumber = document.EstimatedNumber;
            Period = document.Period;
            Unit = document.Unit;
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
