using System.Collections.Generic;

namespace Weaving.Domain.ValueObjects
{
    public class Blended : ListX<float>
    {
        public Blended(IEnumerable<float> collection) : base(collection)
        {
        }
    }
}
