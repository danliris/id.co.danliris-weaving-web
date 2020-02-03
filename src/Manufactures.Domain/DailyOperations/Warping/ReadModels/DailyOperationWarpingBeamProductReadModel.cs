using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.ReadModels
{
    public class DailyOperationWarpingBeamProductReadModel : ReadModelBase
    {
        public Guid WarpingBeamId { get; internal set; }
        public double WarpingTotalBeamLength { get; internal set; }
        public int WarpingTotalBeamLengthUomId { get; internal set; }
        public double Tention { get; internal set; }
        public int MachineSpeed { get; internal set; }
        public double PressRoll { get; internal set; }
        public string PressRollUom { get; internal set; }
        public string BeamStatus { get; internal set; }
        public DateTimeOffset LatestDateTimeBeamProduct { get; internal set; }
        public Guid DailyOperationWarpingDocumentId { get; set; }

        public DailyOperationWarpingBeamProductReadModel(Guid identity) : base(identity)
        {
        }
    }
}
