using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Estimations.Productions.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.ReadModels
{
    public class EstimatedProductionDocumentReadModel : ReadModelBase
    {
        public EstimatedProductionDocumentReadModel(Guid identity) : base(identity)
        {
        }
        public string EstimatedNumber { get; internal set; }
        public string Period { get; internal set; }
        public string Unit { get; internal set; }
        public List<EstimationProduct> EstimationProducts { get; internal set; }
        public double TotalEstimationOrder { get; internal set; }
    }
}
