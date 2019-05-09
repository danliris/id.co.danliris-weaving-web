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
        public Guid ShiftId { get; private set; }
        public Guid OperatorDocumentId { get; private set; }
        public string DailyOperationSizingHistory { get; private set; }
        public string Causes { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }
        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingDetail(Guid identity, ShiftId shiftId, OperatorId operatorDocumentId, DailyOperationSizingHistoryValueObject dailyOperationSizingHistory, DailyOperationSizingCausesValueObject causes) : base(identity)
        {
            ShiftId = shiftId.Value;
            OperatorDocumentId = operatorDocumentId.Value;
            DailyOperationSizingHistory = dailyOperationSizingHistory.Serialize();
            Causes = causes.Serialize();
        }

        public void SetShiftId(ShiftId shiftId)
        {
                ShiftId = shiftId.Value;
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

        public void SetDailyOperationSizingHistory(DailyOperationSizingHistoryValueObject dailyOperationSizingHistory)
        {
            DailyOperationSizingHistory = dailyOperationSizingHistory.Serialize();
            MarkModified();
        }

        public void SetCauses(DailyOperationSizingCausesValueObject causes)
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
