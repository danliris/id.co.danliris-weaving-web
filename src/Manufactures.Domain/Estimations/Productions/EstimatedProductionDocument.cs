using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions
{
    public class EstimatedProductionDocument : AggregateRoot<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>
    {
        public string EstimatedNumber { get; private set; }
        public DateTime Period { get; private set; }
        public UnitId UnitId { get; private set; }
        public List<EstimatedProductionDetail> EstimationProducts { get; private set; }

        public EstimatedProductionDocument(Guid identity,
                                           string estimatedNumber,
                                           DateTime period,
                                           UnitId unitId) : base(identity)
        {
            Identity = identity;
            EstimatedNumber = estimatedNumber;
            Period = period;
            UnitId = unitId;

            this.MarkTransient();

            ReadModel = new EstimatedProductionDocumentReadModel(Identity)
            {
                EstimatedNumber = this.EstimatedNumber,
                Period = this.Period,
                UnitId = this.UnitId.Value,
            };

            ReadModel.AddDomainEvent(new OnAddEstimation(Identity));
        }

        public EstimatedProductionDocument(EstimatedProductionDocumentReadModel readModel) : base(readModel)
        {
            EstimatedNumber = readModel.EstimatedNumber;
            Period = readModel.Period;
            UnitId = new UnitId(readModel.UnitId);
        }

        public void SetEstimatedNumber(string estimatedNumber)
        {
            Validator.ThrowIfNull(() => estimatedNumber);

            if (estimatedNumber != EstimatedNumber)
            {
                EstimatedNumber = estimatedNumber;
                ReadModel.EstimatedNumber = EstimatedNumber;

                MarkModified();
            }
        }

        public void SetPeriod(DateTime period)
        {

            if (period != Period)
            {

                Period = period;
                ReadModel.Period = Period;

                MarkModified();
            }
        }

        public void SetUnit(UnitId unit)
        {
            Validator.ThrowIfNull(() => unit);

            if (unit != UnitId)
            {

                UnitId = unit;
                ReadModel.UnitId = UnitId.Value;

                MarkModified();
            }
        }

        //public void AddEstimationProduct(EstimatedProductionDetail estimationProduct)
        //{
        //    if (!EstimationProducts.Any(product => product.OrderDocument.Deserialize<OrderDocumentValueObject>().OrderNumber
        //                                                  .Equals(estimationProduct.OrderDocument
        //                                                                           .Deserialize<OrderDocumentValueObject>()
        //                                                                           .OrderNumber)))
        //    {
        //        var list = EstimationProducts.ToList();
        //        list.Add(estimationProduct);
        //        EstimationProducts = list;
        //        ReadModel.EstimationProducts = EstimationProducts.ToList();

        //        MarkModified();
        //    }
        //    else
        //    {
        //        Validator.ErrorValidation(("Order Number On Estimation Production ", estimationProduct.OrderDocument
        //                                                                                              .Deserialize<OrderDocumentValueObject>()
        //                                                                                              .OrderNumber + " has available"));
        //    }
        //}
        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override EstimatedProductionDocument GetEntity()
        {
            return this;
        }
    }
}
