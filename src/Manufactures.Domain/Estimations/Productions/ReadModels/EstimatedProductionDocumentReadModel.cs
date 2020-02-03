using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Estimations.Productions.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.ReadModels
{
    public class EstimatedProductionDocumentReadModel : ReadModelBase
    {
        public string EstimatedNumber { get; internal set; }
        public DateTime Period { get; internal set; }
        public int UnitId { get; internal set; }
        public EstimatedProductionDocumentReadModel(Guid identity) : base(identity)
        {
        }
    }
}
