using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.ReachingTying.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.ReadModels
{
    public class DailyOperationReachingTyingReadModel : ReadModelBase
    {
        public DailyOperationReachingTyingReadModel(Guid identity) : base(identity)
        {
        }
        public Guid? MachineDocumentId { get; internal set; }
        public int? WeavingUnitId { get; internal set; }
        public Guid? ConstructionDocumentId { get; internal set; }
        public Guid? SizingBeamId { get; internal set; }
        public double PISPieces { get; internal set; }
        public string ReachingValueObjects { get; internal set; }
        public string TyingValueObjects { get; internal set; }
        public string OperationStatus { get; internal set; }
        public List<DailyOperationReachingTyingDetail> ReachingTyingDetails { get; internal set; }
    }
}
