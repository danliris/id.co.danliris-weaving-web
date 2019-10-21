using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BeamStockMonitoring.ReadModels
{
    public class BeamStockMonitoringReadModel : ReadModelBase
    {
        public Guid BeamDocumentId { get; internal set; }
        public DateTimeOffset SizingEntryDate { get; internal set; }
        public DateTimeOffset ReachingEntryDate { get; internal set; }
        public DateTimeOffset LoomEntryDate { get; internal set; }
        public DateTimeOffset EmptyEntryDate { get; internal set; }
        public Guid OrderDocumentId { get; internal set; }
        public double SizingLengthStock { get; internal set; }
        public double ReachingLengthStock { get; internal set; }
        public double LoomLengthStock { get; internal set; }
        public int LengthUOMId { get; internal set; }
        public int Position { get; internal set; }
        public bool LoomFinish { get; internal set; }

        public BeamStockMonitoringReadModel(Guid identity) : base(identity) { }
    }
}
