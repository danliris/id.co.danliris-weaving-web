using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class Material : ValueObject
    {
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new System.NotImplementedException();
        }
    }
}