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
        //Property
        public string DailyOperationNumber { get; private set; }
        public ConstructionId ConstructionId { get; private set; }
        public MaterialTypeId MaterialTypeId { get; private set; }
        public int AmountOfCones { get; private set; }
        public string ColourOfCone { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public OperatorId OperatorId { get; private set; }
        public string DailyOperationStatus { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingHistory> DailyOperationWarpingDetailHistory { get; private set; }
        public IReadOnlyCollection<DailyOperationWarpingBeamProduct> DailyOperationWarpingBeamProducts { get; private set; }

        //Main Constructor
        public DailyOperationWarpingDocument(Guid id,
                                             string dailyOperationNumber,
                                             ConstructionId constructionId,
                                             MaterialTypeId materialTypeId,
                                             int amountOfCones,
                                             string colourOfCone,
                                             DateTimeOffset datetimeOperation,
                                             OperatorId operatorId) : base(id)
        {
            //Instantiate property from parameter variable
            Identity = id;
            DailyOperationNumber = dailyOperationNumber;
            ConstructionId = constructionId;
            MaterialTypeId = materialTypeId;
            AmountOfCones = amountOfCones;
            ColourOfCone = colourOfCone;
            DateTimeOperation = datetimeOperation;
            OperatorId = operatorId;
            DailyOperationWarpingDetailHistory = new List<DailyOperationWarpingHistory>();
            DailyOperationWarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>();
            DailyOperationStatus = " ";

            this.MarkTransient();

            //Save object to database as new one
            ReadModel = new DailyOperationWarpingReadModel(Identity)
            {
                DailyOperationNumber = this.DailyOperationNumber,
                ConstructionId = this.ConstructionId.Value,
                MaterialTypeId = this.MaterialTypeId.Value,
                AmountOfCones = this.AmountOfCones,
                ColourOfCone = this.ColourOfCone,
                DateTimeOperation = this.DateTimeOperation,
                OperatorId = this.OperatorId.Value
            };
        }

        //Constructor mapping object from database to domain
        public DailyOperationWarpingDocument(DailyOperationWarpingReadModel readModel) : base(readModel)
        {
            //Instantiate object from database
            this.DailyOperationNumber = ReadModel.DailyOperationNumber;
            this.ConstructionId = new ConstructionId(readModel.ConstructionId);
            this.MaterialTypeId = new MaterialTypeId(readModel.MaterialTypeId);
            this.AmountOfCones = readModel.AmountOfCones;
            this.ColourOfCone = readModel.ColourOfCone;
            this.DateTimeOperation = readModel.DateTimeOperation;
            this.OperatorId = new OperatorId(readModel.OperatorId);
            this.DailyOperationWarpingBeamProducts = readModel.DailyOperationWarpingBeamProducts;
            this.DailyOperationWarpingDetailHistory = readModel.DailyOperationWarpingDetailHistory;
        }

        public void SetDailyOperationStatus(string value)
        {
            if (!DailyOperationStatus.Equals(value))
            {
                DailyOperationStatus = value;
                ReadModel.DailyOperationStatus = DailyOperationStatus;

                MarkModified();
            }
        }

        //Add Daily Operation Warping History
        public void AddDailyOperationWarpingDetailHistory(DailyOperationWarpingHistory value)
        {
            //Modified existing list of history
            var dailyOperationWarpingHistory = DailyOperationWarpingDetailHistory.ToList();

            //Add new History
            dailyOperationWarpingHistory.Add(value);

            //Update old list
            DailyOperationWarpingDetailHistory = dailyOperationWarpingHistory;

            //Save list
            ReadModel.DailyOperationWarpingDetailHistory = dailyOperationWarpingHistory;
            MarkModified();
        }

        //Add Warping Beam Product
        public void AddDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        {
            //Modified existing list of beam product
            var dailyOperationWarpingBeamProduct = DailyOperationWarpingBeamProducts.ToList();

            //Add new History
            dailyOperationWarpingBeamProduct.Add(value);

            //Update old list
            DailyOperationWarpingBeamProducts = dailyOperationWarpingBeamProduct;

            //Save list
            ReadModel.DailyOperationWarpingBeamProducts = dailyOperationWarpingBeamProduct;
            MarkModified();
        }

        //Update existing beam product
        public void UpdateDailyOperationWarpingBeamProduct(DailyOperationWarpingBeamProduct value)
        {
            //Check if any value
            if (DailyOperationWarpingBeamProducts.Any(x => x.Identity.Equals(value.Identity)))
            {
                //Get Value by Identity & Index
                var dailyOperationWarpingBeamProducts = DailyOperationWarpingBeamProducts.ToList();
                var index = dailyOperationWarpingBeamProducts.FindIndex(x => x.Identity.Equals(value.Identity));
                var warpingBeamProduct =
                    dailyOperationWarpingBeamProducts
                        .Where(x => x.Identity.Equals(value.Identity))
                        .FirstOrDefault();
                
                //Update properties when not same
                warpingBeamProduct.SetBeamId(value.Identity);
                warpingBeamProduct.SetLength(value.Length ?? 0);
                warpingBeamProduct.SetTention(value.Tention ?? 0);
                warpingBeamProduct.SetSpeed(value.Speed ?? 0);
                warpingBeamProduct.SetPressRoll(value.PressRoll ?? 0);

                //Replace to upate warping product
                dailyOperationWarpingBeamProducts[index] = warpingBeamProduct;
                DailyOperationWarpingBeamProducts = dailyOperationWarpingBeamProducts;

                //replace old one list
                ReadModel.DailyOperationWarpingBeamProducts = dailyOperationWarpingBeamProducts;
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
