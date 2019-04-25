using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.Entities
{
    public class DailyOperationLoomDetail
        : EntityBase<DailyOperationLoomDetail>
    {
        public Guid? OrderDocumentId { get; private set; }
        public string WarpsOrigin { get; private set; }
        public string WeftsOrigin { get; private set; }
        public Guid? BeamDocumentId { get; private set; }
        public string DOMTime { get; private set; }
        public Guid? ShiftDocumentId { get; private set; }
        public Guid? BeamOperatorDocumentId { get; private set; }
        public Guid? SizingOperatorDocumentId { get; private set; }
        public string Information { get; private set; }
        public string DetailStatus { get; private set; }

        public Guid DailyOperationLoomDocumentId { get; set; }
        public DailyOperationMachineLoomReadModel DailyOperationLoomDocument { get; set; }

        public DailyOperationLoomDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationLoomDetail(Guid identity,
                                             OrderId orderDocumentId,
                                             List<Origin> warpsOrigin,
                                             List<Origin> weftsOrigin,
                                             BeamId beamDocumentId,
                                             DailyOperationLoomTimeValueObject domTime,
                                             ShiftId shiftDocumentId,
                                             OperatorId beamOperatorDocumentId,
                                             OperatorId sizingOperatorDocumentId,
                                             string information,
                                             string detailStatus) : base(identity)
        {
            Identity = identity;
            OrderDocumentId = orderDocumentId.Value;
            WarpsOrigin = warpsOrigin.Serialize();
            WeftsOrigin = weftsOrigin.Serialize();
            BeamDocumentId = beamDocumentId.Value;
            DOMTime = domTime.Serialize();
            ShiftDocumentId = shiftDocumentId.Value;
            BeamOperatorDocumentId = beamOperatorDocumentId.Value;
            SizingOperatorDocumentId = sizingOperatorDocumentId.Value;
            Information = information;
            DetailStatus = detailStatus;
        }

        public void SetOrderDocumentId(OrderId value)
        {
            if (!OrderDocumentId.Value.Equals(value.Value))
            {
                OrderDocumentId = value.Value;
                MarkModified();
            }
        }

        public void UpdateWarpsOrigin(List<Origin> warpValues)
        {
            WarpsOrigin = warpValues.Serialize();
            MarkModified();
        }

        public void UpdateWeftOrigin(List<Origin> weftValues)
        {
            WeftsOrigin = weftValues.Serialize();
            MarkModified();
        }

        public void SetBeamDocumentId(BeamId value)
        {
            if (!BeamDocumentId.Value.Equals(value.Value))
            {
                BeamDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetDomTime(DailyOperationLoomTimeValueObject value)
        {
            DOMTime = value.Serialize();
            MarkModified();
        }

        public void SetShiftDocumentId(ShiftId value)
        {
            if (!ShiftDocumentId.Value.Equals(value.Value))
            {
                ShiftDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetBeamOperatorDocumentId(OperatorId value)
        {
            if(!BeamOperatorDocumentId.Value.Equals(value.Value))
            {
                BeamOperatorDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetSizingOperatorDocumentId(OperatorId value)
        {
            if(!SizingOperatorDocumentId.Value.Equals(value.Value))
            {
                SizingOperatorDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            Information = information;
            MarkModified();
        }

        public void SetDetailStatus(string detailStatus)
        {
            DetailStatus = detailStatus;
            MarkModified();
        }

        protected override DailyOperationLoomDetail GetEntity()
        {
            return this;
        }
    }
}
