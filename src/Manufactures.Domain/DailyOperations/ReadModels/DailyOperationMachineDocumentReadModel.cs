using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReadModels
{
    public class DailyOperationMachineDocumentReadModel : ReadModelBase
    {
        public DailyOperationMachineDocumentReadModel(Guid identity) : base(identity)
        {
        }
        public DateTimeOffset Time { get; internal set; }
        public string Information { get; internal set; }
        public Guid? MachineId { get; internal set; }
        public int? UnitId { get; internal set; }
        public List<DailyOperationMachineDetail> DailyOperationMachineDetails { get; internal set; }
    }
}
