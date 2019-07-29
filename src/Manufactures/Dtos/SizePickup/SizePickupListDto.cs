using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos
{
    public class SizePickupListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; }

        [JsonProperty(PropertyName = "TexSQ")]
        public string TexSQ { get; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        public SizePickupListDto(DailyOperationSizingDocument document, DateTimeOffset dateTimeOperation, double pisMeter, double spu, string operatorName, string operatorGroup, string beamNumber)
        {
            Id = document.Identity;
            DateTimeMachineHistory = dateTimeOperation;
            RecipeCode = document.RecipeCode;
            MachineSpeed = document.MachineSpeed;
            TexSQ = document.TexSQ;
            Visco = document.Visco;
            PISMeter = pisMeter;
            SPU = spu;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            BeamNumber = beamNumber;
        }
    }
}
