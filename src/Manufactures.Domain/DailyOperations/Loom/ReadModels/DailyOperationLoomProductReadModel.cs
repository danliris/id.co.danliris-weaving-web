using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomProductReadModel : ReadModelBase
    {
        public double StartCounter { get; internal set; }
        public double FinishCounter { get; internal set; }
        public double MachineSpeed { get; internal set; }
        public double SCMPX { get; internal set; }
        public double Efficiency { get; internal set; }
        public double F { get; internal set; }
        public double W { get; internal set; }
        public double L { get; internal set; }
        public double T { get; internal set; }
        public int UomDocumentId { get; internal set; }
        public string UomUnit { get; internal set; }
        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomProductReadModel(Guid identity) : base(identity)
        {
        }
    }
}
