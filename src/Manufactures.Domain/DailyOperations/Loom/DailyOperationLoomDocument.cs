using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Loom
{
    public class DailyOperationLoomDocument : AggregateRoot<DailyOperationLoomDocument, DailyOperationLoomReadModel>
    {

        public OrderId OrderDocumentId { get; private set; }
        public string OperationStatus { get; private set; }
        //public IReadOnlyCollection<DailyOperationLoomBeamProduct> LoomBeamProducts { get; private set; }

        public DailyOperationLoomDocument(Guid id, OrderId orderDocumentId, string operationStatus) : base(id)
        {
            Identity = id;
            OrderDocumentId = orderDocumentId;
            OperationStatus = operationStatus;
            //LoomBeamProducts = new List<DailyOperationLoomBeamProduct>();
            //LoomBeamHistories = new List<DailyOperationLoomBeamHistory>();

            this.MarkTransient();

            ReadModel = new DailyOperationLoomReadModel(Identity)
            {
                OrderDocumentId = OrderDocumentId.Value,
                OperationStatus = OperationStatus,
                //LoomBeamProducts = LoomBeamProducts.ToList(),
                //LoomBeamHistories = LoomBeamHistories.ToList()
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationLoomDocument(Identity));
        }

        //Constructor for Mapping Object from Database to Domain
        public DailyOperationLoomDocument(DailyOperationLoomReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.OperationStatus = readModel.OperationStatus;
            //this.LoomBeamHistories = readModel.LoomBeamHistories;
            //this.LoomBeamProducts = readModel.LoomBeamProducts;
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

        public void SetOperationStatus(string newOperationStatus)
        {
            if(OperationStatus != newOperationStatus)
            {
                OperationStatus = newOperationStatus;
                ReadModel.OperationStatus = OperationStatus;
                MarkModified();
            }
        }

        public void SetOrderDocumentId(OrderId newOrderDocumentId)
        {
            if(OrderDocumentId != newOrderDocumentId)
            {
                OrderDocumentId = newOrderDocumentId;
                ReadModel.OrderDocumentId = OrderDocumentId.Value;
                MarkModified();
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override DailyOperationLoomDocument GetEntity()
        {
            return this;
        }
    }
}
