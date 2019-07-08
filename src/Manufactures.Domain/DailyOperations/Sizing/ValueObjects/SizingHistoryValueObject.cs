using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class SizingHistoryValueObject : ValueObject
    {
        public DateTimeOffset MachineDate { get; private set; }
        public TimeSpan MachineTime { get; private set; }
        public string MachineStatus { get; private set; }
        public string Information { get; private set; }

        public SizingHistoryValueObject() { }

        public SizingHistoryValueObject(DateTimeOffset machineDate, TimeSpan machineTime,
                                         string machineStatus,
                                         string information)
        {
            MachineDate = machineDate;
            MachineTime = machineTime;
            MachineStatus = machineStatus;
            Information = information;
        }

        public void SetMachineDate(DateTimeOffset value)
        {
            MachineDate = value;
        }

        public void SetMachineTime(TimeSpan machineTime)
        {
            MachineTime = machineTime;
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
            yield return MachineDate;
            yield return MachineTime;
            yield return MachineStatus;
            yield return Information;
        }
    }
}
