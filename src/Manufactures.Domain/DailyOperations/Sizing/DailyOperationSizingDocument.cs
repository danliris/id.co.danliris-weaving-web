using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing
{
    public class DailyOperationSizingDocument : AggregateRoot<DailyOperationSizingDocument, DailyOperationSizingReadModel>
    {
        public MachineId MachineDocumentId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }
        public ConstructionId ConstructionDocumentId { get; private set; }
        public string RecipeCode { get; private set; }
        public DailyOperationSizingCounterValueObject Counter { get; private set; }
        public DailyOperationSizingWeightValueObject Weight { get; private set; }
        public List<BeamId> WarpingBeamsId { get; private set; }
        public int Cutmark { get; private set; }
        public int MachineSpeed { get; private set; }
        public double TexSQ { get; private set; }
        public double Visco { get; private set; }
        public int PIS { get; private set; }
        public double SPU { get; private set; }
        public double NeReal { get; private set; }
        public BeamId SizingBeamDocumentId { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingDetail> Details { get; private set; }

        public DailyOperationSizingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, string recipeCode, DailyOperationSizingCounterValueObject counter, DailyOperationSizingWeightValueObject weight, List<BeamId> warpingBeamsId, int cutmark, int machineSpeed, double texSQ, double visco, int pis, double spu, double neReal, BeamId sizingBeamDocumentId, string operationStatus) :base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            RecipeCode = recipeCode;
            Counter = counter;
            Weight = weight;
            WarpingBeamsId = warpingBeamsId;
            Cutmark = cutmark;
            MachineSpeed = machineSpeed;
            TexSQ = texSQ;
            Visco = visco;
            PIS = pis;
            SPU = spu;
            NeReal = neReal;
            SizingBeamDocumentId = sizingBeamDocumentId;
            OperationStatus = operationStatus;
            Details = new List<DailyOperationSizingDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationSizingReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                ConstructionDocumentId = this.ConstructionDocumentId.Value,
                RecipeCode = this.RecipeCode,
                Counter = this.Counter.Serialize(),
                Weight = this.Weight.Serialize(),
                WarpingBeamsId = this.WarpingBeamsId.Serialize(),
                Cutmark = this.Cutmark,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                PIS = this.PIS,
                SPU = this.SPU,
                NeReal = this.NeReal,
                SizingBeamDocumentId = this.SizingBeamDocumentId.Value,
                OperationStatus = this.OperationStatus,
                Details = this.Details.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.ConstructionDocumentId = readModel.ConstructionDocumentId.HasValue ? new ConstructionId(readModel.ConstructionDocumentId.Value) : null;
            this.RecipeCode = readModel.RecipeCode;
            this.Counter = JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(readModel.Counter);
            this.Weight = JsonConvert.DeserializeObject<DailyOperationSizingWeightValueObject>(readModel.Weight);
            this.WarpingBeamsId = readModel.WarpingBeamsId.Deserialize<List<BeamId>>();
            this.Cutmark = readModel.Cutmark;
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ.HasValue ? readModel.TexSQ.Value : 0;
            this.Visco = readModel.Visco.HasValue ? readModel.Visco.Value : 0;
            this.PIS = readModel.PIS.HasValue ? readModel.PIS.Value : 0;
            this.SPU = readModel.SPU.HasValue ? readModel.SPU.Value : 0;
            this.NeReal = readModel.NeReal.HasValue ? readModel.NeReal.Value : 0;
            this.SizingBeamDocumentId = readModel.SizingBeamDocumentId.HasValue ? new BeamId(readModel.SizingBeamDocumentId.Value) : null;
            this.OperationStatus = readModel.OperationStatus;
            this.Details = readModel.Details;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingDetail dailyOperationSizingDetail)
        {
            var list = Details.ToList();
            list.Add(dailyOperationSizingDetail);
            Details = list;
            ReadModel.Details = Details.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = Details.Where(o => o.Identity == identity).FirstOrDefault();
            var list = Details.ToList();

            list.Remove(detail);
            Details = list;
            ReadModel.Details = Details.ToList();

            MarkModified();
        }

        public void SetCounter(DailyOperationSizingCounterValueObject counter)
        {
            Counter = counter;
            ReadModel.Counter = counter.Serialize();
            MarkModified();
        }

        public void SetWeight(DailyOperationSizingWeightValueObject weight)
        {
            Weight = weight;
            ReadModel.Weight = weight.Serialize();
            MarkModified();
        }

        public void SetCutmark(int cutmark)
        {
            Cutmark = cutmark;
            ReadModel.Cutmark = cutmark;
            MarkModified();
        }

        public void SetMachineSpeed(int machineSpeed)
        {
            MachineSpeed = machineSpeed;
            ReadModel.MachineSpeed = machineSpeed;
            MarkModified();
        }

        public void SetTexSQ(double texSQ)
        {
            TexSQ = texSQ;
            ReadModel.TexSQ = texSQ;
            MarkModified();
        }

        public void SetVisco(double visco)
        {
            Visco = visco;
            ReadModel.Visco = visco;
            MarkModified();
        }

        public void SetPIS(int pis)
        {
            PIS = pis;
            ReadModel.PIS = pis;
            MarkModified();
        }

        public void SetSPU(double spu)
        {
            SPU = spu;
            ReadModel.SPU = spu;
            MarkModified();
        }

        public void SetNeReal(double neReal)
        {
            NeReal = neReal;
            ReadModel.NeReal = neReal;
            MarkModified();
        }

        public void SetSizingBeamDocumentId(BeamId sizingBeamDocumentId)
        {
            SizingBeamDocumentId = sizingBeamDocumentId;
            ReadModel.SizingBeamDocumentId = sizingBeamDocumentId.Value;
            MarkModified();
        }

        public void SetOperationStatus(string operationStatus)
        {
            OperationStatus = operationStatus;
            ReadModel.OperationStatus = operationStatus;
            MarkModified();
        }

        protected override DailyOperationSizingDocument GetEntity()
        {
            return this;
        }
    }
}
