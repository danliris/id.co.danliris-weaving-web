using Infrastructure.Domain;
using Infrastructure.Domain.Events;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingBeamProduct : AggregateRoot<DailyOperationSizingBeamProduct, DailyOperationSizingBeamProductReadModel>
    {
        public BeamId SizingBeamId { get; private set; }
        public double CounterStart { get; private set; }
        public double CounterFinish { get; private set; }
        public double WeightNetto { get; private set; }
        public double WeightBruto { get; private set; }
        public double WeightTheoritical { get; private set; }
        public double PISMeter { get; private set; }
        public double SPU { get; private set; }
        public string BeamStatus { get; private set; }
        public DateTimeOffset LatestDateTimeBeamProduct { get; private set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingBeamProduct(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingBeamProduct(Guid identity,
                                               BeamId sizingBeamId,
                                               double counterStart,
                                               //double counterFinish,
                                               //double weightNetto,
                                               //double weightBruto,
                                               //double weightTheoritical,
                                               //double pisMeter,
                                               //double spu,
                                               string beamStatus,
                                               DateTimeOffset latestDateTimeBeamProduct,
                                               Guid dailyOperationSizingDocumentId) : base(identity)
        {
            Identity = identity;
            SizingBeamId = sizingBeamId;
            CounterStart = counterStart;
            //CounterFinish = counterFinish;
            //WeightNetto = weightNetto;
            //WeightBruto = weightBruto;
            //WeightTheoritical = weightTheoritical;
            //PISMeter = pisMeter;
            //SPU = spu;
            BeamStatus = beamStatus;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            DailyOperationSizingDocumentId = dailyOperationSizingDocumentId;

            MarkTransient();

            ReadModel = new DailyOperationSizingBeamProductReadModel(Identity)
            {

                SizingBeamId = SizingBeamId.Value,
                CounterStart = CounterStart,
                CounterFinish = CounterFinish,
                WeightNetto = WeightNetto,
                WeightBruto = WeightBruto,
                WeightTheoritical = WeightTheoritical,
                PISMeter = PISMeter,
                SPU = SPU,
                BeamStatus = BeamStatus,
                LatestDateTimeBeamProduct = LatestDateTimeBeamProduct,
                DailyOperationSizingDocumentId = DailyOperationSizingDocumentId
            };
        }

        public DailyOperationSizingBeamProduct(DailyOperationSizingBeamProductReadModel readModel) : base(readModel)
        {
            SizingBeamId = new BeamId(readModel.SizingBeamId);
            CounterStart = readModel.CounterStart;
            CounterFinish = readModel.CounterFinish;
            WeightNetto = readModel.WeightNetto;
            WeightBruto = readModel.WeightBruto;
            WeightTheoritical = readModel.WeightTheoritical;
            PISMeter = readModel.PISMeter;
            SPU = readModel.SPU;
            BeamStatus = readModel.BeamStatus;
            LatestDateTimeBeamProduct = readModel.LatestDateTimeBeamProduct;
            DailyOperationSizingDocumentId = readModel.DailyOperationSizingDocumentId;
        }

        public void SetSizingBeamId(BeamId sizingBeamId)
        {
            Validator.ThrowIfNull(() => sizingBeamId);
            if (sizingBeamId != SizingBeamId)
            {
                SizingBeamId = sizingBeamId;
                ReadModel.SizingBeamId = SizingBeamId.Value;

                MarkModified();
            }
        }

        public void SetCounterStart(double counterStart)
        {
            if (counterStart != CounterStart)
            {
                CounterStart = counterStart;
                ReadModel.CounterStart = CounterStart;

                MarkModified();
            }
        }

        public void SetCounterFinish(double counterFinish)
        {
            if (counterFinish != CounterFinish)
            {
                CounterFinish = counterFinish;
                ReadModel.CounterFinish = CounterFinish;

                MarkModified();
            }
        }

        public void SetWeightNetto(double weightNetto)
        {
            if (weightNetto != WeightNetto)
            {
                WeightNetto = weightNetto;
                ReadModel.WeightNetto = WeightNetto;

                MarkModified();
            }
        }

        public void SetWeightBruto(double weightBruto)
        {
            if (weightBruto != WeightBruto)
            {
                WeightBruto = weightBruto;
                ReadModel.WeightBruto = WeightBruto;

                MarkModified();
            }
        }

        public void SetWeightTheoritical(double weightTheoritical)
        {
            if (weightTheoritical != WeightTheoritical)
            {
                WeightTheoritical = weightTheoritical;
                ReadModel.WeightTheoritical = WeightTheoritical;

                MarkModified();
            }
        }

        public void SetPISMeter(double pisMeter)
        {
            if (pisMeter != PISMeter)
            {
                PISMeter = pisMeter;
                ReadModel.PISMeter = PISMeter;

                MarkModified();
            }
        }

        public void SetSPU(double spu)
        {
            if (spu != SPU)
            {
                SPU = spu;
                ReadModel.SPU = SPU;

                MarkModified();
            }
        }

        public void SetSizingBeamStatus(string beamStatus)
        {
            Validator.ThrowIfNull(() => beamStatus);
            if (beamStatus != BeamStatus)
            {
                BeamStatus = beamStatus;
                ReadModel.BeamStatus = BeamStatus;

                MarkModified();
            }
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset latestDateTimeBeamProduct)
        {
            if (latestDateTimeBeamProduct != LatestDateTimeBeamProduct)
            {
                LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
                ReadModel.LatestDateTimeBeamProduct = LatestDateTimeBeamProduct;

                MarkModified();
            }
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationSizingBeamProduct GetEntity()
        {
            return this;
        }

        //protected override void MarkRemoved()
        //{
        //    DeletedBy = "System";
        //    Deleted = true;
        //    DeletedDate = DateTimeOffset.UtcNow;

        //    if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<DailyOperationSizingBeamProduct>))
        //        this.AddDomainEvent(new OnEntityDeleted<DailyOperationSizingBeamProduct>(GetEntity()));

        //    // clear updated events
        //    if (this.DomainEvents.Any(o => o is OnEntityUpdated<DailyOperationSizingBeamProduct>))
        //    {
        //        this.DomainEvents.Where(o => o is OnEntityUpdated<DailyOperationSizingBeamProduct>)
        //            .ToList()
        //            .ForEach(o => this.RemoveDomainEvent(o));
        //    }
        //}
    }
}