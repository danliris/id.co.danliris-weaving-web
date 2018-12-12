using Moonlay.Domain;
using System.Collections.Generic;

namespace Moonlay.Manufactures.Domain.ValueObjects
{
    public class Material : ValueObject
    {
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new System.NotImplementedException();
        }
    }
}