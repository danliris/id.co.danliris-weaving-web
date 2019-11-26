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

        [JsonProperty(PropertyName = "DateTimeBeamProduct")]
        public DateTimeOffset DateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "BeamProductStatus")]
        public string BeamProductStatus { get; }

        public DailyOperationLoomBeamProductDto(string beamNumber, 
                                                string machineNumber, 
                                                DateTimeOffset dateTimeBeamProduct, 
                                                string beamProductStatus)
        {
            BeamNumber = beamNumber;
            MachineNumber = machineNumber;
            DateTimeBeamProduct = dateTimeBeamProduct;
            BeamProductStatus = beamProductStatus;
        }
    }
}
