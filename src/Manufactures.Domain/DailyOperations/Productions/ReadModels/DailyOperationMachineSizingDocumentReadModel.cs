using Infrastructure.Domain.ReadModels;
//using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Productions.ReadModels
{
    public class DailyOperationMachineSizingDocumentReadModel : ReadModelBase
    {
        public string EstimatedNumber { get; internal set; }
        public DateTime Period { get; internal set; }
        public int UnitId { get; internal set; }
        public DailyOperationMachineSizingDocumentReadModel(Guid identity) : base(identity)
        {
        }
    }
}
