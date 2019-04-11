using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.DailyOperations.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Entities
{
    public class DailyOperationalMachineDetail : EntityBase<DailyOperationalMachineDetail>
    {
        public Guid? OrderDocumentId { get; private set; }
        public string WarpsOrigin { get; private set; }
        public string WeftsOrigin { get; private set; }
        public Guid? BeamDocumentId { get; private set; }
        public string DOMTime { get; private set; }
        public Guid? ShiftDocumentId { get; private set; }
        public Guid? BeamOperatorDocumentId { get; private set; }
        public Guid? SizingOperatorDocumentId { get; private set; }
        public string LoomGroup { get; private set; }
        public string SizingGroup { get; private set; }
        public string Information { get; private set; }
        public string DetailStatus { get; private set; }

        public Guid DailyOperationMachineDocumentId { get; set; }
        public DailyOperationalMachineDocumentReadModel DailyOperationMachineDocument { get; set; }

        public DailyOperationalMachineDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationalMachineDetail(Guid identity, Guid orderDocumentId, List<Origin> warpsOrigin, List<Origin> weftsOrigin, Guid beamDocumentId, DOMTimeValueObject domTime, Guid shiftDocumentId, Guid beamOperatorDocumentId, Guid sizingOperatorDocumentId, string loomGroup, string sizingGroup, string information, string detailStatus) : base(identity)
        {
            Identity = identity;
            OrderDocumentId = orderDocumentId;
            WarpsOrigin = warpsOrigin.Serialize();
            WeftsOrigin = weftsOrigin.Serialize();
            BeamDocumentId = beamDocumentId;
            DOMTime = domTime.Serialize();
            ShiftDocumentId = shiftDocumentId;
            BeamOperatorDocumentId = beamOperatorDocumentId;
            SizingOperatorDocumentId = sizingOperatorDocumentId;
            LoomGroup = loomGroup;
            SizingGroup = sizingGroup;
            Information = information;
            DetailStatus = detailStatus;
        }

        public void SetOrderDocumentId(Guid orderDocumentId)
        {
            Validator.ThrowIfNull(() => orderDocumentId);

            MarkModified();
        }

        public void SetWarpOrigin(Origin warpOrigin)
        {
            Validator.ThrowIfNull(() => warpOrigin);

            MarkModified();
        }

        public void SetWeftOrigin(Origin weftOrigin)
        {
            Validator.ThrowIfNull(() => weftOrigin);

            MarkModified();
        }

        public void SetBeamDocumentId(Guid beamDocumentId)
        {
            Validator.ThrowIfNull(() => beamDocumentId);

            MarkModified();
        }

        public void SetDomTime(DOMTimeValueObject domTime)
        {
            Validator.ThrowIfNull(() => domTime);

            DOMTime = domTime.Serialize();

            MarkModified();
        }

        public void SetShiftDocumentId(Guid shiftDocumentId)
        {
            Validator.ThrowIfNull(() => shiftDocumentId);

            MarkModified();
        }

        public void SetBeamOperatorDocumentId(Guid beamOperatorDocumentId)
        {
            Validator.ThrowIfNull(() => beamOperatorDocumentId);

            MarkModified();
        }

        public void SetSizingOperatorDocumentId(Guid sizingOperatorDocumentId)
        {
            Validator.ThrowIfNull(() => sizingOperatorDocumentId);

            MarkModified();
        }

        public void SetLoomGroup(string loomGroup)
        {
            Validator.ThrowIfNull(() => loomGroup);

            MarkModified();
        }

        public void SetSizingGroup(string sizingGroup)
        {
            Validator.ThrowIfNull(() => sizingGroup);

            MarkModified();
        }

        public void SetInformation(string information)
        {
            Validator.ThrowIfNull(() => information);

            MarkModified();
        }

        public void SetDetailStatus(string detailStatus)
        {
            Validator.ThrowIfNull(() => detailStatus);

            MarkModified();
        }

        protected override DailyOperationalMachineDetail GetEntity()
        {
            return this;
        }
    }
}
