using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class OperatorDocumentValueObject : ValueObject
    {
        public Guid Identity { get; set; }
        public string OperatorName { get; set; }

        public OperatorDocumentValueObject(Guid identity, string operatorName)
        {
            Identity = identity;
            OperatorName = operatorName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return OperatorName;
        }
    }
}
