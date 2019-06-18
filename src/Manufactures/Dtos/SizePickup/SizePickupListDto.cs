using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class SizePickupListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; }

        [JsonProperty(PropertyName = "TexSQ")]
        public double TexSQ { get; }

        [JsonProperty(PropertyName = "Visco")]
        public double Visco { get; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; }

        [JsonProperty(PropertyName = "Finish")]
        public string Finish { get; }

        [JsonProperty(PropertyName = "Netto")]
        public string Netto { get; }

        [JsonProperty(PropertyName = "Bruto")]
        public string Bruto { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        public SizePickupListDto(DailyOperationSizingDocument document, DailyOperationSizingDetail detail)
        {
            Id = document.Identity;
            DateTimeMachineHistory = detail.DateTimeOperation;
            OperatorDocumentId = new OperatorId(detail.OperatorDocumentId);
            RecipeCode = document.RecipeCode;
            MachineSpeed = document.MachineSpeed;
            TexSQ = document.TexSQ;
            Visco = document.Visco;
            PIS = document.PIS;
            Finish = document.Counter.Finish;
            Netto = document.Weight.Netto;
            Bruto = document.Weight.Bruto;
            SPU = document.SPU;
        }
    }
}
