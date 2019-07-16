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
        public List<DailyOperationSizingBeamsCollectionValueObject> BeamsWarping { get; private set; }
        public string RecipeCode { get; private set; }
        public double NeReal { get; private set; }
        public int MachineSpeed { get; private set; }
        public double TexSQ { get; private set; }
        public double Visco { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingBeamDocument> SizingBeamDocuments { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingDetail> SizingDetails { get; private set; }

        public DailyOperationSizingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, List<DailyOperationSizingBeamsCollectionValueObject> beamsWarping, string recipeCode, double neReal, int machineSpeed, double texSQ, double visco, string operationStatus) :base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            BeamsWarping = beamsWarping;
            RecipeCode = recipeCode;
            NeReal = neReal;
            MachineSpeed = machineSpeed;
            TexSQ = texSQ;
            Visco = visco;
            OperationStatus = operationStatus;
            SizingBeamDocuments = new List<DailyOperationSizingBeamDocument>();
            SizingDetails = new List<DailyOperationSizingDetail>();

            this.MarkTransient();

            ReadModel = new DailyOperationSizingReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                ConstructionDocumentId = this.ConstructionDocumentId.Value,
                BeamsWarping = JsonConvert.SerializeObject(this.BeamsWarping),
                RecipeCode = this.RecipeCode,
                NeReal = this.NeReal,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                OperationStatus = this.OperationStatus,
                SizingBeamDocuments = this.SizingBeamDocuments.ToList(),
                Details = this.SizingDetails.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.ConstructionDocumentId = readModel.ConstructionDocumentId.HasValue ? new ConstructionId(readModel.ConstructionDocumentId.Value) : null;
            this.BeamsWarping = JsonConvert.DeserializeObject<List<DailyOperationSizingBeamsCollectionValueObject>>(readModel.BeamsWarping);
            this.RecipeCode = readModel.RecipeCode;
            this.NeReal = readModel.NeReal;
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ.HasValue ? readModel.TexSQ.Value : 0;
            this.Visco = readModel.Visco.HasValue ? readModel.Visco.Value : 0;
            this.OperationStatus = readModel.OperationStatus;
            this.SizingBeamDocuments = readModel.SizingBeamDocuments;
            this.SizingDetails = readModel.Details;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingDetail dailyOperationSizingDetail)
        {
            var list = SizingDetails.ToList();
            list.Add(dailyOperationSizingDetail);
            SizingDetails = list;
            ReadModel.Details = SizingDetails.ToList();

            MarkModified();
        }

        public void RemoveDailyOperationMachineDetail(Guid identity)
        {
            var detail = SizingDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingDetails.ToList();

            list.Remove(detail);
            SizingDetails = list;
            ReadModel.Details = SizingDetails.ToList();

            MarkModified();
        }

        public void AddSizingBeam(DailyOperationSizingBeamDocument sizingBeamDocument)
        {
            var list = SizingBeamDocuments.ToList();
            list.Add(sizingBeamDocument);
            SizingBeamDocuments = list;
            ReadModel.SizingBeamDocuments = SizingBeamDocuments.ToList();

            MarkModified();
        }

        public void SetRecipeCode(string recipeCode)
        {
            RecipeCode = recipeCode;
            ReadModel.RecipeCode = recipeCode;
            MarkModified();
        }

        public void SetNeReal(double neReal)
        {
            NeReal = neReal;
            ReadModel.NeReal = neReal;
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
