using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class YarnDocumentValueObject : ValueObject
    {
        public string  Code { get; set; }
        public string Name { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
        }
    }
}
