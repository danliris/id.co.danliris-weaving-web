using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomDocumentReadModel : ReadModelBase
    {
        public Guid OrderDocumentId { get; internal set; }

        public double TotalCounter { get; internal set; }

        public int BeamProcessed { get; internal set; }

        public string OperationStatus { get; internal set; }

        public DailyOperationLoomDocumentReadModel(Guid identity) : base(identity)
        {
        }
    }
}
