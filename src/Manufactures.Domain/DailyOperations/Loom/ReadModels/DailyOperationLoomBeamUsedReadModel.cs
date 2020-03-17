using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomBeamUsedReadModel : ReadModelBase
    {
        public string BeamOrigin { get; internal set; }
        public Guid BeamDocumentId { get; internal set; }
        public string BeamNumber { get; internal set; }
        public Guid TyingMachineId { get; internal set; }
        public Guid TyingOperatorId { get; internal set; }
        public Guid LoomMachineId { get; internal set; }
        public Guid LoomOperatorId { get; internal set; }
        public DateTimeOffset DateTimeProcessed { get; internal set; }
        public Guid ShiftDocumentId { get; internal set; }
        public string Process { get; internal set; }
        public string BeamUsedStatus { get; internal set; }
        public Guid DailyOperationLoomDocumentId { get; set; }
        public Guid DailyOperationLoomProductId { get; set; }

        public DailyOperationLoomBeamUsedReadModel(Guid identity) : base(identity)
        {
        }
    }
}
