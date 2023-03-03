using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.TroubleMachineMonitoring.ReadModels
{
    public class TroubleMachineMonitoringReadModel : ReadModelBase
    {
        public TroubleMachineMonitoringReadModel(Guid identity) : base(identity)
        {
        }

        public DateTimeOffset ContinueDate { get; internal set; }
        public DateTimeOffset StopDate { get; internal set; }
        public Guid? OrderDocument { get; internal set; }
        public string Process { get; internal set; }
        public Guid? OperatorDocument { get; internal set; }
        public Guid? MachineDocument { get; internal set; }
        public string Trouble { get; internal set; }
        public string Description { get; internal set; }              
    }
}
