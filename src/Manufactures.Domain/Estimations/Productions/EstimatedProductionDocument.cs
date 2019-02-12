using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.Estimations.Productions
{
    public class EstimatedProductionDocument : AggregateRoot<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>
    {
        public string EstimatedNumber { get; private set; }
        public Period Period { get; private set; }
        public WeavingUnit Unit { get; private set; }
        public double TotalEstimationOrder { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public IReadOnlyCollection<EstimationProduct> EstimationProducts { get; private set; }

        public EstimatedProductionDocument(Guid id,
                                           string estimatedNumber,
                                           Period period,
                                           WeavingUnit unit,
                                           double totalEstimationOrder) : base(id)
        {
            Validator.ThrowIfNullOrEmpty(() => estimatedNumber);
            Validator.ThrowIfNull(() => period);
            Validator.ThrowIfNull(() => unit);

            Identity = id;
            EstimatedNumber = estimatedNumber;
            Period = period;
            Unit = unit;
            TotalEstimationOrder = totalEstimationOrder;
            EstimationProducts = new List<EstimationProduct>();

            ReadModel = new EstimatedProductionDocumentReadModel(Identity)
            {
                EstimatedNumber = this.EstimatedNumber,
                Period = this.Period.Serialize(),
                Unit = this.Unit.Serialize(),
                TotalEstimationOrder = this.TotalEstimationOrder,
                EstimationProducts = this.EstimationProducts.ToList()
            };
        }

        public EstimatedProductionDocument(EstimatedProductionDocumentReadModel readModel) : base(readModel)
        {
            this.EstimatedNumber = readModel.EstimatedNumber;
            this.Period = readModel.Period.Deserialize<Period>();
            this.Unit = readModel.Unit.Deserialize<WeavingUnit>();
            this.TotalEstimationOrder = readModel.TotalEstimationOrder;
            this.EstimationProducts = readModel.EstimationProducts;
            this.Date = readModel.CreatedDate;
        }

        public void AddEstimationProduct(EstimationProduct estimationProduct)
        {
            if (!EstimationProducts.Any(product => product.OrderDocument.Deserialize<OrderDocumentValueObject>().OrderNumber
                                                          .Equals(estimationProduct.OrderDocument
                                                                                   .Deserialize<OrderDocumentValueObject>()
                                                                                   .OrderNumber)))
            {
                var list = EstimationProducts.ToList();
                list.Add(estimationProduct);
                EstimationProducts = list;
                ReadModel.EstimationProducts = EstimationProducts.ToList();
            }
            else
            {
                Validator.ErrorValidation(("Order Number On Estimation Production ", estimationProduct.OrderDocument
                                                                                                      .Deserialize<OrderDocumentValueObject>()
                                                                                                      .OrderNumber + " has available"));
            }
        }

        public void SetTotalEstimationOrder(double value)
        {
            if(TotalEstimationOrder != value)
            {
                TotalEstimationOrder = value;
                ReadModel.TotalEstimationOrder = TotalEstimationOrder;

                MarkModified();
            }
        }

        protected override EstimatedProductionDocument GetEntity()
        {
            return this;
        }
    }
}
