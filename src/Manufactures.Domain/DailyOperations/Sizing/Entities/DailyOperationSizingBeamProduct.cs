using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingBeamProduct : EntityBase<DailyOperationSizingBeamProduct>
    {
        public Guid SizingBeamId { get; private set; }
        
        public double? CounterStart { get; private set; }

        public double? CounterFinish { get; private set; }

        public double? WeightNetto { get; private set; }

        public double? WeightBruto { get; private set; }

        public double? WeightTheoritical { get; private set; }

        public double? PISMeter { get; private set; }

        public double? SPU { get; private set; }

        public string BeamStatus { get; private set; }

        public DateTimeOffset LatestDateTimeBeamProduct { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingBeamProduct(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingBeamProduct(Guid identity,
                                               BeamId sizingBeamId,
                                               DateTimeOffset latestDateTimeBeamProduct,
                                               double counterStart,
                                               double counterFinish,
                                               double weightNetto,
                                               double weightBruto,
                                               double weightTheoritical,
                                               double pisMeter,
                                               double spu,
                                               string beamStatus) : base(identity)
        {
            SizingBeamId = sizingBeamId.Value;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            CounterStart = counterStart;
            CounterFinish = counterFinish;
            WeightNetto = weightNetto;
            WeightBruto = weightBruto;
            WeightTheoritical = weightTheoritical;
            PISMeter = pisMeter;
            SPU = spu;
            BeamStatus = beamStatus;
        }

        public void SetSizingBeamId(Guid sizingBeamId)
        {
            if (!SizingBeamId.Equals(sizingBeamId))
            {
                SizingBeamId = sizingBeamId;

                MarkModified();
            }
        }

        public void SetLatestDateTimeBeamProduct(DateTimeOffset latestDateTimeBeamProduct)
        {
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            MarkModified();
        }

        public void SetCounterStart(double counterStart)
        {
            CounterStart = counterStart;
            MarkModified();
        }

        public void SetCounterFinish(double counterFinish)
        {
            CounterFinish = counterFinish;
            MarkModified();
        }

        public void SetWeightNetto(double weightNetto)
        {
            WeightNetto = weightNetto;
            MarkModified();
        }

        public void SetWeightBruto(double weightBruto)
        {
            WeightBruto = weightBruto;
            MarkModified();
        }

        public void SetWeightTheoritical(double weightTheoritical)
        {
            WeightTheoritical = weightTheoritical;
            MarkModified();
        }

        public void SetPISMeter(double pisMeter)
        {
            PISMeter = pisMeter;
            MarkModified();
        }

        public void SetSPU(double spu)
        {
            SPU = spu;
            MarkModified();
        }

        public void SetSizingBeamStatus(string beamStatus)
        {
            BeamStatus = beamStatus;
            MarkModified();
        }

        protected override DailyOperationSizingBeamProduct GetEntity()
        {
            return this;
        }
    }
}