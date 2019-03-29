using Manufactures.Domain.OperationalMachinesPlanning;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.OperationalMachinesPlanning
{
    public class MachinesPlanningListDto
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

        public MachinesPlanningListDto(MachinesPlanningDocument document,
                                       ManufactureMachine machine)
        {
            Id = document.Identity;
            Area = document.Area;
            Blok = document.Blok;
            BlokKaizen = document.BlokKaizen;
            Machine = machine;
        }
    }
}
