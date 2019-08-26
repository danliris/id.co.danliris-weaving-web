using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomReadModel : ReadModelBase
    {
        public int UnitId { get; internal set; }
        public Guid MachineId { get; internal set; }
        public Guid BeamId { get; internal set; }
        public Guid OrderId { get; internal set; }
        public string DailyOperationStatus { get; internal set; }
        public Guid? DailyOperationMonitoringId { get; internal set; }
        public List<DailyOperationLoomDetail> DailyOperationLoomDetails { get; internal set; }
        public double? UsedYarn { get; internal set; }

        public DailyOperationLoomReadModel(Guid identity) : base(identity)
        {
        }

    }
}
