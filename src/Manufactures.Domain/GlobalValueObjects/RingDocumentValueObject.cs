using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.GlobalValueObjects 
{
    public class RingDocumentValueObject : ValueObject
    {
        public string Code { get; private set; }
        public int Number { get; private set; }

        public RingDocumentValueObject(string code, int number)
        {
            Code = code;
            Number = number;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Number;
        }
    }
}
