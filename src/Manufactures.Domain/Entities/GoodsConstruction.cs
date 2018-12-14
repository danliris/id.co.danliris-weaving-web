using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Entities
{
    public class GoodsConstruction : ReadModelBase
    {
        public GoodsConstruction(Guid identity, List<string> codes) : base(identity)
        {
            Codes = codes;
        }

        public IReadOnlyList<string> Codes { get; }
    }
}
