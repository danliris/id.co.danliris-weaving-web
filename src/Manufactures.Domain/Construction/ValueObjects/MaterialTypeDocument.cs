using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class MaterialTypeDocument : ValueObject
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }

        public MaterialTypeDocument(Guid id, string code)
        {
            Id = id;
            Code = code;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
        }
    }
}
