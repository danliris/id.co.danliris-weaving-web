using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ReadModels
{
    public class DailyOperationSizingReadModel : ReadModelBase
    {
        public DailyOperationSizingReadModel(Guid identity) : base(identity)
        {
        }
        public Guid? MachineDocumentId { get; internal set; }
        public int? WeavingUnitId { get; internal set; }
        public Guid? OrderDocumentId { get; internal set; }
        public string BeamsWarping { get; internal set; }
        public double YarnStrands { get; internal set; }
        public string RecipeCode { get; internal set; }
        public double NeReal { get; internal set; }
        public int? MachineSpeed { get; internal set; }
        public string TexSQ { get; internal set; }
        public string Visco { get; internal set; }
        public string OperationStatus { get; internal set; }
        public List<DailyOperationSizingBeamDocument> SizingBeamDocuments { get; internal set; }
        public List<DailyOperationSizingDetail> SizingDetails { get; internal set; }
    }
}
