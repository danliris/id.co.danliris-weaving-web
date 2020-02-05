using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomBeamProductReadModel : ReadModelBase
    {
        public DailyOperationLoomBeamProductReadModel(Guid identity) : base(identity)
        {

        }

        public string BeamOrigin { get; internal set; }

        public Guid BeamDocumentId { get; internal set; }

        public double CombNumber { get; internal set; }

        public Guid MachineDocumentId { get; internal set; }

        public DateTimeOffset LatestDateTimeBeamProduct { get; internal set; }

        public string LoomProcess { get; internal set; }

        public string BeamProductStatus { get; internal set; }

        public Guid DailyOperationLoomDocumentId { get; internal set; }
    }
}
