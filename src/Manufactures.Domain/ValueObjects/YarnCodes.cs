using System.Collections.Generic;

namespace Manufactures.Domain.ValueObjects
{
    public class YarnCodes : ListX<string>
    {
        public YarnCodes(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}