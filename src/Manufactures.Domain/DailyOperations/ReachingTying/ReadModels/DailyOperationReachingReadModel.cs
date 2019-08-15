using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.ReadModels
{
    public class DailyOperationReachingReadModel : ReadModelBase
    {
        public DailyOperationReachingReadModel(Guid identity) : base(identity)
        {
        }
        public Guid? MachineDocumentId { get; internal set; }
        public int? WeavingUnitId { get; internal set; }
        public Guid? ConstructionDocumentId { get; internal set; }
        public Guid? SizingBeamId { get; internal set; }
        public double PISPieces { get; internal set; }
        public string ReachingType { get; internal set; }
        public double ReachingWidth { get; internal set; }
        public string OperationStatus { get; internal set; }
        public List<DailyOperationReachingDetail> ReachingDetails { get; internal set; }
    }
}
