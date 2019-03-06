using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class YarnValueObject : ValueObject
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string MaterialCode { get; private set; }
        public string RingCode { get; private set; }

        public YarnValueObject(string code,
                               string name,
                               string materialCode,
                               string ringCode)
        {
            Code  = code;
            Name = name;
            MaterialCode = materialCode;
            RingCode = ringCode;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
            yield return MaterialCode;
            yield return RingCode;
        }
    }
}
