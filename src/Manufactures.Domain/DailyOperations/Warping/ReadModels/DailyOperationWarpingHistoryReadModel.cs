using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingHistoryReadModel : ReadModelBase
    {
        public Guid ShiftDocumentId { get; internal set; }
        public Guid OperatorDocumentId { get; internal set; }
        public DateTimeOffset DateTimeMachine { get; internal set; }
        public string MachineStatus { get; internal set; }
        public string Information { get; internal set; }
        public Guid WarpingBeamId { get; internal set; }
        public double WarpingBeamLengthPerOperator { get; internal set; }
        public Guid DailyOperationWarpingDocumentId { get; internal set; }

        public DailyOperationWarpingHistoryReadModel(Guid identity) : base(identity)
        {
        }
    }
}
