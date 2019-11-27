using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomBeamProductDto
    {
        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "LatestDateTimeBeamProduct")]
        public DateTimeOffset LatestDateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "LoomProcess")]
        public string LoomProcess { get; }

        [JsonProperty(PropertyName = "BeamProductStatus")]
        public string BeamProductStatus { get; }

        public DailyOperationLoomBeamProductDto(string beamNumber, 
                                                string machineNumber, 
                                                DateTimeOffset latestDateTimeBeamProduct,
                                                string loomProcess,
                                                string beamProductStatus)
        {
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            LoomProcess = loomProcess;
            BeamProductStatus = beamProductStatus;
        }
    }
}
