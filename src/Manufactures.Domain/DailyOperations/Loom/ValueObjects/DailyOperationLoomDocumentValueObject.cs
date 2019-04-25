using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ValueObjects
{
    public class DailyOperationLoomDocumentValueObject : ValueObject
    {
        public Guid Id { get; private set; }
        public DateTimeOffset DateOperated { get; private set; }
        public string MachineId { get; private set; }
        public int UnitId { get; private set; }
        public string Status { get; private set; }
       
        public IReadOnlyCollection<DailyOperationLoomDetailsValueObject> DailyOperationMachineDetails { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOperated;
            yield return MachineId;
            yield return UnitId;
            yield return Status;
            yield return DailyOperationMachineDetails;
        }
    }
}
