using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Sizing
{
    public class DailyOperationSizingDocument : AggregateRoot<DailyOperationSizingDocument, DailyOperationSizingDocumentReadModel>
    {
        public MachineId MachineDocumentId { get; private set; }
        public OrderId OrderDocumentId { get; private set; }
        public double EmptyWeight { get; private set; }
        public double YarnStrands { get; private set; }
        public string RecipeCode { get; private set; }
        public double NeReal { get; private set; }
        public int MachineSpeed { get; private set; }
        public int TexSQ { get; private set; }
        public int Visco { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public List<DailyOperationSizingBeamsWarping> BeamsWarping { get; private set; }
        public List<DailyOperationSizingBeamProduct> SizingBeamProducts { get; private set; }
        public List<DailyOperationSizingHistory> SizingHistories { get; private set; }

        public DailyOperationSizingDocument(Guid identity,
                                            MachineId machineDocumentId,
                                            OrderId orderDocumentId,
                                            double emptyWeight,
                                            double yarnStrands,
                                            string recipeCode,
                                            double neReal,
                                            DateTimeOffset datetimeOperation,
                                            string operationStatus) : base(identity)
        {
            Identity = identity;
            MachineDocumentId = machineDocumentId;
            OrderDocumentId = orderDocumentId;
            EmptyWeight = emptyWeight;
            YarnStrands = yarnStrands;
            RecipeCode = recipeCode;
            NeReal = neReal;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationSizingDocumentReadModel(Identity)
            {
                MachineDocumentId = this.MachineDocumentId.Value,
                OrderDocumentId = this.OrderDocumentId.Value,
                EmptyWeight = this.EmptyWeight,
                YarnStrands = this.YarnStrands,
                RecipeCode = this.RecipeCode,
                NeReal = this.NeReal,
                MachineSpeed = this.MachineSpeed,
                TexSQ = this.TexSQ,
                Visco = this.Visco,
                DateTimeOperation = this.DateTimeOperation,
                OperationStatus = this.OperationStatus,
            };
        }
        public DailyOperationSizingDocument(DailyOperationSizingDocumentReadModel readModel) : base(readModel)
        {
            this.MachineDocumentId = new MachineId(readModel.MachineDocumentId);
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.EmptyWeight = readModel.EmptyWeight;
            this.YarnStrands = readModel.YarnStrands;
            this.RecipeCode = readModel.RecipeCode;
            this.NeReal = readModel.NeReal;
            this.MachineSpeed = readModel.MachineSpeed.HasValue ? readModel.MachineSpeed.Value : 0;
            this.TexSQ = readModel.TexSQ.HasValue ? readModel.TexSQ.Value : 0;
            this.Visco = readModel.Visco.HasValue ? readModel.Visco.Value : 0;
            this.DateTimeOperation = readModel.DateTimeOperation;
            this.OperationStatus = readModel.OperationStatus;
        }

        //public void AddDailyOperationSizingHistory(DailyOperationSizingHistory sizingHistory)
        //{
        //    var list = SizingHistories.ToList();
        //    list.Add(sizingHistory);
        //    SizingHistories = list;
        //    ReadModel.SizingHistories = SizingHistories.ToList();

        //    MarkModified();
        //}

        //public void UpdateDailyOperationSizingHistory(DailyOperationSizingHistory history)
        //{
        //    var sizingHistories = SizingHistories.ToList();

        //    //Get Sizing Detail Update
        //    var index =
        //        sizingHistories
        //            .FindIndex(x => x.Identity.Equals(history.Identity));
        //    var sizingHistory =
        //        sizingHistories
        //            .Where(x => x.Identity.Equals(history.Identity))
        //            .FirstOrDefault();

        //    //Update Propertynya
        //    sizingHistory.SetShiftId(new ShiftId(history.ShiftDocumentId));
        //    sizingHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
        //    sizingHistory.SetDateTimeMachine(history.DateTimeMachine);
        //    sizingHistory.SetMachineStatus(history.MachineStatus);
        //    sizingHistory.SetInformation(history.Information);
        //    //sizingDetail.SetCauses(JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(detail.Causes));
        //    sizingHistory.SetBrokenBeam(history.BrokenBeam);
        //    sizingHistory.SetMachineTroubled(history.MachineTroubled);
        //    sizingHistory.SetSizingBeamNumber(history.SizingBeamNumber);

        //    sizingHistories[index] = sizingHistory;
        //    SizingHistories = sizingHistories;
        //    ReadModel.SizingHistories = sizingHistories;
        //    MarkModified();
        //}

        //public void RemoveDailyOperationSizingHistory(Guid identity)
        //{
        //    var history = SizingHistories.Where(o => o.Identity == identity).FirstOrDefault();
        //    var list = SizingHistories.ToList();

        //    list.Remove(history);
        //    SizingHistories = list;
        //    ReadModel.SizingHistories = SizingHistories.ToList();

        //    MarkModified();
        //}

        //public void AddDailyOperationSizingBeamProduct(DailyOperationSizingBeamProduct sizingBeamProduct)
        //{
        //    var list = SizingBeamProducts.ToList();
        //    list.Add(sizingBeamProduct);
        //    SizingBeamProducts = list;
        //    ReadModel.SizingBeamProducts = SizingBeamProducts.ToList();

        //    MarkModified();
        //}

        //public void UpdateDailyOperationSizingBeamProduct(DailyOperationSizingBeamProduct beamProduct)
        //{
        //    var sizingBeamProducts = SizingBeamProducts.ToList();

        //    //Get Sizing Beam Update
        //    var index =
        //        sizingBeamProducts
        //            .FindIndex(x => x.Identity.Equals(beamProduct.Identity));
        //    var sizingBeamProduct =
        //        sizingBeamProducts
        //            .Where(x => x.Identity.Equals(beamProduct.Identity))
        //            .FirstOrDefault();

        //    //Update Propertynya
        //    sizingBeamProduct.SetSizingBeamId(beamProduct.SizingBeamId);
        //    sizingBeamProduct.SetLatestDateTimeBeamProduct(beamProduct.LatestDateTimeBeamProduct);
        //    sizingBeamProduct.SetCounterStart(beamProduct.CounterStart ??0);
        //    sizingBeamProduct.SetCounterFinish(beamProduct.CounterFinish ?? 0);
        //    sizingBeamProduct.SetWeightNetto(beamProduct.WeightNetto ?? 0);
        //    sizingBeamProduct.SetWeightBruto(beamProduct.WeightBruto ?? 0);
        //    sizingBeamProduct.SetWeightTheoritical(beamProduct.WeightTheoritical ?? 0);
        //    sizingBeamProduct.SetPISMeter(beamProduct.PISMeter ?? 0);
        //    sizingBeamProduct.SetSPU(beamProduct.SPU ?? 0);
        //    sizingBeamProduct.SetSizingBeamStatus(beamProduct.BeamStatus);

        //    sizingBeamProducts[index] = sizingBeamProduct;
        //    SizingBeamProducts = sizingBeamProducts;
        //    ReadModel.SizingBeamProducts = sizingBeamProducts;
        //    MarkModified();
        //}

        //public void RemoveDailyOperationSizingBeamProduct(Guid identity)
        //{
        //    var beamProduct = SizingBeamProducts.Where(o => o.Identity == identity).FirstOrDefault();
        //    var list = SizingBeamProducts.ToList();

        //    list.Remove(beamProduct);
        //    SizingBeamProducts = list;
        //    ReadModel.SizingBeamProducts = SizingBeamProducts.ToList();

        //    MarkModified();
        //}

        public void SetEmptyWeight(double emptyWeight)
        {
            if (emptyWeight != EmptyWeight)
            {
                EmptyWeight = emptyWeight;
                ReadModel.EmptyWeight = EmptyWeight;
                MarkModified();
            }
        }

        public void SetYarnStrands(double yarnStrands)
        {
            if (yarnStrands != YarnStrands)
            {
                YarnStrands = yarnStrands;
                ReadModel.YarnStrands = YarnStrands;

                MarkModified();
            }
        }

        public void SetRecipeCode(string recipeCode)
        {
            Validator.ThrowIfNull(() => recipeCode);
            if (recipeCode != RecipeCode)
            {
                RecipeCode = recipeCode;
                ReadModel.RecipeCode = RecipeCode;

                MarkModified();
            }
        }

        public void SetNeReal(double neReal)
        {
            if (neReal != NeReal)
            {
                NeReal = neReal;
                ReadModel.NeReal = NeReal;

                MarkModified();
            }
        }

        public void SetMachineSpeed(int machineSpeed)
        {
            if (machineSpeed != MachineSpeed)
            {
                MachineSpeed = machineSpeed;
                ReadModel.MachineSpeed = MachineSpeed;

                MarkModified();
            }
        }

        public void SetTexSQ(int texSQ)
        {
            if (texSQ != TexSQ)
            {
                TexSQ = texSQ;
                ReadModel.TexSQ = TexSQ;

                MarkModified();
            }
        }

        public void SetVisco(int visco)
        {
            if (visco != Visco)
            {
                Visco = visco;
                ReadModel.Visco = Visco;

                MarkModified();
            }
        }

        public void SetDateTimeOperation(DateTimeOffset dateTimeOperation)
        {
            if (dateTimeOperation != DateTimeOperation)
            {
                DateTimeOperation = dateTimeOperation;
                ReadModel.DateTimeOperation = DateTimeOperation;

                MarkModified();
            }
        }

        public void SetOperationStatus(string operationStatus)
        {
            Validator.ThrowIfNull(() => operationStatus);
            if (operationStatus != OperationStatus)
            {
                OperationStatus = operationStatus;
                ReadModel.OperationStatus = OperationStatus;

                MarkModified();
            }
        }

        protected override DailyOperationSizingDocument GetEntity()
        {
            return this;
        }
    }
}
