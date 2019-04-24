using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.ValueObjects
{
    public class DailyOperationalLoomDetailsValueObject : ValueObject
    {
        public Guid Identity { get; private set; }
        public OrderDocumentValueObject OrderDocument { get; private set; }
        public BeamDocumentValueObject BeamDocument { get; private set; }
        public DailyOperationLoomTimeValueObject DOMTime { get; private set; }
        public ShiftDocumentValueObject ShiftDocument { get; private set; }
        public OperatorDocumentValueObject BeamOperatorDocument { get; private set; }
        public OperatorDocumentValueObject SizingOperatorDocument { get; private set; }
        public string LoomGroup { get; private set; }
        public string SizingGroup { get; private set; }
        public string Information { get; private set; }
        public string DetailStatus { get; private set; }

        public DailyOperationalLoomDetailsValueObject(Guid identity, OrderDocumentValueObject orderDocument, 
            //BeamDocumentValueObject beamDocument, 
            DailyOperationLoomTimeValueObject domTime, 
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
