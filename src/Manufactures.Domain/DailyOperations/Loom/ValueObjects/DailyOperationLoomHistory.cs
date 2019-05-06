using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ValueObjects
{
    public class DailyOperationLoomHistory : ValueObject
    {
        public DateTimeOffset TimeOnMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }

        public DailyOperationLoomHistory() { }

        public DailyOperationLoomHistory(DateTimeOffset timeOnMachine,
                                         string machineStatus,
                                         string information)
        {
            TimeOnMachine = timeOnMachine;
            MachineStatus = machineStatus;
            Information = information;
        }

        public void SetTimeOnMachine(DateTimeOffset value)
        {
            TimeOnMachine = value;
        }

        public void SetMachineStatus(string value)
        {
            MachineStatus = value;
        }

        public void SetInformation(string value)
        {
            Information = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TimeOnMachine;
            yield return MachineStatus;
            yield return Information;
        }
    }
}
