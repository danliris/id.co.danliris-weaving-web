using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Loom
{
    public class DailyOperationLoomByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OperationDate")]
        public DateTimeOffset OperationDate { get; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public int WeavingUnit { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "FabricConstructionNumber")]
        public string FabricConstructionNumber { get; }

        [JsonProperty(PropertyName = "LoomHistory")]
        public List<DailyOperationLoomHistoryDto> LoomHistory { get; set; }

        public DailyOperationLoomByIdDto(Guid dailyOperationId, 
                                         DateTimeOffset operationDate,
                                         int weavingUnit,
                                         string machineNumber,
                                         string beamNumber,
                                         string orderNumber,
                                         string fabricConstructionNumber)
        {
            Id = dailyOperationId;
            OperationDate = operationDate;
            WeavingUnit = weavingUnit;
            MachineNumber = machineNumber;
            BeamNumber = beamNumber;
            OrderNumber = orderNumber;
            FabricConstructionNumber = fabricConstructionNumber;
            LoomHistory = new List<DailyOperationLoomHistoryDto>();
        }
    }
}
