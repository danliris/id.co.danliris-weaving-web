using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.TroubleMachineMonitoring;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.TroubleMachineMonitoring.DTOs
{
    public class TroubleMachineMonitoringListDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "Trouble")]
        public string Trouble { get; private set; }

        [JsonProperty(propertyName: "Description")]
        public string Description { get; private set; }

        [JsonProperty(propertyName: "Process")]
        public string Process { get; private set; }

        [JsonProperty(propertyName: "MachineNumber")]
        public string MachineNumber { get; private set; }

        [JsonProperty(propertyName: "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(propertyName: "Operator")]
        public string Operator { get; private set; }

        [JsonProperty(propertyName: "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(propertyName: "Unit")]
        public int WeavingUnitId { get; private set; }

        [JsonProperty(PropertyName = "StopDate")]
        public DateTimeOffset StopDate { get; private set; }

        [JsonProperty(PropertyName = "ContinueDate")]
        public DateTimeOffset ContinueDate { get; private set; }

        public TroubleMachineMonitoringListDto(TroubleMachineMonitoringDocument document)
        {
            Id = document.Identity;
            Trouble = document.Trouble;
            ContinueDate = document.ContinueDate;
            StopDate = document.StopDate;
            Description = document.Description;
            Process = document.Process;
        }

        internal void SetOrderName(string orderName)
        {
            OrderNumber = orderName;
        }

        internal void SetConstructionNumber(string constructionNumber)
        {
            ConstructionNumber = constructionNumber;
        }

        internal void SetUnitId(int unitId)
        {
            WeavingUnitId = unitId;
        }

        internal void SetMachineNumber(string machineNumber)
        {
            MachineNumber = machineNumber;
        }

        internal void SetOperatorName(string operatorName)
        {
            Operator = operatorName;
        }
    }
}
