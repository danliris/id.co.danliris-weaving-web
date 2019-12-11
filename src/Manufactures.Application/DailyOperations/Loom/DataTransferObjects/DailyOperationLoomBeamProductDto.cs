using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomBeamProductDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public Guid BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "BeamOrigin")]
        public string BeamOrigin { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        [JsonProperty(PropertyName = "CombNumber")]
        public double CombNumber { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "LatestDateTimeBeamProduct")]
        public DateTimeOffset LatestDateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "LoomProcess")]
        public string LoomProcess { get; }

        [JsonProperty(PropertyName = "BeamProductStatus")]
        public string BeamProductStatus { get; }

        public void SetBeamDocumentId(Guid beamDocumentId)
        {
            this.BeamDocumentId = beamDocumentId;
        }

        public DailyOperationLoomBeamProductDto(Guid identity,
                                                string beamOrigin,
                                                string beamNumber, 
                                                double combNumber,
                                                string machineNumber, 
                                                DateTimeOffset latestDateTimeBeamProduct,
                                                string loomProcess,
                                                string beamProductStatus)
        {
            Id = identity;
            BeamOrigin = beamOrigin;
            BeamNumber = beamNumber;
            CombNumber = combNumber;
            MachineNumber = machineNumber;
            LatestDateTimeBeamProduct = latestDateTimeBeamProduct;
            LoomProcess = loomProcess;
            BeamProductStatus = beamProductStatus;
        }
    }
}
