using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
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
        public IReadOnlyCollection<DailyOperationLoomBeamHistory> LoomBeamHistories { get; private set; }
        public IReadOnlyCollection<DailyOperationLoomBeamProduct> LoomBeamProducts { get; private set; }

        public DailyOperationLoomDocument(Guid id, OrderId orderDocumentId, string operationStatus) : base(id)
        {
            Identity = id;
            OrderDocumentId = orderDocumentId;
            OperationStatus = operationStatus;
            LoomBeamHistories = new List<DailyOperationLoomBeamHistory>();
            LoomBeamProducts = new List<DailyOperationLoomBeamProduct>();

            this.MarkTransient();

            ReadModel = new DailyOperationLoomReadModel(Identity)
            {
                OrderDocumentId = OrderDocumentId.Value,
                OperationStatus = OperationStatus
            };
        }

        public DailyOperationLoomDocument(DailyOperationLoomReadModel readModel) : base(readModel)
        {
            OrderDocumentId = readModel.OrderDocumentId.HasValue ? new OrderId(readModel.OrderDocumentId.Value) : null;
            OperationStatus = readModel.OperationStatus;
        }

        public void AddDailyOperationLoomHistory(DailyOperationLoomBeamHistory history)
        {
            var list = LoomBeamHistories.ToList();
            list.Add(history);
            LoomBeamHistories = list;
            ReadModel.LoomBeamHistories = LoomBeamHistories.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationReachingHistory(DailyOperationLoomBeamHistory history)
        {
            var loomHistories = LoomBeamHistories.ToList();

            //Get Reaching History Update
            var index =
                loomHistories
                    .FindIndex(x => x.Identity.Equals(history.Identity));
            var loomHistory =
                loomHistories
                    .Where(x => x.Identity.Equals(history.Identity))
                    .FirstOrDefault();

            //Update History Properties
            loomHistory.SetBeamDocumentId(new BeamId(history.BeamDocumentId));
            loomHistory.SetMachineDocumentId(new MachineId(history.MachineDocumentId));
            loomHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
            loomHistory.SetDateTimeMachine(history.DateTimeMachine);
            loomHistory.SetShiftDocumentId(new ShiftId(history.ShiftDocumentId));
            loomHistory.SetInformation(history.Information);
            loomHistory.SetMachineStatus(history.MachineStatus);

            loomHistories[index] = loomHistory;
            LoomBeamHistories = loomHistories;
            ReadModel.LoomBeamHistories = loomHistories;
            MarkModified();
        }

        public void RemoveDailyOperationLoomHistory(Guid identity)
        {
            var history = LoomBeamHistories.Where(o => o.Identity == identity).FirstOrDefault();
            var list = LoomBeamHistories.ToList();

            list.Remove(history);
            LoomBeamHistories = list;
            ReadModel.LoomBeamHistories = LoomBeamHistories.ToList();

            MarkModified();
        }

        public void AddDailyOperationLoomBeamProduct(DailyOperationLoomBeamProduct loomBeamProduct)
        {
            var list = LoomBeamProducts.ToList();
            list.Add(loomBeamProduct);
            LoomBeamProducts = list;
            ReadModel.LoomBeamProducts = LoomBeamProducts.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationLoomBeamProduct(DailyOperationLoomBeamProduct beamProduct)
        {
            var loomBeamProducts = LoomBeamProducts.ToList();

            //Get Sizing Beam Update
            var index =
                loomBeamProducts
                    .FindIndex(x => x.Identity.Equals(beamProduct.Identity));
            var loomBeamProduct =
                loomBeamProducts
                    .Where(x => x.Identity.Equals(beamProduct.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            loomBeamProduct.SetGreigeLength(beamProduct.GreigeLength ?? 0);
            loomBeamProduct.SetLatestDateTimeBeamProduct(beamProduct.LatestDateTimeBeamProduct);
            loomBeamProduct.SetBeamProductStatus(beamProduct.BeamProductStatus);

            loomBeamProducts[index] = loomBeamProduct;
            LoomBeamProducts = loomBeamProducts;
            ReadModel.LoomBeamProducts = loomBeamProducts;
            MarkModified();
        }

        public void RemoveDailyOperationLoomBeamProduct(Guid identity)
        {
            var beamProduct = LoomBeamProducts.Where(o => o.Identity == identity).FirstOrDefault();
            var list = LoomBeamProducts.ToList();

            list.Remove(beamProduct);
            LoomBeamProducts = list;
            ReadModel.LoomBeamProducts = LoomBeamProducts.ToList();

            MarkModified();
        }

        public void SetOperationStatus(string operationStatus)
        {
            OperationStatus = operationStatus;
            ReadModel.OperationStatus = operationStatus;
            MarkModified();
        }

        protected override DailyOperationLoomDocument GetEntity()
        {
            return this;
        }
    }
}
