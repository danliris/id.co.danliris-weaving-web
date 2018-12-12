using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weaving.Domain.Entities
{
    public class GoodsConstruction : Entity
    {
        public GoodsConstruction(Guid identity, List<string> codes)
        {
            Identity = identity;
            Codes = codes;
        }

        public IReadOnlyList<string> Codes { get; }
    }
}
