using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class MaterialTypeDocument : ValueObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
        }
    }
}
