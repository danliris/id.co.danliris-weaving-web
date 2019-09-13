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
    public class DailyOperationWarpingDocument
        : AggregateRoot<DailyOperationWarpingDocument,
                        DailyOperationWarpingReadModel>
    {
        //Properties
        public OrderId OrderDocumentId { get; private set; }
        public MaterialTypeId MaterialTypeId { get; private set; }
        public int AmountOfCones { get; private set; }
        public string ColourOfCone { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingHistory> WarpingHistories { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingBeamProduct> WarpingBeamProducts { get; private set; }

        //Main Constructor
        public DailyOperationWarpingDocument(Guid id,
                                             OrderId orderDocumentId,
                                             MaterialTypeId materialTypeId,
                                             int amountOfCones,
                                             string colourOfCone,
                                             DateTimeOffset datetimeOperation,
                                             string operationStatus) : base(id)
        {
            //Instantiate Properties from Parameter Variable
            Identity = id;
            OrderDocumentId = orderDocumentId;
            MaterialTypeId = materialTypeId;
            AmountOfCones = amountOfCones;
            ColourOfCone = colourOfCone;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;
            WarpingHistories = new List<DailyOperationWarpingHistory>();
            WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>();

            this.MarkTransient();

            //Save Object to Database as New One
            ReadModel = new DailyOperationWarpingReadModel(Identity)
            {
                OrderDocumentId = this.OrderDocumentId.Value,
                MaterialTypeId = this.MaterialTypeId.Value,
                AmountOfCones = this.AmountOfCones,
                ColourOfCone = this.ColourOfCone,
                DateTimeOperation = this.DateTimeOperation,
                OperationStatus = this.OperationStatus
            };
        }

        //Constructor for Mapping Object from Database to Domain
        public DailyOperationWarpingDocument(DailyOperationWarpingReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            this.OrderDocumentId = new OrderId(ReadModel.OrderDocumentId);
            this.MaterialTypeId = new MaterialTypeId(readModel.MaterialTypeId);
            this.AmountOfCones = readModel.AmountOfCones;
            this.ColourOfCone = readModel.ColourOfCone;
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
                warpingHistory.SetDateTimeMachine(history.DateTimeMachine);
                warpingHistory.SetShiftId(new ShiftId(history.ShiftDocumentId));
                warpingHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
                warpingHistory.SetMachineStatus(history.MachineStatus);
                warpingHistory.SetInformation(history.Information);
                warpingHistory.SetWarpingBeamNumber(history.WarpingBeamNumber);

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
                warpingBeamProduct.SetBeamId(value.Identity);
                warpingBeamProduct.SetLength(value.Length ?? 0);
                warpingBeamProduct.SetTention(value.Tention ?? 0);
                warpingBeamProduct.SetSpeed(value.Speed ?? 0);
                warpingBeamProduct.SetPressRoll(value.PressRoll ?? 0);

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

        //Get entity
        protected override DailyOperationWarpingDocument GetEntity()
        {
            return this;
        }
    }
}
