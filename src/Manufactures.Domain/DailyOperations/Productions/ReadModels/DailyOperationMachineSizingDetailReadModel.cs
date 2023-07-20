using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Productions.ReadModels
{
    public class DailyOperationMachineSizingDetailReadModel : ReadModelBase
    {
        public Guid OrderId { get; internal set; }
        public Guid ConstructionId { get; internal set; }
        public double GradeA { get; internal set; }
        public double GradeB { get; internal set; }
        public double GradeC { get; internal set; }
        public double? GradeD { get; internal set; }
        public Guid EstimatedProductionDocumentId { get; internal set; }
        public DailyOperationMachineSizingDetailReadModel(Guid identity) : base(identity)
        {
        }
    }
}
