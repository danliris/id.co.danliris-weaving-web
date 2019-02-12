using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class MaterialTypeValueObject : ValueObject
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public MaterialTypeValueObject(Guid id, 
                                       string code,
                                       string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Name;
        }
    }
}
