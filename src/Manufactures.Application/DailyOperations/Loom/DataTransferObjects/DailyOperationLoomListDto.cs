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

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; private set; }

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

        public void SetDateTimeMachine(DateTimeOffset dateTimeMachine)
        {
            DateTimeMachine = dateTimeMachine;
        }

        public void SetWeavingUnit(string weavingUnit)
        {
            WeavingUnit = weavingUnit;
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
