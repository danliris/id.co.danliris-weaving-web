using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.ReadModels
{
    public class DailyOperationLoomBeamUsedReadModel : ReadModelBase
    {
        public string BeamOrigin { get; internal set; }
        public Guid BeamDocumentId { get; internal set; }
        public string BeamNumber { get; internal set; }
        public Guid TyingMachineDocumentId { get; internal set; }
        public string TyingMachineNumber { get; internal set; }
        public string LoomMachineNumber { get; internal set; }
        public Guid LastTyingOperatorDocumentId { get; internal set; }
        public string LastTyingOperatorName { get; internal set; }
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
        public DateTimeOffset LastDateTimeProcessed { get; internal set; }
        public string BeamUsedStatus { get; internal set; }
        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomBeamUsedReadModel(Guid identity) : base(identity)
        {
        }
    }
}
