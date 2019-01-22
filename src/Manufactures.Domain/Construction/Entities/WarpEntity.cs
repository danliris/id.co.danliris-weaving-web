using Manufactures.Domain.Construction.ValueObjects;
using System;

namespace Manufactures.Domain.Construction.Entities
{
    public class WarpEntity : ConstructionDetail
    {
        public WarpEntity(Guid identity) : base(identity) { }
        public WarpEntity(Guid identity, double quantity, string information, Yarn yarn, bool isNew) : base(identity, quantity, information, yarn, isNew) { }
    }
}
