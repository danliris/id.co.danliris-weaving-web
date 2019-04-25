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
        public Guid? BeamDocumentId { get; private set; }
        public Guid? ConstructionDocumentId { get; private set; }
        public int PIS { get; private set; }
        public string Visco { get; private set; }
        public string ProductionTime { get; private set; }
        public string BeamTime { get; private set; }
        public int BrokenBeam { get; private set; }
        public int TroubledMachine { get; private set; }
        public double Counter { get; private set; }
        public Guid? ShiftDocumentId { get; private set; }
        public string Information { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }
        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingDetail(Guid identity, BeamId beamDocumentId, ConstructionId constructionDocumentId, int pis, string visco, DailyOperationSizingProductionTimeValueObject productionTime, DailyOperationSizingBeamTimeValueObject beamTime, int brokenBeam, int troubledMachine, double counter, ShiftId shiftDocumentId, string information, Guid dailyOperationSizingDocumentId) : base(identity)
        {
            BeamDocumentId = beamDocumentId.Value;
            ConstructionDocumentId = constructionDocumentId.Value;
            PIS = pis;
            Visco = visco;
            ProductionTime = productionTime.Serialize();
            BeamTime = beamTime.Serialize();
            BrokenBeam = brokenBeam;
            TroubledMachine = troubledMachine;
            Counter = counter;
            ShiftDocumentId = shiftDocumentId.Value;
            Information = information;
        }

        public void SetBeamDocumentId(BeamId value)
        {
            if (!BeamDocumentId.Value.Equals(value.Value))
            {
                BeamDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetConstructionDocumentId(ConstructionId value)
        {
            if (!ConstructionDocumentId.Value.Equals(value.Value))
            {
                ConstructionDocumentId = value.Value;
                MarkModified();
            }
        }

        public void SetPIS(int pis)
        {
            PIS = pis;
            MarkModified();
        }

        public void SetVisco(string visco)
        {
            Visco = visco;
            MarkModified();
        }

        public void SetProductionTime(DailyOperationSizingProductionTimeValueObject value)
        {
            ProductionTime = value.Serialize();
            MarkModified();
        }

        public void SetBeamTime(DailyOperationSizingBeamTimeValueObject value)
        {
            BeamTime = value.Serialize();
            MarkModified();
        }

        public void SetBrokenBeam(int brokenBeam)
        {
            BrokenBeam = brokenBeam;
            MarkModified();
        }

        public void SetTroubledMachine(int troubledMachine)
        {
            TroubledMachine = troubledMachine;
            MarkModified();
        }

        public void SetCounter(double counter)
        {
            Counter = counter;
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

        public void SetInformation(string information)
        {
            Information = information;
            MarkModified();
        }

        protected override DailyOperationSizingDetail GetEntity()
        {
            return this;
        }
    }
}
