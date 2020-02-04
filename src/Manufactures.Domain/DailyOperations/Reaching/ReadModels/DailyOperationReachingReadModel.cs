using Infrastructure.Domain.ReadModels;

using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Reaching.ReadModels
{
    public class DailyOperationReachingReadModel : ReadModelBase
    {
        public Guid? MachineDocumentId { get; internal set; }
        public Guid? OrderDocumentId { get; internal set; }
        public Guid? SizingBeamId { get; internal set; }
        public string ReachingInTypeInput { get; internal set; }
        public string ReachingInTypeOutput { get; internal set; }
        public double ReachingInWidth { get; internal set; }
        public int CombEdgeStitching { get; internal set; }
        public int CombNumber { get; internal set; }
        public double CombWidth { get; internal set; }
        public string OperationStatus { get; internal set; }

        public DailyOperationReachingReadModel(Guid identity) : base(identity)
        {
        }
    }
}
