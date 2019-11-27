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
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "LoomOperatorGroup")]
        public string LoomOperatorGroup { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "WarpBrokenThreads")]
        public int WarpBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "WeftBrokenThreads")]
        public int WeftBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "LenoBrokenThreads")]
        public int LenoBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "ReprocessTo")]
        public string ReprocessTo { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationLoomBeamHistoryDto(
                                                string beamNumber,
                                                string machineNumber,
                                                string operatorName, 
                                                string loomOperatorGroup, 
                                                DateTimeOffset dateTimeMachine, 
                                                string shiftName, 
                                                string reprocessTo,
                                                string information, 
                                                string machineStatus)
        {
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            OperatorName = operatorName;
            LoomOperatorGroup = loomOperatorGroup;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            ReprocessTo = reprocessTo;
            Information = information;
            MachineStatus = machineStatus;
        }

        public void SetWarpBrokenThreads(int warpBrokenThreads)
        {
            WarpBrokenThreads = warpBrokenThreads;
        }

        public void SetWeftBrokenThreads(int weftBrokenThreads)
        {
            WeftBrokenThreads = weftBrokenThreads;
        }

        public void SetLenoBrokenThreads(int lenoBrokenThreads)
        {
            LenoBrokenThreads = lenoBrokenThreads;
        }
    }
}
