using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomHistoryReadModel : ReadModelBase
    {
        public Guid BeamDocumentId { get; internal set; }

        public string BeamNumber { get; internal set; }

        public Guid TyingMachineId { get; internal set; }

        public Guid TyingOperatorId { get; internal set; }

        public Guid LoomMachineId { get; internal set; }

        public Guid LoomOperatorId { get; internal set; }

        public double CounterPerOperator { get; internal set; }

        public DateTimeOffset DateTimeMachine { get; internal set; }

        public Guid ShiftDocumentId { get; internal set; }

        public string Information { get; internal set; }

        public string MachineStatus { get; internal set; }

        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomHistoryReadModel(Guid identity) : base(identity)
        {
        }
    }
}
