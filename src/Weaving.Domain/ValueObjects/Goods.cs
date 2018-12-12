using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Moonlay.Manufactures.Domain.ValueObjects
{
    public class Goods : ValueObject
    {
        public Goods(Guid productId, string name)
        {
            this.Identity = productId;
            this.Name = name;
        }

        public Guid Identity { get; }

        public string Name { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}