using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Yarns.ValueObjects
{
    public class UomValueObject : ValueObject
    {
        public UomValueObject(string code, string unit)
        {
            Code = code;
            Unit = unit;
        }

        public string Code { get; private set; }
        public string Unit { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Unit;
        }
    }
}
