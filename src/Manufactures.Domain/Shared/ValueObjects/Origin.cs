using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class Origin : SingleValueObject<string>
    {
        public Origin(string origin) : base(origin) { }
    }
}
