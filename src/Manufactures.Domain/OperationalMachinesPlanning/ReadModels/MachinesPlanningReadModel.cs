using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.OperationalMachinesPlanning.ReadModels
{
    public class MachinesPlanningReadModel : ReadModelBase
    {
        public string Area { get; internal set; }
        public string Blok { get; internal set; }
        public string BlokKaizen { get; internal set; }
        public int? UnitDepartementId { get; internal set; }
        public Guid? MachineId { get; internal set; }
        public int? UserMaintenanceId { get; internal set; }
        public int? UserOperatorId { get; internal set; }

        public MachinesPlanningReadModel(Guid identity) : base(identity) { }
    }
}
