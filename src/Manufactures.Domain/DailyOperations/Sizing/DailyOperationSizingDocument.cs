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
        public int TexSQ { get; private set; }
        public int Visco { get; private set; }
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
                                            int texSQ, 
                                            int visco,
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

        public void AddDailyOperationSizingHistory(DailyOperationSizingHistory sizingHistory)
        {
            var list = SizingHistories.ToList();
            list.Add(sizingHistory);
            SizingHistories = list;
            ReadModel.SizingHistories = SizingHistories.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationSizingHistory(DailyOperationSizingHistory history)
        {
            var sizingHistories = SizingHistories.ToList();

            //Get Sizing Detail Update
            var index =
                sizingHistories
                    .FindIndex(x => x.Identity.Equals(history.Identity));
            var sizingHistory =
                sizingHistories
                    .Where(x => x.Identity.Equals(history.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            sizingHistory.SetShiftId(new ShiftId(history.ShiftDocumentId));
            sizingHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
            sizingHistory.SetDateTimeMachine(history.DateTimeMachine);
            sizingHistory.SetMachineStatus(history.MachineStatus);
            sizingHistory.SetInformation(history.Information);
            //sizingDetail.SetCauses(JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(detail.Causes));
            sizingHistory.SetBrokenBeam(history.BrokenBeam);
            sizingHistory.SetMachineTroubled(history.MachineTroubled);
            sizingHistory.SetSizingBeamNumber(history.SizingBeamNumber);

            sizingHistories[index] = sizingHistory;
            SizingHistories = sizingHistories;
            ReadModel.SizingHistories = sizingHistories;
            MarkModified();
        }

        public void RemoveDailyOperationSizingHistory(Guid identity)
        {
            var history = SizingHistories.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingHistories.ToList();

            list.Remove(history);
            SizingHistories = list;
            ReadModel.SizingHistories = SizingHistories.ToList();

            MarkModified();
        }

        public void AddDailyOperationSizingBeamProduct(DailyOperationSizingBeamProduct sizingBeamProduct)
        {
            var list = SizingBeamProducts.ToList();
            list.Add(sizingBeamProduct);
            SizingBeamProducts = list;
            ReadModel.SizingBeamProducts = SizingBeamProducts.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationSizingBeamProduct(DailyOperationSizingBeamProduct beamProduct)
        {
            var sizingBeamProducts = SizingBeamProducts.ToList();

            //Get Sizing Beam Update
            var index =
                sizingBeamProducts
                    .FindIndex(x => x.Identity.Equals(beamProduct.Identity));
            var sizingBeamProduct =
                sizingBeamProducts
                    .Where(x => x.Identity.Equals(beamProduct.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            sizingBeamProduct.SetSizingBeamId(beamProduct.SizingBeamId);
            sizingBeamProduct.SetLatestDateTimeBeamProduct(beamProduct.LatestDateTimeBeamProduct);
            sizingBeamProduct.SetCounterStart(beamProduct.CounterStart ??0);
            sizingBeamProduct.SetCounterFinish(beamProduct.CounterFinish ?? 0);
            sizingBeamProduct.SetWeightNetto(beamProduct.WeightNetto ?? 0);
            sizingBeamProduct.SetWeightBruto(beamProduct.WeightBruto ?? 0);
            sizingBeamProduct.SetWeightTheoritical(beamProduct.WeightTheoritical ?? 0);
            sizingBeamProduct.SetPISMeter(beamProduct.PISMeter ?? 0);
            sizingBeamProduct.SetSPU(beamProduct.SPU ?? 0);
            sizingBeamProduct.SetSizingBeamStatus(beamProduct.BeamStatus);

            sizingBeamProducts[index] = sizingBeamProduct;
            SizingBeamProducts = sizingBeamProducts;
            ReadModel.SizingBeamProducts = sizingBeamProducts;
            MarkModified();
        }

        public void RemoveDailyOperationSizingBeamProduct(Guid identity)
        {
            var beamProduct = SizingBeamProducts.Where(o => o.Identity == identity).FirstOrDefault();
            var list = SizingBeamProducts.ToList();

            list.Remove(beamProduct);
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

        public void SetTexSQ(int texSQ)
        {
            TexSQ = texSQ;
            ReadModel.TexSQ = texSQ;
            MarkModified();
        }

        public void SetVisco(int visco)
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
