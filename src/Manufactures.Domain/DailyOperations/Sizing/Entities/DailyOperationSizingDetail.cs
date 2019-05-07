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
        public Guid ConstructionDocumentId { get; private set; }
        public string Shift { get; private set; }
        public Guid OperatorDocumentId { get; private set; }
        public string ProductionTime { get; private set; }
        public string Counter { get; private set; }
        public string Weight { get; private set; }
        public string WarpingBeamCollectionDocumentId { get; private set; }
        public string Causes { get; private set; }
        public string Information { get; private set; }
        public int? MachineSpeed { get; private set; }
        public double? TexSQ { get; private set; }
        public double? Visco { get; private set; }
        public int? PIS { get; private set; }
        public double? SPU { get; private set; }
        public Guid? SizingBeamDocumentId { get; private set; }

        public Guid DailyOperationSizingDocumentId { get; set; }
        public DailyOperationSizingReadModel DailyOperationSizingDocument { get; set; }

        public DailyOperationSizingDetail(Guid identity) : base(identity)
        {
        }

        public DailyOperationSizingDetail(Guid identity, ConstructionId constructionDocumentId, string shift, OperatorId operatorDocumentId, DailyOperationSizingProductionTimeValueObject productionTime, DailyOperationSizingCounterValueObject counter, DailyOperationSizingWeightValueObject weight, List<string> warpingBeamsDocumentId, DailyOperationSizingCausesValueObject causes, string information, int machineSpeed, double texSQ, double visco, int pis, double spu, BeamId sizingBeamDocumentId) : base(identity)
        {
            ConstructionDocumentId = constructionDocumentId.Value;
            Shift = shift;
            OperatorDocumentId = operatorDocumentId.Value;
            ProductionTime = productionTime.Serialize();
            Counter = counter.Serialize();
            Weight = weight.Serialize();
            WarpingBeamCollectionDocumentId = warpingBeamsDocumentId.Serialize();
            Causes = causes.Serialize();
            Information = information;
            MachineSpeed = machineSpeed;
            TexSQ = texSQ;
            Visco = visco;
            PIS = pis;
            SPU = spu;
            SizingBeamDocumentId = sizingBeamDocumentId.Value;
        }

        public void SetConstructionDocumentId(ConstructionId constructionDocumentId)
        {
            if (!ConstructionDocumentId.Equals(constructionDocumentId.Value))
            {
                ConstructionDocumentId = constructionDocumentId.Value;
                MarkModified();
            }
        }

        public void SetShift(string shift)
        {
                Shift = shift;
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

        public void SetProductionTime(DailyOperationSizingProductionTimeValueObject productionTime)
        {
            ProductionTime = productionTime.Serialize();
            MarkModified();
        }

        public void SetCounter(DailyOperationSizingCounterValueObject counter)
        {
            Counter = counter.Serialize();
            MarkModified();
        }

        public void SetWeight(DailyOperationSizingWeightValueObject weight)
        {
            Weight = weight.Serialize();
            MarkModified();
        }

        public void SetWarpingBeamsDocumentId(List<string> warpingBeamsDocumentId)
        {
            WarpingBeamCollectionDocumentId = warpingBeamsDocumentId.Serialize();
            MarkModified();
        }

        public void SetCauses(DailyOperationSizingCausesValueObject causes)
        {
            Causes = causes.Serialize();
            MarkModified();
        }

        public void SetInformation(string information)
        {
            Information = information;
            MarkModified();
        }

        public void SetMachineSpeed(int machineSpeed)
        {
            MachineSpeed = machineSpeed;
            MarkModified();
        }

        public void SetTexSQ(double texSQ)
        {
            TexSQ = texSQ;
            MarkModified();
        }

        public void SetVisco(double visco)
        {
            Visco = visco;
            MarkModified();
        }

        public void SetPIS(int pis)
        {
            PIS = pis;
            MarkModified();
        }

        public void SetSPU(double spu)
        {
            SPU = spu;
            MarkModified();
        }

        public void SetSizingBeamDocumentId(BeamId sizingBeamDocumentId)
        {
            if (!SizingBeamDocumentId.Equals(sizingBeamDocumentId.Value))
            {
                SizingBeamDocumentId = sizingBeamDocumentId.Value;
                MarkModified();
            }
        }

        protected override DailyOperationSizingDetail GetEntity()
        {
            return this;
        }
    }
}
