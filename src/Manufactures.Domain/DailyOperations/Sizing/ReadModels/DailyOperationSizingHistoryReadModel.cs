using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingHistoryReadModel : ReadModelBase
    {
        public Guid ShiftDocumentId { get; internal set; }
        public Guid OperatorDocumentId { get; internal set; }
        public DateTimeOffset DateTimeMachine { get; internal set; }
        public string MachineStatus { get; internal set; }
        public int BrokenPerShift { get; internal set; }
        public string SizingBeamNumber { get; internal set; }
        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingHistoryReadModel(Guid identity) : base(identity)
        {
        }
    }
}
