using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomHistoryReadModel : ReadModelBase
    {
        public DailyOperationLoomHistoryReadModel(Guid identity) : base(identity)
        {

        }

        public string BeamNumber { get; internal set; }

        public string MachineNumber { get; internal set; }

        public Guid OperatorDocumentId { get; internal set; }

        public DateTimeOffset DateTimeMachine { get; internal set; }

        public Guid ShiftDocumentId { get; internal set; }

        public int? WarpBrokenThreads { get; internal set; }

        public int? WeftBrokenThreads { get; internal set; }

        public int? LenoBrokenThreads { get; internal set; }

        public string ReprocessTo { get; internal set; }

        public string Information { get; internal set; }

        public string MachineStatus { get; internal set; }

        public Guid DailyOperationLoomDocumentId { get; internal set; }
    }
}
