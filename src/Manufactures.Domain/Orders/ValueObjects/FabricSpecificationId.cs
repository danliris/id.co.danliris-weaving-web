using Moonlay.Domain;
using System;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class FabricSpecificationId : SingleValueObject<Guid>
    {
        public FabricSpecificationId(Guid value) : base(value) { }
    }
}
