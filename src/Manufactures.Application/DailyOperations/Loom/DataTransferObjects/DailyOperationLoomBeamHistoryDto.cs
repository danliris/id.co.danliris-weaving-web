using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomBeamHistoryDto
    {
        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "GreigeLength")]
        public string GreigeLength { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationLoomBeamHistoryDto(string beamNumber, 
                                                string machineNumber, 
                                                string operatorName, 
                                                string operatorGroup, 
                                                DateTimeOffset dateTimeMachine, 
                                                string shiftName, 
                                                string greigeLength,
                                                string information, 
                                                string machineStatus)
        {
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            Information = information;
            MachineStatus = machineStatus;
        }

        //public void SetGreigeLength(string greigeLength)
        //{
        //    GreigeLength = greigeLength;
        //}
    }
}
