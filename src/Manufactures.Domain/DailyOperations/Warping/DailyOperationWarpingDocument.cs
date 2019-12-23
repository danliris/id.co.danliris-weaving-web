using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.DailyOperations.Warping
{
    /**
     * Domain for Daily Operation Warping
     * Read Model : DailyOperationWarpingReadModel
     * **/
    public class DailyOperationWarpingDocument : AggregateRoot<DailyOperationWarpingDocument, DailyOperationWarpingReadModel>
    {
        //Properties
        public OrderId OrderDocumentId { get; private set; }
        public int AmountOfCones { get; private set; }
        public int BeamProductResult { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingHistory> WarpingHistories { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingBeamProduct> WarpingBeamProducts { get; private set; }

        //Main Constructor
        public DailyOperationWarpingDocument(Guid id,
                                             OrderId orderDocumentId,
                                             int amountOfCones,
                                             int beamProductResult,
                                             DateTimeOffset datetimeOperation,
                                             string operationStatus) : base(id)
        {
            //Instantiate Properties from Parameter Variable
            Identity = id;
            OrderDocumentId = orderDocumentId;
            AmountOfCones = amountOfCones;
            BeamProductResult = beamProductResult;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;
            WarpingHistories = new List<DailyOperationWarpingHistory>();
            WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>();

            this.MarkTransient();

            //Save Object to Database as New One
            ReadModel = new DailyOperationWarpingReadModel(Identity)
            {
                OrderDocumentId = this.OrderDocumentId.Value,
                AmountOfCones = this.AmountOfCones,
                BeamProductResult = this.BeamProductResult,
                DateTimeOperation = this.DateTimeOperation,
                OperationStatus = this.OperationStatus
            };
        }

        //Constructor for Mapping Object from Database to Domain
        public DailyOperationWarpingDocument(DailyOperationWarpingReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.AmountOfCones = readModel.AmountOfCones;
            this.BeamProductResult = readModel.BeamProductResult;
            this.DateTimeOperation = readModel.DateTimeOperation;
            this.OperationStatus = readModel.OperationStatus;
            this.WarpingHistories = readModel.WarpingHistories;
            this.WarpingBeamProducts = readModel.WarpingBeamProducts;
        }

        //Add Daily Operation Warping History
        public void AddDailyOperationWarpingHistory(DailyOperationWarpingHistory history)
        {
            //Modified Existing List of Detail
            var dailyOperationWarpingHistories = WarpingHistories.ToList();

            //Add New Detail
            dailyOperationWarpingHistories.Add(history);

            //Update Old List
            WarpingHistories = dailyOperationWarpingHistories;

            //Save List
            ReadModel.WarpingHistories = dailyOperationWarpingHistories;
            MarkModified();
        }

        //Update Existing Histories
        public void UpdateDailyOperationWarpingHistory(DailyOperationWarpingHistory history)
        {
            //Check If Any Value
            if (WarpingHistories.Any(x => x.Identity.Equals(history.Identity)))
            {
                //Get Value by Identity & Index
                var dailyOperationWarpingHistories = WarpingHistories.ToList();
                var index = dailyOperationWarpingHistories.FindIndex(x => x.Identity.Equals(history.Identity));
                var warpingHistory = dailyOperationWarpingHistories
                                    .Where(x => x.Identity.Equals(history.Identity))
                                    .FirstOrDefault();

                //Update Properties When not Same
                warpingHistory.SetShiftId(new ShiftId(history.ShiftDocumentId));
                warpingHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
                warpingHistory.SetDateTimeMachine(history.DateTimeMachine);
                warpingHistory.SetMachineStatus(history.MachineStatus);
                warpingHistory.SetInformation(history.Information);
                warpingHistory.SetWarpingBeamId(new BeamId(history.WarpingBeamId));

                //Replace to Update Warping Product
                dailyOperationWarpingHistories[index] = warpingHistory;
                WarpingHistories = dailyOperationWarpingHistories;

                //Replace Old One List
                ReadModel.WarpingHistories = dailyOperationWarpingHistories;
                MarkModified();
            }
        }

        //Add Warping Beam Product
        public void AddDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        {
            //Modified Existing List of Beam Product
            var dailyOperationWarpingBeamProduct = WarpingBeamProducts.ToList();

            //Add New Beam Product
            dailyOperationWarpingBeamProduct.Add(value);

            //Update Old List
            WarpingBeamProducts = dailyOperationWarpingBeamProduct;

            //Save List
            ReadModel.WarpingBeamProducts = dailyOperationWarpingBeamProduct;
            MarkModified();
        }

        //Update Existing Beam Product
        public void UpdateDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        {
            //Check If Any Value
            if (WarpingBeamProducts.Any(x => x.Identity.Equals(value.Identity)))
            {
                //Get Value by Identity & Index
                var dailyOperationWarpingBeamProducts = WarpingBeamProducts.ToList();
                var index = dailyOperationWarpingBeamProducts.FindIndex(x => x.Identity.Equals(value.Identity));
                var warpingBeamProduct =
                    dailyOperationWarpingBeamProducts
                        .Where(x => x.Identity.Equals(value.Identity))
                        .FirstOrDefault();
                
                //Update Properties When Not Same
                warpingBeamProduct.SetWarpingBeamId(value.Identity);
                warpingBeamProduct.SetWarpingTotalBeamLength(value.WarpingTotalBeamLength ?? 0);
                warpingBeamProduct.SetTention(value.Tention ?? 0);
                warpingBeamProduct.SetMachineSpeed(value.MachineSpeed ?? 0);
                warpingBeamProduct.SetPressRoll(value.PressRoll ?? 0);
                warpingBeamProduct.SetBeamStatus(value.BeamStatus);
                warpingBeamProduct.SetLatestDateTimeBeamProduct(value.LatestDateTimeBeamProduct);

                //Replace to Update Warping Product
                dailyOperationWarpingBeamProducts[index] = warpingBeamProduct;
                WarpingBeamProducts = dailyOperationWarpingBeamProducts;

                //Replace Old One List
                ReadModel.WarpingBeamProducts = dailyOperationWarpingBeamProducts;
                MarkModified();
            }
        }

        public void SetOperationStatus(string value)
        {
            if (!OperationStatus.Equals(value))
            {
                OperationStatus = value;
                ReadModel.OperationStatus = OperationStatus;

                MarkModified();
            }
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

        //Get entity
        protected override DailyOperationWarpingDocument GetEntity()
        {
            return this;
        }
    }
}
