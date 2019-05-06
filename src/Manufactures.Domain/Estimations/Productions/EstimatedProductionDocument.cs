using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.Events;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
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
        public UnitId UnitId { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public IReadOnlyCollection<EstimationProduct> EstimationProducts { get; private set; }

        public EstimatedProductionDocument(Guid id,
                                           string estimatedNumber,
                                           Period period,
                                           UnitId unitId) : base(id)
        {
            Identity = id;
            EstimatedNumber = estimatedNumber;
            Period = period;
            UnitId = unitId;
            EstimationProducts = new List<EstimationProduct>();

            this.MarkTransient();

            ReadModel = new EstimatedProductionDocumentReadModel(Identity)
            {
                EstimatedNumber = this.EstimatedNumber,
                Period = this.Period.Serialize(),
                UnitId = this.UnitId.Value,
                EstimationProducts = this.EstimationProducts.ToList()
            };

            ReadModel.AddDomainEvent(new OnAddEstimation(Identity));
        }

        public EstimatedProductionDocument(EstimatedProductionDocumentReadModel readModel) : base(readModel)
        {
            this.EstimatedNumber = readModel.EstimatedNumber;
            this.Period = readModel.Period.Deserialize<Period>();
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
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

                MarkModified();
            }
            else
            {
                Validator.ErrorValidation(("Order Number On Estimation Production ", estimationProduct.OrderDocument
                                                                                                      .Deserialize<OrderDocumentValueObject>()
                                                                                                      .OrderNumber + " has available"));
            }
        }

        protected override EstimatedProductionDocument GetEntity()
        {
            return this;
        }
    }
}
