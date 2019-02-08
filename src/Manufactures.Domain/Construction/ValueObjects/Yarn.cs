using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Yarn : ValueObject
    {
        public Yarn(Guid id,
                    string code,
                    string name,
                    string materialCode,
                    string ringCode)
        {
            Id = id;
            Code  = code;
            Name = name;
            MaterialCode = materialCode;
            RingCode = ringCode;
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string MaterialCode { get; private set; }
        public string RingCode { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Code;
            yield return Name;
            yield return MaterialCode;
            yield return RingCode;
        }
    }
}
