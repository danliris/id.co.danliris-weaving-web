using Manufactures.Domain.MachinesPlanning;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.MachinesPlanning
{
    public class MachinesPlanningDocumentDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; }

        [JsonProperty(PropertyName = "Blok")]
        public string Blok { get; }

        [JsonProperty(PropertyName = "BlokKaizen")]
        public string BlokKaizen { get; }

        [JsonProperty(PropertyName = "Machine")]
        public ManufactureMachine Machine { get; }

        [JsonProperty(PropertyName = "UnitDepartementId")]
        public UnitId UnitDepartementId { get; }

        [JsonProperty(PropertyName = "UserMaintenanceId")]
        public UserId UserMaintenanceId { get; }

        [JsonProperty(PropertyName = "UserOperatorId")]
        public UserId UserOperatorId { get; }

        public MachinesPlanningDocumentDto(MachinesPlanningDocument document, 
                                           ManufactureMachine machine)
        {
            Id = document.Identity;
            Area = document.Area;
            Blok = document.Blok;
            BlokKaizen = document.BlokKaizen;
            Machine = machine;
            UnitDepartementId = document.UnitDepartementId;
            UserMaintenanceId = document.UserMaintenanceId;
            UserOperatorId = document.UserOperatorId;
        }
    }
}
