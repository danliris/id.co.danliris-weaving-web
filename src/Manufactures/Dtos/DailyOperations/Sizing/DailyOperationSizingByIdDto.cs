using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "ProductionDate")]
        public DateTimeOffset ProductionDate { get; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; }

        [JsonProperty(PropertyName = "WeavingUnitDocumentId")]
        public UnitId WeavingUnitDocumentId { get; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; }

        [JsonProperty(PropertyName = "Start")]
        public DateTimeOffset? Start { get; }

        [JsonProperty(PropertyName = "DoffFinish")]
        public DateTimeOffset? Doff { get; }

        [JsonProperty(PropertyName = "BrokenBeam")]
        public int BrokenBeam { get; }

        [JsonProperty(PropertyName = "Counter")]
        public double Counter { get; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document, DailyOperationSizingDetail details)
        {
            //Id = document.Identity;
            //ProductionDate = document.ProductionDate;
            //MachineDocumentId = document.MachineDocumentId;
            //WeavingUnitDocumentId = document.WeavingUnitId;
            //BeamDocumentId = new BeamId(details.BeamDocumentId.Value);
            //ConstructionDocumentId = new ConstructionId(details.ConstructionDocumentId.Value);
            //PIS = details.PIS;
            //Visco = details.Visco;
            //Start =details.ProductionTime.Deserialize<DailyOperationSizingProductionTimeValueObject>().Start;
            //Doff = details.ProductionTime.Deserialize<DailyOperationSizingProductionTimeValueObject>().DoffFinish;
            //BrokenBeam = details.BrokenBeam;
            //Counter = details.Counter;
            //ShiftDocumentId = new ShiftId(details.ShiftDocumentId.Value);
        }
    }
}
