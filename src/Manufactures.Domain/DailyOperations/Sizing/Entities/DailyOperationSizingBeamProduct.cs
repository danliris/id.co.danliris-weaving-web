using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingBeamProduct : EntityBase<DailyOperationSizingBeamProduct>
    {
        public Guid SizingBeamId { get; private set; }

        public DateTimeOffset DateTimeBeamDocument { get; private set; }

        //public string Counter { get; private set; }
        public double CounterStart { get; private set; }

        public double CounterFinish { get; private set; }

        //public string Weight { get; private set; }
        public double WeightNetto { get; private set; }

        public double WeightBruto { get; private set; }

        public double WeightTheoritical { get; private set; }

        public double PISMeter { get; private set; }

        public double SPU { get; private set; }

        public string SizingBeamStatus { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingBeamProduct(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingBeamProduct(Guid identity,
                                                BeamId sizingBeamId,
                                                DateTimeOffset dateTimeBeamDocument,
                                                //DailyOperationSizingCounterValueObject counter,
                                                double start,
                                                double finish,
                                                //DailyOperationSizingWeightValueObject weight,
                                                double netto,
                                                double bruto,
                                                double theoritical,
                                                double pisMeter,
                                                double spu,
                                                string sizingBeamStatus) : this(identity)
        {
            SizingBeamId = sizingBeamId.Value;
            DateTimeBeamDocument = dateTimeBeamDocument;
            //Counter = counter.Serialize();
            CounterStart = start;
            CounterFinish = finish;
            //Weight = weight.Serialize();
            WeightNetto = netto;
            WeightBruto = bruto;
            WeightTheoritical = theoritical;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
        public DailyOperationSizingBeamProduct(Guid identity,
                                                DateTimeOffset dateTimeBeamDocument,
                                                //DailyOperationSizingCounterValueObject counter,
                                                double start,
                                                double finish,
                                                //DailyOperationSizingWeightValueObject weight,
                                                double netto,
                                                double bruto,
                                                double theoritical,
                                                double pisMeter,
                                                double spu,
                                                string sizingBeamStatus) : this(identity)
        {
            DateTimeBeamDocument = dateTimeBeamDocument;
            //Counter = counter.Serialize();
            CounterStart = start;
            CounterFinish = finish;
            //Weight = weight.Serialize();
            WeightNetto = netto;
            WeightBruto = bruto;
            WeightTheoritical = theoritical;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }

        public void SetSizingBeamId(Guid sizingBeamId)
        {
            SizingBeamId = sizingBeamId;
            MarkModified();
        }

        public void SetDateTimeBeamDocument(DateTimeOffset dateTimeBeamDocument)
        {
            DateTimeBeamDocument = dateTimeBeamDocument;
            MarkModified();
        }

        //public void SetCounter(DailyOperationSizingCounterValueObject counter)
        //{
        //    Counter = counter.Serialize();
        //    MarkModified();
        //}

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

        //public void SetWeight(DailyOperationSizingWeightValueObject weight)
        //{
        //    Weight = weight.Serialize();
        //    MarkModified();
        //}

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

        public void SetSizingBeamStatus(string sizingBeamStatus)
        {
            SizingBeamStatus = sizingBeamStatus;
            MarkModified();
        }

        protected override DailyOperationSizingBeamProduct GetEntity()
        {
            return this;
        }
    }
}