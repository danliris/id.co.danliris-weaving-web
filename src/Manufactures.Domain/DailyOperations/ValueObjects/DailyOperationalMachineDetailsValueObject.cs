using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DailyOperationalMachineDetailsValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Identity")]
        public Guid Identity { get; private set; }

        //From Order Document
        [JsonProperty(PropertyName = "OrderDocument")]
        public OrderDocumentValueObject OrderDocument { get; private set; }

        //From Beam Document
        [JsonProperty(PropertyName = "BeamDocument")]
        public BeamDocumentValueObject BeamDocument { get; private set; }

        //From DOM Time
        [JsonProperty(PropertyName = "DOMTime")]
        public DOMTimeValueObject DOMTime { get; private set; }

        ////Self Properties (Details)
        [JsonProperty(PropertyName = "ShiftDocument")]
        public ShiftDocumentValueObject ShiftDocument { get; private set; }

        [JsonProperty(PropertyName = "BeamOperatorDocument")]
        public OperatorDocumentValueObject BeamOperatorDocument { get; private set; }

        [JsonProperty(PropertyName = "SizingOperatorDocument")]
        public OperatorDocumentValueObject SizingOperatorDocument { get; private set; }

        [JsonProperty(PropertyName = "LoomGroup")]
        public string LoomGroup { get; private set; }

        [JsonProperty(PropertyName = "SizingGroup")]
        public string SizingGroup { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        [JsonProperty(PropertyName = "DetailStatus")]
        public string DetailStatus { get; private set; }

        public DailyOperationalMachineDetailsValueObject(Guid identity, OrderDocumentValueObject orderDocument, 
            //BeamDocumentValueObject beamDocument, 
            DOMTimeValueObject domTime, 
            //ShiftDocumentValueObject shiftDocument, OperatorDocumentValueObject beamOperatorDocument, OperatorDocumentValueObject sizingOperatorDocument, 
            string loomGroup, string sizingGroup, string information, string detailStatus)
        {
            Identity = identity;
            OrderDocument = orderDocument;
            //BeamDocument = beamDocument;
            DOMTime = domTime;
            //ShiftDocument = shiftDocument;
            //BeamOperatorDocument = beamOperatorDocument;
            //SizingOperatorDocument = sizingOperatorDocument;
            LoomGroup = loomGroup;
            SizingGroup = sizingGroup;
            Information = information;
            DetailStatus = detailStatus;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OrderDocument;
            //yield return BeamDocument;
            yield return DOMTime;
            //yield return ShiftDocument;
            //yield return BeamOperatorDocument;
            //yield return SizingOperatorDocument;
            yield return LoomGroup;
            yield return SizingGroup;
            yield return Information;
            yield return DetailStatus;
        }
    }
}
