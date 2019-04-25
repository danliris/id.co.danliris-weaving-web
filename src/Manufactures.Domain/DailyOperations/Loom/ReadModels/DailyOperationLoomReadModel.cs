using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomReadModel : ReadModelBase
    {
        public DailyOperationLoomReadModel(Guid identity) : base(identity)
        {
        }
        public DateTimeOffset DateOperated { get; internal set; }
        public Guid? MachineId { get; internal set; }
        public int? UnitId { get; internal set; }
        public string Status { get; internal set; }
        public List<DailyOperationLoomDetail> DailyOperationLoomDetails { get; internal set; }
    }
}
