using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomHistoryDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "TyingMachineNumber")]
        public string TyingMachineNumber { get; }

        [JsonProperty(PropertyName = "TyingOperatorName")]
        public string TyingOperatorName { get; }

        [JsonProperty(PropertyName = "TyingOperatorGroup")]
        public string TyingOperatorGroup { get; }

        [JsonProperty(PropertyName = "LoomMachineNumber")]
        public string LoomMachineNumber { get; }

        [JsonProperty(PropertyName = "LoomOperatorName")]
        public string LoomOperatorName { get; }

        [JsonProperty(PropertyName = "LoomOperatorGroup")]
        public string LoomOperatorGroup { get; }

        [JsonProperty(PropertyName = "CounterPerOperator")]
        public double CounterPerOperator { get; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeMachine { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        public DailyOperationLoomHistoryDto(Guid identity,
                                            string beamNumber,
                                            string tyingMachineNumber,
                                            string tyingOperatorName,
                                            string tyingOperatorGroup,
                                            string loomMachineNumber,
                                            string loomOperatorName,
                                            string loomOperatorGroup,
                                            DateTimeOffset dateTimeMachine, 
                                            string shiftName, 
                                            string information, 
                                            string machineStatus)
        {
            Id = identity;
            BeamNumber = beamNumber;
            TyingMachineNumber = tyingMachineNumber;
            TyingOperatorName = tyingOperatorName;
            TyingOperatorGroup = tyingOperatorGroup;
            LoomMachineNumber = loomMachineNumber;
            LoomOperatorName = loomOperatorName;
            LoomOperatorGroup = loomOperatorGroup;
            DateTimeMachine = dateTimeMachine;
            ShiftName = shiftName;
            Information = information;
            MachineStatus = machineStatus;
        }

        //public void SetWarpBrokenThreads(int warpBrokenThreads)
        //{
        //    WarpBrokenThreads = warpBrokenThreads;
        //}

        //public void SetWeftBrokenThreads(int weftBrokenThreads)
        //{
        //    WeftBrokenThreads = weftBrokenThreads;
        //}

        //public void SetLenoBrokenThreads(int lenoBrokenThreads)
        //{
        //    LenoBrokenThreads = lenoBrokenThreads;
        //}
    }
}
