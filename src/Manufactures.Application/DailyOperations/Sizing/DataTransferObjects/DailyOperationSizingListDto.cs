﻿using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; private set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; private set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionNumber")]
        public string FabricConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public int WeavingUnitId { get; private set; }

        [JsonProperty(PropertyName = "OperationStatus")]
        public string OperationStatus { get; private set; }

        public DailyOperationSizingListDto(DailyOperationSizingDocument document)
        {
            Id = document.Identity;
            DateTimeMachine = document.DateTimeOperation;
            OperationStatus = document.OperationStatus;
        }

        public void SetMachineNumber(string machineNumber)
        {
            MachineNumber = machineNumber;
        }

        public void SetOrderProductionNumber(string orderProductionNumber)
        {
            OrderProductionNumber = orderProductionNumber;
        }

        public void SetFabricConstructionNumber(string fabricConstructionNumber)
        {
            FabricConstructionNumber = fabricConstructionNumber;
        }

        public void SetWeavingUnitId(int weavingUnitId)
        {
            WeavingUnitId = weavingUnitId;
        }
    }
}