using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Reaching.ReadModels
{
    public class DailyOperationReachingReadModel : ReadModelBase
    {
        public DailyOperationReachingReadModel(Guid identity) : base(identity)
        {
        }
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
        public List<DailyOperationReachingHistory> ReachingHistories { get; internal set; }
    }
}
