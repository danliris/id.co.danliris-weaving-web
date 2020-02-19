using Manufactures.Domain.Machines;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Machines.DataTransferObjects
{
    public class MachineByIdDto : MachineListDto
    {
        [JsonProperty(PropertyName = "UnitId")]
        public int UnitId { get; private set; }

        [JsonProperty(propertyName: "Cutmark")]
        public int Cutmark { get; private set; }

        [JsonProperty(propertyName: "CutmarkUom")]
        public string CutmarkUom { get; private set; }

        [JsonProperty(propertyName: "MachineTypeId")]
        public Guid MachineTypeId { get; private set; }

        [JsonProperty(propertyName: "Speed")]
        public int Speed { get; private set; }

        [JsonProperty(propertyName: "Process")]
        public string Process { get; private set; }

        [JsonProperty(propertyName: "Area")]
        public string Area { get; private set; }

        [JsonProperty(propertyName: "Block")]
        public string Block { get; private set; }

        public MachineByIdDto(MachineDocument machineDocument, string weavingUnit, MachineTypeDocument machineTypeDocument): base(machineDocument)
        {
            UnitId = machineDocument.WeavingUnitId.Value;
            Cutmark = machineDocument.Cutmark;
            CutmarkUom = machineDocument.CutmarkUom;
            MachineTypeId = machineDocument.MachineTypeId.Value;
            Speed = machineTypeDocument.Speed;
            Process = machineDocument.Process;
            Area = machineDocument.Area;
            Block = machineDocument.Block.ToString();
        }
    }
}
