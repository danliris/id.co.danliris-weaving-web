using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
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
        public List<EstimationProductValueObject> EstimationItems { get; }

        public EstimationByIdDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimatedNumber = document.EstimatedNumber;
            Period = document.Period;
            Unit = document.Unit;

            foreach(var product in document.EstimationProducts)
            {
                var obj = new EstimationProductValueObject();
                var order = product.OrderDocument.Deserialize<OrderDocumentValueObject>();
                var productGrade = product.ProductGrade.Deserialize<ProductGrade>();

                obj.SetConstructionNumber(order.Construction.ConstructionNumber);
                obj.SetAmountTotal(order.Construction.TotalYarn);
                obj.SetDateOrdered(order.DateOrdered);
                obj.SetOrderNumber(order.OrderNumber);
                obj.SetOrderTotal(order.AllGrade);
                obj.SetGradeA(productGrade.GradeA);
                obj.SetGradeB(productGrade.GradeB);
                obj.SetGradeC(productGrade.GradeC);
                obj.SetGradeD(productGrade.GradeD);

                EstimationItems.Add(obj);
            }
        }
    }
}
