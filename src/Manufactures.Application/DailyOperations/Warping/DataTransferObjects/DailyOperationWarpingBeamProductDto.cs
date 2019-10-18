﻿using Manufactures.Domain.Beams;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingBeamProductDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; }

        [JsonProperty(PropertyName = "LatestDateTimeBeamProduct")]
        public DateTimeOffset LatestDateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "BrokenThreadsCause")]
        public int BrokenThreadsCause { get; }

        [JsonProperty(PropertyName = "ConeDeficient")]
        public int ConeDeficient { get; }

        [JsonProperty(PropertyName = "LooseThreadsAmount")]
        public int LooseThreadsAmount { get; }

        [JsonProperty(PropertyName = "RightLooseCreel")]
        public int RightLooseCreel { get; }

        [JsonProperty(PropertyName = "LeftLooseCreel")]
        public int LeftLooseCreel { get; }

        [JsonProperty(PropertyName = "WarpingBeamLength")]
        public double WarpingBeamLength { get; }

        [JsonProperty(PropertyName = "Tention")]
        public int Tention { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; }

        [JsonProperty(PropertyName = "PressRoll")]
        public double PressRoll { get; }

        [JsonProperty(PropertyName = "WarpingBeamStatus")]
        public string WarpingBeamStatus { get; }

        public DailyOperationWarpingBeamProductDto(DailyOperationWarpingBeamProduct beamProduct, BeamDocument beamDocument)
        {
            Id = beamProduct.Identity;
            WarpingBeamNumber = beamDocument.Number;
            LatestDateTimeBeamProduct = beamProduct.LatestDateTimeBeamProduct;
            WarpingBeamLength = beamProduct.WarpingBeamLength ?? 0;
            BrokenThreadsCause = beamProduct.BrokenThreadsCause ?? 0;
            ConeDeficient = beamProduct.ConeDeficient ?? 0;
            LooseThreadsAmount = beamProduct.LooseThreadsAmount ?? 0;
            RightLooseCreel = beamProduct.RightLooseCreel ?? 0;
            LeftLooseCreel = beamProduct.LeftLooseCreel ?? 0;
            Tention = beamProduct.Tention ?? 0;
            MachineSpeed = beamProduct.MachineSpeed ?? 0;
            PressRoll = beamProduct.PressRoll ?? 0;
            WarpingBeamStatus = beamProduct.BeamStatus;
        }
    }
}
