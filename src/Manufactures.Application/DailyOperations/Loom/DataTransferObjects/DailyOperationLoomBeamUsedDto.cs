using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomBeamUsedDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamOrigin")]
        public string BeamOrigin { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "StartCounter")]
        public double StartCounter { get; }

        [JsonProperty(PropertyName = "FinishCounter")]
        public double FinishCounter { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public double MachineSpeed { get; }

        [JsonProperty(PropertyName = "SCMPX")]
        public double SCMPX { get; }

        [JsonProperty(PropertyName = "Efficiency")]
        public double Efficiency { get; }

        [JsonProperty(PropertyName = "F")]
        public double F { get; }

        [JsonProperty(PropertyName = "W")]
        public double W { get; }

        [JsonProperty(PropertyName = "L")]
        public double L { get; }

        [JsonProperty(PropertyName = "T")]
        public double T { get; }

        [JsonProperty(PropertyName = "UomUnit")]
        public string UomUnit { get; }

        [JsonProperty(PropertyName = "LatestDateTimeProcessed")]
        public DateTimeOffset LatestDateTimeProcessed { get; }

        [JsonProperty(PropertyName = "BeamUsedStatus")]
        public string BeamUsedStatus { get; }

        public DailyOperationLoomBeamUsedDto(Guid identity,
                                             string beamOrigin,
                                             string beamNumber,
                                             double startCounter,
                                             double finishCounter,
                                             double machineSpeed,
                                             double scmpx,
                                             double efficiency,
                                             double f,
                                             double w,
                                             double l,
                                             double t,
                                             string uomUnit,
                                             DateTimeOffset latestDateTimeBeamProduct,
                                             string beamUsedStatus)
        {
            Id = identity;
            BeamOrigin = beamOrigin;
            BeamNumber = beamNumber;
            StartCounter = startCounter;
            FinishCounter = finishCounter;
            MachineSpeed = machineSpeed;
            SCMPX = scmpx;
            Efficiency = efficiency;
            F = f;
            W = w;
            L = l;
            T = t;
            UomUnit = uomUnit;
            LatestDateTimeProcessed = latestDateTimeBeamProduct;
            BeamUsedStatus = beamUsedStatus;
        }
    }
}
