using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class Blended : ListX<float>
    {
        public Blended(IEnumerable<float> collection) : base(collection)
        {
        }
    }
}