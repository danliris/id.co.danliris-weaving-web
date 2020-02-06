using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingBeamProductReadModel : ReadModelBase
    {
        public Guid SizingBeamId { get; internal set; }
        public double CounterStart { get; internal set; }
        public double CounterFinish { get; internal set; }
        public double WeightNetto { get; internal set; }
        public double WeightBruto { get; internal set; }
        public double WeightTheoritical { get; internal set; }
        public double PISMeter { get; internal set; }
        public double SPU { get; internal set; }
        public string BeamStatus { get; internal set; }
        public DateTimeOffset LatestDateTimeBeamProduct { get; internal set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingBeamProductReadModel(Guid identity) : base(identity)
        {
        }
    }
}
