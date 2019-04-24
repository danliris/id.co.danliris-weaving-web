using Manufactures.Domain.DailyOperations.Loom.Commands;
using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ValueObjects
{
    public class DailyOperationLoomTimeValueObject : ValueObject
    {
        public DateTimeOffset Pause { get; set; }
        public DateTimeOffset Resume { get; set; }
        public int Difference { get; set; }


        public DailyOperationLoomTimeValueObject(DateTimeOffset pause, 
                                  DateTimeOffset resume, 
                                  DateTimeOffset difference)
        {
            Pause = pause;
            Resume = resume;
            Difference = pause.Subtract(resume).Hours;
        }

        public DailyOperationLoomTimeValueObject(DailyOperationLoomTimeCommand dailyOperationMachine)
        {
            Pause = dailyOperationMachine.Pause;
            Resume = dailyOperationMachine.Resume;
            Difference = 
                dailyOperationMachine.Pause
                                    .Subtract(dailyOperationMachine.Resume)
                                    .Hours;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Pause;
            yield return Resume;
            yield return Difference;
        }
    }
}
