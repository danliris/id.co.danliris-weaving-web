using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ValueObjects
{
    public class DailyOperationLoomHistory : ValueObject
    {
        public DateTimeOffset MachineDate { get; private set; }
        public TimeSpan MachineTime { get; private set; }
        public string MachineStatus { get; private set; }
        public bool IsUp { get; private set; }
        public bool IsDown { get; private set; }

        public DailyOperationLoomHistory() { }

        public DailyOperationLoomHistory(DateTimeOffset machineDate, 
                                         TimeSpan machineTime,
                                         string machineStatus,
                                         bool isUp,
                                         bool isDown)
        {
            MachineDate = machineDate;
            MachineTime = machineTime;
            MachineStatus = machineStatus;
            IsUp = IsUp;
            IsDown = IsDown;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MachineDate;
            yield return MachineStatus;
            yield return IsUp;
            yield return IsDown;
        }
    }
}
