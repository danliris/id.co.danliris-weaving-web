using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingHistoryValueObject : ValueObject
    {
        public DateTimeOffset TimeOnMachine { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }

        public DailyOperationSizingHistoryValueObject() { }

        public DailyOperationSizingHistoryValueObject(DateTimeOffset timeOnMachine,
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
