using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingReadModel : ReadModelBase
    {
        public DailyOperationSizingReadModel(Guid identity) : base(identity)
        {
        }
        public DateTimeOffset ProductionDate { get; internal set; }
        public Guid? MachineDocumentId { get; internal set; }
        public int? WeavingUnitId { get; internal set; }
        public List<DailyOperationSizingDetail> DailyOperationSizingDetails { get; internal set; }
    }
}
