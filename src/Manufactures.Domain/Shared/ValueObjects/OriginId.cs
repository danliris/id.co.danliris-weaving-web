using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class OriginId : SingleValueObject<string>
    {
        public OriginId(string origin) : base(origin) { }
    }
}
