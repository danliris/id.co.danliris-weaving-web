using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Loom
{
    public class DailyOperationLoomDocument : AggregateRoot<DailyOperationLoomDocument, DailyOperationLoomDocumentReadModel>
    {

        public OrderId OrderDocumentId { get; private set; }

        public double TotalCounter { get; private set; }

        public int BeamProcessed { get; private set; }

        public string OperationStatus { get; private set; }

        public List<DailyOperationLoomBeamUsed> LoomBeamsUsed { get; set; }

        public List<DailyOperationLoomHistory> LoomHistories { get; set; }

        public DailyOperationLoomDocument(Guid identity,
                                          OrderId orderDocumentId,
                                          int beamProcessed,
                                          string operationStatus) : base(identity)
        {
            Identity = identity;
            OrderDocumentId = orderDocumentId;
            BeamProcessed = beamProcessed;
            OperationStatus = operationStatus;
            //LoomBeamsUsed= new List<DailyOperationLoomBeamUsed>();
            //LoomProducts = new List<DailyOperationLoomProduct>();
            //LoomHistories = new List<DailyOperationLoomHistory>();

            this.MarkTransient();

            ReadModel = new DailyOperationLoomDocumentReadModel(Identity)
            {
                OrderDocumentId = OrderDocumentId.Value,
                TotalCounter = TotalCounter,
                BeamProcessed = BeamProcessed,
                OperationStatus = OperationStatus
            };

            //ReadModel.AddDomainEvent(new OnAddDailyOperationLoomDocument(Identity));
        }

        //Constructor for Mapping Object from Database to Domain
        public DailyOperationLoomDocument(DailyOperationLoomDocumentReadModel readModel) : base(readModel)
        {
            //Instantiate Object from Database
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.TotalCounter = readModel.TotalCounter;
            this.BeamProcessed = readModel.BeamProcessed;
            this.OperationStatus = readModel.OperationStatus;
        }

        //public void AddDailyOperationLoomBeamProduct(DailyOperationLoomBeamProduct beamProduct)
        //{
        //    var list = LoomBeamProducts.ToList();
        //    list.Add(beamProduct);
        //    LoomBeamProducts = list;
        //    ReadModel.LoomBeamProducts = LoomBeamProducts.ToList();

        //    MarkModified();
        //}

        //public void UpdateDailyOperationLoomBeamProduct(DailyOperationLoomBeamProduct beamProduct)
        //{
        //    var loomBeamProducts = LoomBeamProducts.ToList();

        //    //Get Loom Beam Product Update
        //    var index =
        //        loomBeamProducts
        //            .FindIndex(x => x.Identity.Equals(beamProduct.Identity));
        //    var loomBeamProduct =
        //        loomBeamProducts
        //            .Where(x => x.Identity.Equals(beamProduct.Identity))
        //            .FirstOrDefault();

        //    //Update Beam Product Properties
        //    loomBeamProduct.SetBeamDocumentId(new BeamId(beamProduct.BeamDocumentId));
        //    loomBeamProduct.SetMachineDocumentId(new MachineId(beamProduct.MachineDocumentId));
        //    loomBeamProduct.SetLatestDateTimeBeamProduct(beamProduct.LatestDateTimeBeamProduct);
        //    loomBeamProduct.SetLoomProcess(beamProduct.LoomProcess);
        //    loomBeamProduct.SetBeamProductStatus(beamProduct.BeamProductStatus);

        //    loomBeamProducts[index] = loomBeamProduct;
        //    LoomBeamProducts = loomBeamProducts;
        //    ReadModel.LoomBeamProducts= loomBeamProducts;
        //    MarkModified();
        //}

        //public void RemoveDailyOperationLoomBeamProduct(Guid identity)
        //{
        //    var beamProduct = LoomBeamProducts.Where(o => o.Identity == identity).FirstOrDefault();
        //    var list = LoomBeamProducts.ToList();

        //    list.Remove(beamProduct);
        //    LoomBeamProducts = list;
        //    ReadModel.LoomBeamProducts= LoomBeamProducts.ToList();

        //    MarkModified();
        //}

        //public void AddDailyOperationLoomHistory(DailyOperationLoomBeamHistory history)
        //{
        //    var list = LoomBeamHistories.ToList();
        //    list.Add(history);
        //    LoomBeamHistories = list;
        //    ReadModel.LoomBeamHistories = LoomBeamHistories.ToList();

        //    MarkModified();
        //}

        //public void UpdateDailyOperationLoomHistory(DailyOperationLoomBeamHistory history)
        //{
        //    var loomHistories = LoomBeamHistories.ToList();

        //    //Get Loom History Update
        //    var index =
        //        loomHistories
        //            .FindIndex(x => x.Identity.Equals(history.Identity));
        //    var loomHistory =
        //        loomHistories
        //            .Where(x => x.Identity.Equals(history.Identity))
        //            .FirstOrDefault();

        //    //Update History Properties
        //    loomHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
        //    loomHistory.SetDateTimeMachine(history.DateTimeMachine);
        //    loomHistory.SetShiftDocumentId(new ShiftId(history.ShiftDocumentId));
        //    loomHistory.SetWarpBrokenThreads(history.WarpBrokenThreads ?? 0);
        //    loomHistory.SetWeftBrokenThreads(history.WeftBrokenThreads ?? 0);
        //    loomHistory.SetLenoBrokenThreads(history.LenoBrokenThreads ?? 0);
        //    loomHistory.SetReprocessTo(history.ReprocessTo);
        //    loomHistory.SetInformation(history.Information);
        //    loomHistory.SetMachineStatus(history.MachineStatus);

        //    loomHistories[index] = loomHistory;
        //    LoomBeamHistories = loomHistories;
        //    ReadModel.LoomBeamHistories = loomHistories;
        //    MarkModified();
        //}

        //public void RemoveDailyOperationLoomHistory(Guid identity)
        //{
        //    var history = LoomBeamHistories.Where(o => o.Identity == identity).FirstOrDefault();
        //    var list = LoomBeamHistories.ToList();

        //    list.Remove(history);
        //    LoomBeamHistories = list;
        //    ReadModel.LoomBeamHistories = LoomBeamHistories.ToList();

        //    MarkModified();
        //}

        public void SetOrderDocumentId(OrderId orderDocumentId)
        {
            Validator.ThrowIfNull(() => orderDocumentId);
            if (orderDocumentId != OrderDocumentId)
            {
                OrderDocumentId = orderDocumentId;
                ReadModel.OrderDocumentId = OrderDocumentId.Value;

                MarkModified();
            }
        }

        public void SetTotalCounter(double totalCounter)
        {
            if (totalCounter != TotalCounter)
            {
                TotalCounter = totalCounter;
                ReadModel.TotalCounter = TotalCounter;

                MarkModified();
            }
        }

        public void SetBeamProcessed(int beamProcessed)
        {
            if (beamProcessed != BeamProcessed)
            {
                BeamProcessed = beamProcessed;
                ReadModel.BeamProcessed = BeamProcessed;

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

        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override DailyOperationLoomDocument GetEntity()
        {
            return this;
        }
    }
}
