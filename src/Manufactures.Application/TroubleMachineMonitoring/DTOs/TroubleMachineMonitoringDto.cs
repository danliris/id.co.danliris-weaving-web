using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.TroubleMachineMonitoring;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.TroubleMachineMonitoring.DTOs
{
    public class TroubleMachineMonitoringDto
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "MachineLocation")]
        public string MachineLocation { get; private set; }

        [JsonProperty(propertyName: "Trouble")]
        public string Trouble { get; private set; }

        [JsonProperty(propertyName: "Description")]
        public string Description { get; private set; }

        [JsonProperty(propertyName: "Process")]
        public string Process { get; private set; }

        [JsonProperty(propertyName: "OrderDocument")]
        public Guid OrderDocument { get; private set; }

        [JsonProperty(propertyName: "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(propertyName: "OperatorDocument")]
        public Guid OperatorDocument { get; private set; }

        [JsonProperty(propertyName: "MachineDocument")]
        public Guid MachineDocument { get; private set; }

        [JsonProperty(propertyName: "Operator")]
        public string Operator { get; private set; }

        [JsonProperty(propertyName: "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(propertyName: "MachineTypeName")]
        public string MachineTypeName { get; private set; }

        [JsonProperty(propertyName: "MachineNumber")]
        public string MachineNumber { get; private set; }

        [JsonProperty(propertyName: "Unit")]
        public int Unit { get; private set; }

        [JsonProperty(PropertyName = "StopDate")]
        public DateTimeOffset StopDate { get; private set; }

        [JsonProperty(PropertyName = "ContinueDate")]
        public DateTimeOffset ContinueDate { get; private set; }


        public TroubleMachineMonitoringDto(TroubleMachineMonitoringDocument document)
        {
            Id = document.Identity;
            ContinueDate = document.ContinueDate;
            StopDate = document.StopDate;
            Trouble = document.Trouble;
            Process = document.Process;
            Description = document.Description;
            OrderDocument = document.OrderDocument.Value;
            MachineDocument = document.MachineDocument.Value;
            OperatorDocument = document.OperatorDocument.Value;
        }

        internal void SetOrderNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        internal void SetConstructionNumber(string constructionNumber)
        {
            ConstructionNumber = constructionNumber;
        }

        internal void SetMachineNumber(string machineNumber)
        {
            MachineNumber = machineNumber;
        }

        internal void SetMachineLocation(string location)
        {
            MachineLocation = location;
        }

        internal void SetMachineTypeName(string machineTypeName)
        {
            MachineTypeName = machineTypeName;
        }

        internal void SetUnitId(int unitId)
        {
            Unit = unitId;
        }

        internal void SetOperatorName(string operatorName)
        {
            Operator = operatorName;
        }

    }
}
