using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Yarns.ValueObjects
{
    public class RingDocumentValueObject : ValueObject
    {
        public RingDocumentValueObject(string code, string number)
        {
            Code = code;
            Number = number;
        }

        public string Code { get; private set; }
        public string Number { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Number;
        }
    }
}
