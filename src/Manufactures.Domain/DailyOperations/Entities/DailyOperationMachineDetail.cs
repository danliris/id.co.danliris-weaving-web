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
    public class DailyOperationMachineDetail : EntityBase<DailyOperationMachineDetail>
    {
        public Guid OrderDocumentId { get; private set; }
        public string Shift { get; private set; }
        public string DOMTime { get; private set; }
        public string BeamOperator { get; private set; }
        public string LoomGroup { get; private set; }
        public string SizingNumber { get; private set; }
        public string SizingOperator { get; private set; }
        public string SizingGroup { get; private set; }
        public string Information { get; private set; }

        public Guid DailyOperationMachineDocumentId { get; set; }
        public DailyOperationMachineDocumentReadModel DailyOperationMachineDocument { get; set; }

        public DailyOperationMachineDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationMachineDetail(Guid identity, OrderDocumentId orderDocumentId, string shift, DOMTimeValueObject domTime, string beamOperator, string loomGroup, string sizingNumber, string sizingOperator, string sizingGroup, string information) : base(identity)
        {
            Identity = identity;
            OrderDocumentId = orderDocumentId.Value;
            Shift = shift;
            DOMTime = domTime.Serialize();
            BeamOperator = beamOperator;
            LoomGroup = loomGroup;
            SizingNumber = sizingNumber;
            SizingOperator = sizingOperator;
            SizingGroup = sizingGroup;
            Information = information;
        }

        public void SetOrderDocumentId(OrderDocumentId orderDocumentId)
        {
            Validator.ThrowIfNull(() => orderDocumentId);

            MarkModified();
        }

        public void SetShift(string shift)
        {
            Validator.ThrowIfNull(() => shift);

            MarkModified();
        }
        public void SetDomTime(DOMTimeValueObject domTime)
        {
            Validator.ThrowIfNull(() => domTime);

            DOMTime = domTime.Serialize();

            MarkModified();
        }

        public void SetBeamOperator(string beamOperator)
        {
            Validator.ThrowIfNull(() => beamOperator);

            MarkModified();
        }

        public void SetLoomGroup(string loomGroup)
        {
            Validator.ThrowIfNull(() => loomGroup);

            MarkModified();
        }

        public void SetSizingNumber(string sizingNumber)
        {
            Validator.ThrowIfNull(() => sizingNumber);

            MarkModified();
        }

        public void SetSizingOperator(string sizingOperator)
        {
            Validator.ThrowIfNull(() => sizingOperator);

            MarkModified();
        }

        public void SetSizingGroup(string sizingGroup)
        {
            Validator.ThrowIfNull(() => sizingGroup);

            MarkModified();
        }

        protected override DailyOperationMachineDetail GetEntity()
        {
            return this;
        }
    }
}
