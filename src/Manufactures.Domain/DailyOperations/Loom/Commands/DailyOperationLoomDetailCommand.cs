using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        //From Beam Document
        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; set; }

        //From DOM Time
        [JsonProperty(PropertyName = "DOMTime")]
        public DailyOperationLoomTimeCommand DOMTime { get; private set; }

        ////Self Properties (Details)
        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; private set; }

        [JsonProperty(PropertyName = "BeamOperatorId")]
        public OperatorId BeamOperatorId { get; private set; }

        [JsonProperty(PropertyName = "SizingOperatorId")]
        public OperatorId SizingOperatorId { get; set; }

        [JsonProperty(PropertyName = "WarpsOrigin")]
        public List<Origin> WarpsOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftsOrigin")]
        public List<Origin> WeftsOrigin { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        [JsonProperty(PropertyName = "DetailStatus")]
        public string DetailStatus { get; private set; }
    }
}
