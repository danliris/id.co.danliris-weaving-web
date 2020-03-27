using Manufactures.Domain.Beams;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingBeamProductDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; }

        [JsonProperty(PropertyName = "WarpingBeamId")]
        public Guid WarpingBeamId { get; }

        [JsonProperty(PropertyName = "LatestDateTimeBeamProduct")]
        public DateTimeOffset LatestDateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "WarpingTotalBeamLength")]
        public double WarpingTotalBeamLength { get; }

        [JsonProperty(PropertyName = "Tention")]
        public double Tention { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; }

        [JsonProperty(PropertyName = "PressRoll")]
        public double PressRoll { get; }

        [JsonProperty(PropertyName = "WarpingBeamStatus")]
        public string WarpingBeamStatus { get; }

        [JsonProperty(PropertyName = "BrokenCauses")]
        public List<WarpingBrokenThreadsCausesDto> BrokenCauses { get; set; }

        public DailyOperationWarpingBeamProductDto(DailyOperationWarpingBeamProduct beamProduct, BeamDocument beamDocument)
        {
            Id = beamProduct.Identity;
            WarpingBeamNumber = beamDocument.Number;
            WarpingBeamId = beamDocument.Identity;
            LatestDateTimeBeamProduct = beamProduct.LatestDateTimeBeamProduct;
            WarpingTotalBeamLength = beamProduct.WarpingTotalBeamLength;
            Tention = beamProduct.Tention ?? 0;
            MachineSpeed = beamProduct.MachineSpeed ?? 0;
            PressRoll = beamProduct.PressRoll ?? 0;
            WarpingBeamStatus = beamProduct.BeamStatus;
            BrokenCauses = new List<WarpingBrokenThreadsCausesDto>();
            //BrokenCauses = beamProduct.WarpingBrokenThreadsCauses.Select(item => new WarpingBrokenThreadsCausesDto(item)).ToList();
        }
    }
}
