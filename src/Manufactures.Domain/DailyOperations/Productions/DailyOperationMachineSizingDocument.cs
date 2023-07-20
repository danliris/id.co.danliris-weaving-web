using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Productions
{
    public class DailyOperationMachineSizingDocument : AggregateRoot<DailyOperationMachineSizingDocument, DailyOperationMachineSizingDocumentReadModel>
    {
        public string EstimatedNumber { get; private set; }
        public DateTime Period { get; private set; }
        public UnitId UnitId { get; private set; }
        public List<DailyOperationMachineSizingDetail> EstimationProducts { get; private set; }

        public DailyOperationMachineSizingDocument(Guid identity,
                                           string estimatedNumber,
                                           DateTime period,
                                           UnitId unitId) : base(identity)
        {
            Identity = identity;
            EstimatedNumber = estimatedNumber;
            Period = period;
            UnitId = unitId;

            this.MarkTransient();

            ReadModel = new DailyOperationMachineSizingDocumentReadModel(Identity)
            {
                EstimatedNumber = this.EstimatedNumber,
                Period = this.Period,
                UnitId = this.UnitId.Value,
            };

            ReadModel.AddDomainEvent(new OnAddEstimation(Identity));
        }

        public DailyOperationMachineSizingDocument(DailyOperationMachineSizingDocumentReadModel readModel) : base(readModel)
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

        protected override DailyOperationMachineSizingDocument GetEntity()
        {
            return this;
        }
    }
}
