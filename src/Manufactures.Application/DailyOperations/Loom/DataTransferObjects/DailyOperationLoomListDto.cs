using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnitName")]
        public string WeavingUnitName { get; private set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionNumber")]
        public string FabricConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; private set; }

        public DailyOperationLoomListDto(DailyOperationLoomDocument document)
        {
            Id = document.Identity;
            OperationStatus = document.OperationStatus;
        }

        public void SetDateTimeOperation(DateTimeOffset dateTimeMachine)
        {
            DateTimeOperation = dateTimeMachine;
        }

        public void SetWeavingUnit(string weavingUnit)
        {
            WeavingUnitName = weavingUnit;
        }

        public void SetOrderProductionNumber(string orderProductionNumber)
        {
            OrderProductionNumber = orderProductionNumber;
        }

        public void SetFabricConstructionNumber(string fabricConstructionNumber)
        {
            FabricConstructionNumber = fabricConstructionNumber;
        }

        public void SetWarpOrigin(string warpOrigin)
        {
            WarpOrigin = warpOrigin;
        }

        public void SetWeftOrigin(string weftOrigin)
        {
            WeftOrigin = weftOrigin;
        }
    }
}
