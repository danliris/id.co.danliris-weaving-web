using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.ReadModels;
using Manufactures.Domain.DailyOperations.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Entities
{
    public class DailyOperationMachineDetail : EntityBase<DailyOperationMachineDetail>
    {
        public string OrderDocument { get; private set; }
        public string Shift { get; private set; }
        public string Beam { get; private set; }
        public string DOMTime { get; private set; }

        public Guid DailyOperationMachineDocumentId { get; set; }
        public DailyOperationMachineDocumentReadModel DailyOperationMachineDocument { get; set; }

        public DailyOperationMachineDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationMachineDetail(Guid identity, string orderDocument, string shift, string beam, DOMTimeValueObject domTime) : base(identity)
        {
            Identity = identity;
            OrderDocument = orderDocument;
            Shift = shift;
            Beam = beam;
            DOMTime = domTime.Serialize();
        }

        public void SetShift(string shift)
        {
            Validator.ThrowIfNull(() => shift);

            MarkModified();
        }

        public void SetBeam(string beam)
        {
            Validator.ThrowIfNull(() => beam);

            MarkModified();
        }

        public void SetDomTime (DOMTimeValueObject domTime)
        {
            Validator.ThrowIfNull(() => domTime);

            DOMTime = domTime.Serialize();

            MarkModified();
        }

        protected override DailyOperationMachineDetail GetEntity()
        {
            return this;
        }
    }
}
