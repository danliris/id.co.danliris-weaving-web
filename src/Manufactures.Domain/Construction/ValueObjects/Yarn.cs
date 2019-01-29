using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Yarn : ValueObject
    {
        public Yarn(Guid id,
                    string code,
                    string name)
        {
            Id = id;
            Code  = code;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Name;
        }
    }
}
