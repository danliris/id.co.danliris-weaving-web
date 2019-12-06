using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.ProductionResults.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.ProductionResults.ReadModels
{
    public class ProductionResultReadModel : ReadModelBase
    {
        public Guid ShiftDocumentId { get; internal set; }
        public int WeavingUnitId { get; internal set; }
        public Guid OrderDocumentId { get; internal set; }
        public DateTimeOffset DateTimeProductionResult { get; internal set; }
        public List<ProductionResultProducts> ProductionResultProducts { get; internal set; }

        public ProductionResultReadModel(Guid identity) : base(identity)
        {
        }

    }
}
