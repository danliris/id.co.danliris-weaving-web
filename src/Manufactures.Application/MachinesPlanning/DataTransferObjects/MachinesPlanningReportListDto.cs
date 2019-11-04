using Manufactures.Domain.MachinesPlanning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.MachinesPlanning.DataTransferObjects
{
    public class MachinesPlanningReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "Block")]
        public string Block { get; set; }

        [JsonProperty(PropertyName = "KaizenBlock")]
        public string KaizenBlock { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "Maintenance")]
        public string Maintenance { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        public MachinesPlanningReportListDto(MachinesPlanningDocument document, string weavingUnit, string machineNumber, string area, string block, string kaizenBlock, string location, string maintenance, string operatorName)
        {
            Id = document.Identity;
            WeavingUnit = weavingUnit;
            MachineNumber = machineNumber;
            Area = document.Area;
            Block = document.Blok;
            KaizenBlock = document.BlokKaizen;
            Location = location;
            Maintenance = maintenance;
            OperatorName = operatorName;
        }
    }
}
