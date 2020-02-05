using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingDocumentReadModel : ReadModelBase
    {
        public Guid MachineDocumentId { get; internal set; }
        public Guid OrderDocumentId { get; internal set; }
        public double EmptyWeight { get; internal set; }
        public double YarnStrands { get; internal set; }
        public string RecipeCode { get; internal set; }
        public double NeReal { get; internal set; }
        public int? MachineSpeed { get; internal set; }
        public int? TexSQ { get; internal set; }
        public int? Visco { get; internal set; }
        public DateTimeOffset DateTimeOperation { get; internal set; }
        public string OperationStatus { get; internal set; }

        public DailyOperationSizingDocumentReadModel(Guid identity) : base(identity)
        {
        }
    }
}
