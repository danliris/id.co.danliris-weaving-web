using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Entities
{
    public class DailyOperationSizingDetail : EntityBase<DailyOperationSizingDetail>
    {
        public Guid ShiftDocumentId { get; private set; }

        public Guid OperatorDocumentId { get; private set; }

        public DateTimeOffset DateTimeOperation { get; private set; }

        public string MachineStatus { get; private set; }

        public string Information { get; private set; }

        public string Causes { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }

        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingDetail(Guid identity, 
                                          ShiftId shiftDocumentId, 
                                          OperatorId operatorDocumentId, 
                                          DateTimeOffset dateTimeOperation, 
                                          string machineStatus, 
                                          string information, 
                                          DailyOperationSizingCauseValueObject causes) : base(identity)
        {
            ShiftDocumentId = shiftDocumentId.Value;
            OperatorDocumentId = operatorDocumentId.Value;
            DateTimeOperation = dateTimeOperation;
            MachineStatus = machineStatus;
            Information = information;
            Causes = causes.Serialize();
        }

        public void SetShiftId(ShiftId shiftDocumentId)
        {
                ShiftDocumentId = shiftDocumentId.Value;
                MarkModified();
        }

        public void SetOperatorDocumentId(OperatorId operatorDocumentId)
        {
            if (!OperatorDocumentId.Equals(operatorDocumentId.Value))
            {
                OperatorDocumentId = operatorDocumentId.Value;
                MarkModified();
            }
        }

        public void SetDateTimeOperation(DateTimeOffset dateTimeOperation)
        {
            DateTimeOperation = dateTimeOperation;
            MarkModified();
        }

        public void SetMachineStatus(string machineStatus)
        {
            MachineStatus = machineStatus;
            MarkModified();
        }

        public void SetInformation(string information)
        {
            Information = information;
            MarkModified();
        }

        public void SetCauses(DailyOperationSizingCauseValueObject causes)
        {
            Causes = causes.Serialize();
            MarkModified();
        }        

        protected override DailyOperationSizingDetail GetEntity()
        {
            return this;
        }
    }
}
