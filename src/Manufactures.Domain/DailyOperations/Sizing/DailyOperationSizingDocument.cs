using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
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
        public List<BeamId> BeamsWarping { get; private set; }
        public double YarnStrands { get; private set; }
        public string RecipeCode { get; private set; }
        public double NeReal { get; private set; }
        public int MachineSpeed { get; private set; }
        public double TexSQ { get; private set; }
        public double Visco { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingBeamDocument> SizingBeamDocuments { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingDetail> SizingDetails { get; private set; }

        public DailyOperationSizingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, List<BeamId> beamsWarping, double yarnStrands, string recipeCode, double neReal, int machineSpeed, double texSQ, double visco, string operationStatus) :base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            BeamsWarping = beamsWarping;
            YarnStrands = yarnStrands;
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
                YarnStrands = this.YarnStrands,
                RecipeCode = this.RecipeCode,
                NeReal = this.NeReal,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                OperationStatus = this.OperationStatus,
                SizingBeamDocuments = this.SizingBeamDocuments.ToList(),
                SizingDetails = this.SizingDetails.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.ConstructionDocumentId = readModel.ConstructionDocumentId.HasValue ? new ConstructionId(readModel.ConstructionDocumentId.Value) : null;
            this.BeamsWarping = JsonConvert.DeserializeObject<List<BeamId>>(readModel.BeamsWarping);
            this.YarnStrands = readModel.YarnStrands;
            this.RecipeCode = readModel.RecipeCode;
            this.NeReal = readModel.NeReal;
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ.HasValue ? readModel.TexSQ.Value : 0;
            this.Visco = readModel.Visco.HasValue ? readModel.Visco.Value : 0;
            this.OperationStatus = readModel.OperationStatus;
            this.SizingBeamDocuments = readModel.SizingBeamDocuments;
            this.SizingDetails = readModel.SizingDetails;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingDetail sizingDetail)
        {
            var list = SizingDetails.ToList();
            list.Add(sizingDetail);
            SizingDetails = list;
            ReadModel.SizingDetails = SizingDetails.ToList();

            MarkModified();
        }

        public void UpdateSizingDetail(DailyOperationSizingDetail detail)
        {
            var sizingDetails = SizingDetails.ToList();

            //Get Sizing Detail Update
            var index =
                sizingDetails
                    .FindIndex(x => x.Identity.Equals(detail.Identity));
            var sizingDetail =
                sizingDetails
                    .Where(x => x.Identity.Equals(detail.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            sizingDetail.SetShiftId(new ShiftId(detail.ShiftDocumentId));
            sizingDetail.SetOperatorDocumentId(new OperatorId(detail.OperatorDocumentId));
            sizingDetail.SetDateTimeMachine(detail.DateTimeMachine);
            sizingDetail.SetMachineStatus(detail.MachineStatus);
            sizingDetail.SetInformation(detail.Information);
            sizingDetail.SetCauses(JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(detail.Causes));
            sizingDetail.SetSizingBeamNumber(detail.SizingBeamNumber);

            sizingDetails[index] = sizingDetail;
            SizingDetails = sizingDetails;
            ReadModel.SizingDetails = sizingDetails;
            MarkModified();
        }

        public void RemoveDailyOperationSizingDetail(Guid identity)
        {
            var detail = SizingDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingDetails.ToList();

            list.Remove(detail);
            SizingDetails = list;
            ReadModel.SizingDetails = SizingDetails.ToList();

            MarkModified();
        }

        public void AddDailyOperationSizingBeamDocument(DailyOperationSizingBeamDocument sizingBeamDocument)
        {
            var list = SizingBeamDocuments.ToList();
            list.Add(sizingBeamDocument);
            SizingBeamDocuments = list;
            ReadModel.SizingBeamDocuments = SizingBeamDocuments.ToList();

            MarkModified();
        }

        public void UpdateSizingBeamDocuments(DailyOperationSizingBeamDocument beamDocument)
        {
            var sizingBeamDocuments = SizingBeamDocuments.ToList();

            //Get Sizing Beam Update
            var index =
                sizingBeamDocuments
                    .FindIndex(x => x.Identity.Equals(beamDocument.Identity));
            var sizingBeamDocument =
                sizingBeamDocuments
                    .Where(x => x.Identity.Equals(beamDocument.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            sizingBeamDocument.SetSizingBeamId(beamDocument.SizingBeamId);
            sizingBeamDocument.SetDateTimeBeamDocument(beamDocument.DateTimeBeamDocument);
            sizingBeamDocument.SetCounter(JsonConvert.DeserializeObject<DailyOperationSizingCounterValueObject>(beamDocument.Counter));
            sizingBeamDocument.SetWeight(JsonConvert.DeserializeObject<DailyOperationSizingWeightValueObject>(beamDocument.Weight));
            sizingBeamDocument.SetPISMeter(beamDocument.PISMeter);
            sizingBeamDocument.SetSPU(beamDocument.SPU);
            sizingBeamDocument.SetSizingBeamStatus(beamDocument.SizingBeamStatus);

            sizingBeamDocuments[index] = sizingBeamDocument;
            SizingBeamDocuments = sizingBeamDocuments;
            ReadModel.SizingBeamDocuments = sizingBeamDocuments;
            MarkModified();
        }

        public void RemoveDailyOperationSizingBeamDocument(Guid identity)
        {
            var detail = SizingBeamDocuments.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingBeamDocuments.ToList();

            list.Remove(detail);
            SizingBeamDocuments = list;
            ReadModel.SizingBeamDocuments = SizingBeamDocuments.ToList();

            MarkModified();
        }

        public void SetYarnStrands(double yarnStrands)
        {
            YarnStrands = yarnStrands;
            ReadModel.YarnStrands = yarnStrands;
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
