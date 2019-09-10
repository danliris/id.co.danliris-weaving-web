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
        public IReadOnlyCollection<DailyOperationWarpingDetail> WarpingDetails { get; private set; }
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
            WarpingDetails = new List<DailyOperationWarpingDetail>();
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
            this.WarpingBeamProducts = readModel.WarpingBeamProducts;
            this.WarpingDetails = readModel.WarpingDetails;
        }

        //Add Daily Operation Warping Detail
        public void AddDailyOperationWarpingDetail(DailyOperationWarpingDetail detail)
        {
            //Modified Existing List of Detail
            var dailyOperationWarpingDetails = WarpingDetails.ToList();

            //Add New Detail
            dailyOperationWarpingDetails.Add(detail);

            //Update Old List
            WarpingDetails = dailyOperationWarpingDetails;

            //Save List
            ReadModel.WarpingDetails = dailyOperationWarpingDetails;
            MarkModified();
        }

        //Update Existing Detail
        public void UpdateDailyOperationWarpingDetail(DailyOperationWarpingDetail detail)
        {
            //Check If Any Value
            if (WarpingDetails.Any(x => x.Identity.Equals(detail.Identity)))
            {
                //Get Value by Identity & Index
                var dailyOperationWarpingDetails = WarpingDetails.ToList();
                var index = dailyOperationWarpingDetails.FindIndex(x => x.Identity.Equals(detail.Identity));
                var warpingDetail = dailyOperationWarpingDetails
                                    .Where(x => x.Identity.Equals(detail.Identity))
                                    .FirstOrDefault();

                //Update Properties When not Same
                warpingDetail.SetShiftId(new ShiftId(detail.ShiftDocumentId));
                warpingDetail.SetOperatorDocumentId(new OperatorId(detail.OperatorDocumentId));
                warpingDetail.SetDateTimeMachine(detail.DateTimeMachine);
                warpingDetail.SetMachineStatus(detail.MachineStatus);
                warpingDetail.SetInformation(detail.Information);
                warpingDetail.SetWarpingBeamNumber(detail.WarpingBeamNumber);

                //Replace to Update Warping Product
                dailyOperationWarpingDetails[index] = warpingDetail;
                WarpingDetails = dailyOperationWarpingDetails;

                //Replace Old One List
                ReadModel.WarpingDetails = dailyOperationWarpingDetails;
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
