using Manufactures.Domain.Construction.ValueObjects;
using System;

namespace Manufactures.Domain.Construction.Entities
{
    public class WeftEntity : ConstructionDetail
    {
        public WeftEntity(Guid identity) : base(identity) { }
        public WeftEntity(Guid identity, double quantity, string information, Yarn yarn, bool isNew) : base(identity, quantity, information, yarn, isNew) { }
    }
}
