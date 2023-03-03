using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomBeamsUsedDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamOrigin")]
        public string BeamOrigin { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public Guid BeamDocumentId { get; }

        [JsonProperty(PropertyName = "TyingMachineNumber")]
        public string TyingMachineNumber { get; set; }

        [JsonProperty(PropertyName = "TyingOperatorId")]
        public Guid TyingOperatorId { get; set; }

        [JsonProperty(PropertyName = "LoomMachineNumber")]
        public string LoomMachineNumber { get; set; }

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

        [JsonProperty(PropertyName = "DailyOperationLoomDocumentId")]
        public Guid DailyOperationLoomDocumentId { get; set; }

        public DailyOperationLoomBeamsUsedDto(Guid identity,
                                              string beamOrigin,
                                              string beamNumber,
                                              Guid beamDocumentId,
                                              string tyingMachineNumber,
                                              string loomMachineNumber,
                                              Guid tyingOperatorId,
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
                                              DateTimeOffset latestDateTimeProcessed,
                                              string beamUsedStatus,
                                              Guid dailyOperationLoomDocumentId)
        {
            Id = identity;
            BeamOrigin = beamOrigin;
            BeamNumber = beamNumber;
            BeamDocumentId = beamDocumentId;
            TyingMachineNumber = tyingMachineNumber;
            TyingOperatorId = tyingOperatorId;
            LoomMachineNumber = loomMachineNumber;
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
            LatestDateTimeProcessed = latestDateTimeProcessed;
            BeamUsedStatus = beamUsedStatus;
            DailyOperationLoomDocumentId = dailyOperationLoomDocumentId;
        }

        public void SetTyingMachineNumber(string tyingMachineNumber)
        {
            TyingMachineNumber = tyingMachineNumber;
        }

        public void SetTyingOperatorId(Guid tyingOperatorId)
        {
            TyingOperatorId = tyingOperatorId;
        }

        public void SetLoomMachineNumber(string loomMachineNumber)
        {
            LoomMachineNumber = loomMachineNumber;
        }
    }
}
