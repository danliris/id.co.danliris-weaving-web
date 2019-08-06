using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Dtos.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos
{
    public class SizePickupListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public string TexSQ { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; set; }

        [JsonProperty(PropertyName = "StartCounter")]
        public double StartCounter { get; set; }

        [JsonProperty(PropertyName = "FinishCounter")]
        public double FinishCounter { get; set; }

        [JsonProperty(PropertyName = "NettoWeight")]
        public double NettoWeight { get; set; }

        [JsonProperty(PropertyName = "BrutoWeight")]
        public double BrutoWeight { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        public SizePickupListDto(DailyOperationSizingDocument document, string operatorName, string operatorGroup, DateTimeOffset dateTimeDoff, DailyOperationSizingBeamDocumentsCounterDto counter, DailyOperationSizingBeamDocumentsWeightDto weight, double pisMeter, double spu, string beamNumber)
        {
            Id = document.Identity;
            RecipeCode = document.RecipeCode;
            MachineSpeed = document.MachineSpeed;
            TexSQ = document.TexSQ;
            Visco = document.Visco;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            DateTimeMachineHistory = dateTimeDoff;
            StartCounter = counter.Start;
            FinishCounter = counter.Finish;
            NettoWeight = weight.Netto;
            BrutoWeight = weight.Bruto;
            PISMeter = pisMeter;
            SPU = spu;
            BeamNumber = beamNumber;
        }

        public SizePickupListDto()
        {
        }
    }
}
