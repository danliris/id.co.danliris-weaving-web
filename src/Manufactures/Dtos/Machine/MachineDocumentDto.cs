using Manufactures.Domain.Machines;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Machine
{
    public class MachineDocumentDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "MachineNumber")]
        public string MachineNumber { get; private set; }

        [JsonProperty(propertyName: "Location")]
        public string Location { get; private set; }

        [JsonProperty(propertyName: "MachineTypeId")]
        public MachineTypeId MachineTypeId { get; private set; }

        [JsonProperty(propertyName: "WeavingUnitId")]
        public UnitId WeavingUnitId { get; private set; }

        [JsonProperty(propertyName: "Cutmark")]
        public int Cutmark { get; private set; }

        [JsonProperty(propertyName: "CutmarkUom")]
        public string CutmarkUom { get; private set; }

        public MachineDocumentDto(MachineDocument document, string cutmarkUom)
        {
            Id = document.Identity;
            MachineNumber = document.MachineNumber;
            Location = document.Location;
            MachineTypeId = document.MachineTypeId;
            WeavingUnitId = document.WeavingUnitId;
            Cutmark = document.Cutmark;
            CutmarkUom = cutmarkUom;
        }

        //public MachineDocumentDto(MachineDocument machine, 
        //                          MachineTypeValueObject machineTypeValueObject, 
        //                          WeavingUnit weavingUnitValueObject)
        //{

        //}
    }
}
