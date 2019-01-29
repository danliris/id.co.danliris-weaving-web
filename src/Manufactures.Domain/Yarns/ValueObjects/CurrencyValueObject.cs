using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Yarns.ValueObjects
{
    public class CurrencyValueObject : ValueObject
    {
        public CurrencyValueObject(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
        }
    }
}
