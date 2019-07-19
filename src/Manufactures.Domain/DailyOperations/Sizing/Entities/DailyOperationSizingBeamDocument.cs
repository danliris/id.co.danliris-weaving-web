using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingBeamDocument : EntityBase<DailyOperationSizingBeamDocument>
    {
        public Guid SizingBeamId { get; private set; }

        public DateTimeOffset DateTimeBeamDocument { get; private set; }

        public string Counter { get; private set; }

        public string Weight { get; private set; }

        public double PISMeter { get; private set; }

        public double SPU { get; private set; }

        public string SizingBeamStatus { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingBeamDocument(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingBeamDocument(Guid identity,
                                                BeamId sizingBeamId,
                                                DateTimeOffset dateTimeBeamDocument,
                                                DailyOperationSizingCounterValueObject counter,
                                                DailyOperationSizingWeightValueObject weight,
                                                double pisMeter,
                                                double spu,
                                                string sizingBeamStatus) : this(identity)
        {
            SizingBeamId = sizingBeamId.Value;
            DateTimeBeamDocument = dateTimeBeamDocument;
            Counter = counter.Serialize();
            Weight = weight.Serialize();
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
        public DailyOperationSizingBeamDocument(Guid identity,
                                                DateTimeOffset dateTimeBeamDocument,
                                                DailyOperationSizingCounterValueObject counter,
                                                DailyOperationSizingWeightValueObject weight,
                                                double pisMeter,
                                                double spu,
                                                string sizingBeamStatus) : this(identity)
        {
            DateTimeBeamDocument = dateTimeBeamDocument;
            Counter = counter.Serialize();
            Weight = weight.Serialize();
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

        public void SetCounter(DailyOperationSizingCounterValueObject counter)
        {
            Counter = counter.Serialize();
            MarkModified();
        }

        public void SetWeight(DailyOperationSizingWeightValueObject weight)
        {
            Weight = weight.Serialize();
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

        protected override DailyOperationSizingBeamDocument GetEntity()
        {
            return this;
        }
    }
}