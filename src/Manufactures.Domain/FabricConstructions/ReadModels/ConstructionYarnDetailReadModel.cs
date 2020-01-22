using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.ReadModels
{
    public class ConstructionYarnDetailReadModel : ReadModelBase
    {
        public Guid YarnId { get; internal set; }
        public double Quantity { get; internal set; }
        public string Information { get; internal set; }
        public string Type { get; internal set; }
        public Guid FabricConstructionDocumentId { get; internal set; }

        public ConstructionYarnDetailReadModel(Guid identity) : base(identity)
        {
        }
    }
}
