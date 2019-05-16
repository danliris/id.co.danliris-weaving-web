using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing
{
    public class DailyOperationSizingDocument : AggregateRoot<DailyOperationSizingDocument, DailyOperationSizingReadModel>
    {
        public DateTimeOffset DateOperated { get; private set; }
        public MachineId MachineDocumentId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }
        public ConstructionId ConstructionDocumentId { get; private set; }
        public string Counter { get; private set; }
        public string Weight { get; private set; }
        public string WarpingBeamCollectionDocumentId { get; private set; }
        public int MachineSpeed { get; private set; }
        public double TexSQ { get; private set; }
        public double Visco { get; private set; }
        public int PIS { get; private set; }
        public double SPU { get; private set; }
        public BeamId SizingBeamDocumentId { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingDetail> DailyOperationSizingDetails { get; private set; }

        public DailyOperationSizingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, DailyOperationSizingCounterValueObject counter, DailyOperationSizingWeightValueObject weight, List<BeamId> warpingBeamsDocumentId, int machineSpeed, double texSQ, double visco, int pis, double spu, BeamId sizingBeamDocumentId) :base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            Counter = counter.Serialize();
            Weight = weight.Serialize();
            WarpingBeamCollectionDocumentId = warpingBeamsDocumentId.Serialize();
            MachineSpeed = machineSpeed;
            TexSQ = texSQ;
            Visco = visco;
            PIS = pis;
            SPU = spu;
            SizingBeamDocumentId = sizingBeamDocumentId;
            DailyOperationSizingDetails = new List<DailyOperationSizingDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationSizingReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                ConstructionDocumentId = this.ConstructionDocumentId.Value,
                Counter = this.Counter,
                Weight = this.Weight,
                WarpingBeamCollectionDocumentId = this.WarpingBeamCollectionDocumentId,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                PIS = this.PIS,
                SPU = this.SPU,
                SizingBeamDocumentId = this.SizingBeamDocumentId.Value,
                DailyOperationSizingDetails = this.DailyOperationSizingDetails.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.DateOperated = readModel.CreatedDate;
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.ConstructionDocumentId = readModel.ConstructionDocumentId.HasValue ? new ConstructionId(readModel.ConstructionDocumentId.Value) : null;
            this.Counter = readModel.Counter.Serialize();
            this.Weight = readModel.Weight.Serialize();
            this.WarpingBeamCollectionDocumentId = readModel.WarpingBeamCollectionDocumentId.Serialize();
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ.HasValue ? readModel.TexSQ.Value : 0;
            this.Visco = readModel.Visco.HasValue ? readModel.Visco.Value : 0;
            this.PIS = readModel.PIS.HasValue ? readModel.PIS.Value : 0;
            this.SPU = readModel.SPU.HasValue ? readModel.SPU.Value : 0;
            this.SizingBeamDocumentId = readModel.SizingBeamDocumentId.HasValue ? new BeamId(readModel.SizingBeamDocumentId.Value) : null;
            this.DailyOperationSizingDetails = readModel.DailyOperationSizingDetails;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingDetail dailyOperationSizingDetail)
        {
            var list = DailyOperationSizingDetails.ToList();
            list.Add(dailyOperationSizingDetail);
            DailyOperationSizingDetails = list;
            ReadModel.DailyOperationSizingDetails = DailyOperationSizingDetails.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = DailyOperationSizingDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = DailyOperationSizingDetails.ToList();

            list.Remove(detail);
            DailyOperationSizingDetails = list;
            ReadModel.DailyOperationSizingDetails = DailyOperationSizingDetails.ToList();

            MarkModified();
        }

        protected override DailyOperationSizingDocument GetEntity()
        {
            return this;
        }
    }
}
