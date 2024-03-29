﻿using Infrastructure.Domain;
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
     * Read Model : DailyOperationWarpingDocumentReadModel
     * **/
    public class DailyOperationWarpingDocument : AggregateRoot<DailyOperationWarpingDocument, DailyOperationWarpingDocumentReadModel>
    {
        //Properties
        public OrderId OrderDocumentId { get; private set; }
        public int AmountOfCones { get; private set; }
        public int BeamProductResult { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public string OperationStatus { get; private set; }
        public List<DailyOperationWarpingHistory> WarpingHistories { get; set; }
        public List<DailyOperationWarpingBeamProduct> WarpingBeamProducts { get; set; }

        //Main Constructor (Write)
        public DailyOperationWarpingDocument(Guid identity,
                                             OrderId orderDocumentId,
                                             int amountOfCones,
                                             int beamProductResult,
                                             DateTimeOffset datetimeOperation,
                                             string operationStatus) : base(identity)
        {
            //Instantiate Properties from Parameter Variable
            Identity = identity;
            OrderDocumentId = orderDocumentId;
            AmountOfCones = amountOfCones;
            BeamProductResult = beamProductResult;
            DateTimeOperation = datetimeOperation;
            OperationStatus = operationStatus;

            MarkTransient();

            //Save Object to Database as New One
            ReadModel = new DailyOperationWarpingDocumentReadModel(Identity)
            {
                OrderDocumentId = OrderDocumentId.Value,
                AmountOfCones = AmountOfCones,
                BeamProductResult = BeamProductResult,
                DateTimeOperation = DateTimeOperation,
                OperationStatus = OperationStatus
            };
        }

        //Constructor for Mapping Object from Database to Domain (Read)
        public DailyOperationWarpingDocument(DailyOperationWarpingDocumentReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            AmountOfCones = readModel.AmountOfCones;
            BeamProductResult = readModel.BeamProductResult;
            DateTimeOperation = readModel.DateTimeOperation;
            OperationStatus = readModel.OperationStatus;
        }

        ////Add Daily Operation Warping History
        //public void AddDailyOperationWarpingHistory(DailyOperationWarpingHistory history)
        //{
        //    //Modified Existing List of Detail
        //    var dailyOperationWarpingHistories = WarpingHistories.ToList();

        //    //Add New Detail
        //    dailyOperationWarpingHistories.Add(history);

        //    //Update Old List
        //    WarpingHistories = dailyOperationWarpingHistories;

        //    //Save List
        //    ReadModel.WarpingHistories = dailyOperationWarpingHistories;
        //    MarkModified();
        //}

        ////Update Existing Histories
        //public void UpdateDailyOperationWarpingHistory(DailyOperationWarpingHistory history)
        //{
        //    //Check If Any Value
        //    if (WarpingHistories.Any(x => x.Identity.Equals(history.Identity)))
        //    {
        //        //Get Value by Identity & Index
        //        var dailyOperationWarpingHistories = WarpingHistories.ToList();
        //        var index = dailyOperationWarpingHistories.FindIndex(x => x.Identity.Equals(history.Identity));
        //        var warpingHistory = dailyOperationWarpingHistories
        //                            .Where(x => x.Identity.Equals(history.Identity))
        //                            .FirstOrDefault();

        //        //Update Properties When not Same
        //        warpingHistory.SetShiftDocumentId(history.ShiftDocumentId);
        //        warpingHistory.SetOperatorDocumentId(history.OperatorDocumentId);
        //        warpingHistory.SetDateTimeMachine(history.DateTimeMachine);
        //        warpingHistory.SetMachineStatus(history.MachineStatus);
        //        warpingHistory.SetInformation(history.Information);
        //        warpingHistory.SetWarpingBeamId(history.WarpingBeamId);
        //        warpingHistory.SetWarpingBeamLengthPerOperator(history.WarpingBeamLengthPerOperator);

        //        //Replace to Update Warping Product
        //        dailyOperationWarpingHistories[index] = warpingHistory;
        //        WarpingHistories = dailyOperationWarpingHistories;

        //        //Replace Old One List
        //        ReadModel.WarpingHistories = dailyOperationWarpingHistories;
        //        MarkModified();
        //    }
        //}

        ////Add Warping Beam Product
        //public void AddDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        //{
        //    //Modified Existing List of Beam Product
        //    var dailyOperationWarpingBeamProduct = WarpingBeamProducts.ToList();

        //    //Add New Beam Product
        //    dailyOperationWarpingBeamProduct.Add(value);

        //    //Update Old List
        //    WarpingBeamProducts = dailyOperationWarpingBeamProduct;

        //    //Save List
        //    ReadModel.WarpingBeamProducts = dailyOperationWarpingBeamProduct;
        //    MarkModified();
        //}

        ////Update Existing Beam Product
        //public void UpdateDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        //{
        //    //Check If Any Value
        //    if (WarpingBeamProducts.Any(x => x.Identity.Equals(value.Identity)))
        //    {
        //        //Get Value by Identity & Index
        //        var dailyOperationWarpingBeamProducts = WarpingBeamProducts.ToList();
        //        var index = dailyOperationWarpingBeamProducts.FindIndex(x => x.Identity.Equals(value.Identity));
        //        var warpingBeamProduct =
        //            dailyOperationWarpingBeamProducts
        //                .Where(x => x.Identity.Equals(value.Identity))
        //                .FirstOrDefault();
                
        //        //Update Properties When Not Same
        //        warpingBeamProduct.SetWarpingBeamId(new BeamId(value.Identity));
        //        warpingBeamProduct.SetWarpingTotalBeamLength(value.WarpingTotalBeamLength);
        //        warpingBeamProduct.SetWarpingTotalBeamLengthUomId(value.WarpingTotalBeamLengthUomId);
        //        warpingBeamProduct.SetTention(value.Tention ?? 0);
        //        warpingBeamProduct.SetMachineSpeed(value.MachineSpeed ?? 0);
        //        warpingBeamProduct.SetPressRoll(value.PressRoll ?? 0);
        //        warpingBeamProduct.SetBeamStatus(value.BeamStatus);
        //        warpingBeamProduct.SetLatestDateTimeBeamProduct(value.LatestDateTimeBeamProduct);

        //        //Replace to Update Warping Product
        //        dailyOperationWarpingBeamProducts[index] = warpingBeamProduct;
        //        WarpingBeamProducts = dailyOperationWarpingBeamProducts;

        //        //Replace Old One List
        //        ReadModel.WarpingBeamProducts = dailyOperationWarpingBeamProducts;
        //        MarkModified();
        //    }
        //}

        public void SetOperationStatus(string operationStatus)
        {
            if (operationStatus != OperationStatus)
            {
                OperationStatus = operationStatus;
                ReadModel.OperationStatus = OperationStatus;

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

        public void SetDeleted()
        {
            MarkRemoved();
        }

        //Get entity
        protected override DailyOperationWarpingDocument GetEntity()
        {
            return this;
        }
    }
}
