using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
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
        public OrderId OrderDocumentId { get; private set; }
        public List<BeamId> BeamsWarping { get; private set; }
        public double EmptyWeight { get; private set; }
        public double YarnStrands { get; private set; }
        public string RecipeCode { get; private set; }
        public double NeReal { get; private set; }
        public int MachineSpeed { get; private set; }
        public string TexSQ { get; private set; }
        public string Visco { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingBeamProduct> SizingBeamProducts { get; private set; }
        public IReadOnlyCollection<DailyOperationSizingHistory> SizingHistories { get; private set; }

        public DailyOperationSizingDocument(Guid id, 
                                            MachineId machineDocumentId, 
                                            OrderId orderDocumentId, 
                                            List<BeamId> beamsWarping, 
                                            double emptyWeight, 
                                            double yarnStrands, 
                                            string recipeCode, 
                                            double neReal, 
                                            int machineSpeed, 
                                            string texSQ, 
                                            string visco,
                                            DateTimeOffset datetimeOperation,
                                            string operationStatus) :base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            OrderDocumentId = orderDocumentId;
            BeamsWarping = beamsWarping;
            EmptyWeight = emptyWeight;
            YarnStrands = yarnStrands;
            RecipeCode = recipeCode;
            NeReal = neReal;
            MachineSpeed = machineSpeed;
            TexSQ = texSQ;
            Visco = visco;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;
            SizingBeamProducts = new List<DailyOperationSizingBeamProduct>();
            SizingHistories = new List<DailyOperationSizingHistory>();

            this.MarkTransient();

            ReadModel = new DailyOperationSizingReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                OrderDocumentId = this.OrderDocumentId.Value,
                BeamsWarping = JsonConvert.SerializeObject(this.BeamsWarping),
                EmptyWeight = this.EmptyWeight,
                YarnStrands = this.YarnStrands,
                RecipeCode = this.RecipeCode,
                NeReal = this.NeReal,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                DateTimeOperation = this.DateTimeOperation,
                OperationStatus = this.OperationStatus,
                SizingBeamProducts = this.SizingBeamProducts.ToList(),
                SizingHistories = this.SizingHistories.ToList()
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingReadModel readModel) : base(readModel)
        {
            this.MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            this.OrderDocumentId = readModel.OrderDocumentId.HasValue ? new OrderId(readModel.OrderDocumentId.Value) : null;
            this.BeamsWarping = JsonConvert.DeserializeObject<List<BeamId>>(readModel.BeamsWarping);
            this.EmptyWeight = readModel.EmptyWeight;
            this.YarnStrands = readModel.YarnStrands;
            this.RecipeCode = readModel.RecipeCode;
            this.NeReal = readModel.NeReal;
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ;
            this.Visco = readModel.Visco;
            this.DateTimeOperation = readModel.DateTimeOperation;
            this.OperationStatus = readModel.OperationStatus;
            this.SizingBeamProducts = readModel.SizingBeamProducts;
            this.SizingHistories = readModel.SizingHistories;
        }

        public void AddDailyOperationSizingDetail(DailyOperationSizingHistory sizingDetail)
        {
            var list = SizingHistories.ToList();
            list.Add(sizingDetail);
            SizingHistories = list;
            ReadModel.SizingHistories = SizingHistories.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationSizingDetail(DailyOperationSizingHistory detail)
        {
            var sizingDetails = SizingHistories.ToList();

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
            //sizingDetail.SetCauses(JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(detail.Causes));
            sizingDetail.SetBrokenBeam(detail.BrokenBeam);
            sizingDetail.SetMachineTroubled(detail.MachineTroubled);
            sizingDetail.SetSizingBeamNumber(detail.SizingBeamNumber);

            sizingDetails[index] = sizingDetail;
            SizingHistories = sizingDetails;
            ReadModel.SizingHistories = sizingDetails;
            MarkModified();
        }

        public void RemoveDailyOperationSizingDetail(Guid identity)
        {
            var detail = SizingHistories.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingHistories.ToList();

            list.Remove(detail);
            SizingHistories = list;
            ReadModel.SizingHistories = SizingHistories.ToList();

            MarkModified();
        }

        public void AddDailyOperationSizingBeamDocument(DailyOperationSizingBeamProduct sizingBeamDocument)
        {
            var list = SizingBeamProducts.ToList();
            list.Add(sizingBeamDocument);
            SizingBeamProducts = list;
            ReadModel.SizingBeamProducts = SizingBeamProducts.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationSizingBeamDocument(DailyOperationSizingBeamProduct beamDocument)
        {
            var sizingBeamDocuments = SizingBeamProducts.ToList();

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
            sizingBeamDocument.SetLatestDateTimeBeamProduct(beamDocument.LatestDateTimeBeamProduct);
            sizingBeamDocument.SetCounterStart(beamDocument.CounterStart ??0);
            sizingBeamDocument.SetCounterFinish(beamDocument.CounterFinish ?? 0);
            sizingBeamDocument.SetWeightNetto(beamDocument.WeightNetto ?? 0);
            sizingBeamDocument.SetWeightBruto(beamDocument.WeightBruto ?? 0);
            sizingBeamDocument.SetWeightTheoritical(beamDocument.WeightTheoritical ?? 0);
            sizingBeamDocument.SetPISMeter(beamDocument.PISMeter ?? 0);
            sizingBeamDocument.SetSPU(beamDocument.SPU ?? 0);
            sizingBeamDocument.SetSizingBeamStatus(beamDocument.BeamStatus);

            sizingBeamDocuments[index] = sizingBeamDocument;
            SizingBeamProducts = sizingBeamDocuments;
            ReadModel.SizingBeamProducts = sizingBeamDocuments;
            MarkModified();
        }

        public void RemoveDailyOperationSizingBeamDocument(Guid identity)
        {
            var detail = SizingBeamProducts.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingBeamProducts.ToList();

            list.Remove(detail);
            SizingBeamProducts = list;
            ReadModel.SizingBeamProducts = SizingBeamProducts.ToList();

            MarkModified();
        }

        public void SetEmptyWeight(double emptyWeight)
        {
            EmptyWeight = emptyWeight;
            ReadModel.EmptyWeight = emptyWeight;
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

        public void SetTexSQ(string texSQ)
        {
            TexSQ = texSQ;
            ReadModel.TexSQ = texSQ;
            MarkModified();
        }

        public void SetVisco(string visco)
        {
            Visco = visco;
            ReadModel.Visco = visco;
            MarkModified();
        }

        public void SetDateTimeOperation(DateTimeOffset value)
        {
            if (!DateTimeOperation.Equals(value))
            {
                DateTimeOperation = value;
                ReadModel.DateTimeOperation = DateTimeOperation;

                MarkModified();
            }
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
