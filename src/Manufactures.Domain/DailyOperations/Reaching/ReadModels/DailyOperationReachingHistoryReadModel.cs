using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.ReadModels
{
    public class DailyOperationReachingHistoryReadModel : ReadModelBase
    {
        public DailyOperationReachingHistoryReadModel(Guid identity) : base(identity)
        {

        }
        public Guid OperatorDocumentId { get; internal set; }

        public int YarnStrandsProcessed { get; internal set; }

        public DateTimeOffset DateTimeMachine { get; internal set; }

        public Guid ShiftDocumentId { get; internal set; }

        public string MachineStatus { get; internal set; }

        public Guid DailyOperationReachingDocumentId { get; internal set; }
    }
}
