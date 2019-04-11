using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.ReadModels
{
    public class DailyOperationalMachineDocumentReadModel : ReadModelBase
    {
        public DailyOperationalMachineDocumentReadModel(Guid identity) : base(identity)
        {
        }
        public DateTimeOffset DateOperated { get; internal set; }
        public Guid? MachineId { get; internal set; }
        public int? UnitId { get; internal set; }
        public string Status { get; internal set; }
        public List<DailyOperationalMachineDetail> DailyOperationMachineDetails { get; internal set; }
    }
}
