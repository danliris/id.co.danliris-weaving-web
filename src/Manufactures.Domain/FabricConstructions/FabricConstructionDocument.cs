using Infrastructure.Domain;
using Manufactures.Domain.FabricConstructions.ReadModels;
using System;
using System.Collections.Generic;
using Moonlay;
using Manufactures.Domain.FabricConstructions.Entity;

namespace Manufactures.Domain.FabricConstructions
{
    public class FabricConstructionDocument : AggregateRoot<FabricConstructionDocument, FabricConstructionReadModel>
    {
        public string ConstructionNumber { get; private set; }
        public string MaterialType { get; private set; }
        public string WovenType { get; private set; }
        public double AmountOfWarp { get; private set; }
        public double AmountOfWeft { get; private set; }
        public double Width { get; private set; }
        public string WarpType { get; private set; }
        public string WeftType { get; private set; }
        public double ReedSpace { get; private set; }
        //Jumlah Helai Benang
        public double YarnStrandsAmount { get; private set; }
        //Jumlah Total Benang
        public double TotalYarn { get; private set; }
        public List<ConstructionYarnDetail> ConstructionWarpsDetail { get; private set; }
        public List<ConstructionYarnDetail> ConstructionWeftsDetail { get; private set; }

        public FabricConstructionDocument(Guid identity,
                                          string constructionNumber,
                                          string materialType,
                                          string wovenType,
                                          double amountOfWarp,
                                          double amountOfWeft,
                                          double width,
                                          string warpType,
                                          string weftType,
                                          double reedSpace,
                                          double yarnStrandsAmount,
                                          double totalYarn) : base(identity)
        {
            // Set Properties
            Identity = identity;
            ConstructionNumber = constructionNumber;
            MaterialType = materialType;
            WovenType = wovenType;
            AmountOfWarp = amountOfWarp;
            AmountOfWeft = amountOfWeft;
            Width = width;
            WarpType = warpType;
            WeftType = weftType;
            ReedSpace = reedSpace;
            YarnStrandsAmount = yarnStrandsAmount;
            TotalYarn = totalYarn;

            this.MarkTransient();

            ReadModel = new FabricConstructionReadModel(Identity)
            {
                ConstructionNumber = ConstructionNumber,
                MaterialType = materialType,
                WovenType = WovenType,
                AmountOfWarp = AmountOfWarp,
                AmountOfWeft = AmountOfWeft,
                Width = Width,
                WarpType = WarpType,
                WeftType = WeftType,
                ReedSpace = ReedSpace,
                YarnStrandsAmount = YarnStrandsAmount,
                TotalYarn = TotalYarn
            };

            //ReadModel.AddDomainEvent(new OnFabricConstructionPlaced(this.Identity));
        }

        public FabricConstructionDocument(FabricConstructionReadModel readModel) : base(readModel)
        {
            this.ConstructionNumber = readModel.ConstructionNumber;
            this.MaterialType = readModel.MaterialType;
            this.WovenType = readModel.WovenType;
            this.AmountOfWarp = readModel.AmountOfWarp;
            this.AmountOfWeft = readModel.AmountOfWeft;
            this.Width = readModel.Width;
            this.WarpType = readModel.WarpType;
            this.WeftType = readModel.WeftType;
            this.ReedSpace = readModel.ReedSpace;
            this.YarnStrandsAmount = readModel.YarnStrandsAmount;
            this.TotalYarn = readModel.TotalYarn;
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            Validator.ThrowIfNull(() => constructionNumber);

            if (constructionNumber != ConstructionNumber)
            {
                ConstructionNumber = constructionNumber;
                ReadModel.ConstructionNumber = ConstructionNumber;

                MarkModified();
            }
        }

        public void SetMaterialType(string materialType)
        {
            Validator.ThrowIfNull(() => materialType);

            if (materialType != MaterialType)
            {
                MaterialType = materialType;
                ReadModel.MaterialType = MaterialType;

                MarkModified();
            }
        }

        public void SetWovenType(string wovenType)
        {
            Validator.ThrowIfNull(() => wovenType);

            if (wovenType != WovenType)
            {
                WovenType = wovenType;
                ReadModel.WovenType = WovenType;

                MarkModified();
            }
        }

        public void SetAmountOfWarp(double amountOfWarp)
        {
            if (amountOfWarp != AmountOfWarp)
            {
                AmountOfWarp = amountOfWarp;
                ReadModel.AmountOfWarp = AmountOfWarp;

                MarkModified();
            }
        }

        public void SetAmountOfWeft(double amountOfWeft)
        {
            if (amountOfWeft != AmountOfWeft)
            {
                AmountOfWeft = amountOfWeft;
                ReadModel.AmountOfWeft = AmountOfWeft;

                MarkModified();
            }
        }

        public void SetWidth(double width)
        {
            if (width != Width)
            {
                Width = width;
                ReadModel.Width = Width;

                MarkModified();
            }
        }

        public void SetWarpType(string warpType)
        {
            Validator.ThrowIfNull(() => warpType);

            if (warpType != WarpType)
            {
                WarpType = warpType;
                ReadModel.WarpType = WarpType;

                MarkModified();
            }
        }

        public void SetWeftType(string weftType)
        {
            Validator.ThrowIfNull(() => weftType);

            if (weftType != WeftType)
            {
                WeftType = weftType;
                ReadModel.WeftType = WeftType;

                MarkModified();
            }
        }

        public void SetReedSpace(double reedSpace)
        {
            if (reedSpace != ReedSpace)
            {
                ReedSpace = reedSpace;
                ReadModel.ReedSpace = ReedSpace;

                MarkModified();
            }
        }

        public void SetYarnStrandsAmount(double yarnStrandsAmount)
        {
            if (yarnStrandsAmount != YarnStrandsAmount)
            {
                YarnStrandsAmount = yarnStrandsAmount;
                ReadModel.YarnStrandsAmount = YarnStrandsAmount;

                MarkModified();
            }
        }

        public void SetTotalYarn(double totalYarn)
        {
            if (totalYarn != TotalYarn)
            {
                TotalYarn = totalYarn;
                ReadModel.TotalYarn = TotalYarn;

                MarkModified();
            }
        }

        //---------------------------------------------------------------

        //public void AddWarp(ConstructionYarnDetail value)
        //{
        //    if (!ListOfWarp.Any(o => o.YarnId == value.YarnId))
        //    {
        //        var warps = ListOfWarp.ToList();
        //        warps.Add(value);
        //        ListOfWarp = warps;
        //        ReadModel.ListOfWarp = ListOfWarp.Serialize();

        //        MarkModified();
        //    }
        //}

        //public void UpdateWarp(ConstructionYarnDetail value)
        //{
        //    foreach (var warp in ListOfWarp)
        //    {
        //        if (warp.YarnId == value.YarnId)
        //        {

        //            warp.SetQuantity(value.Quantity);
        //            warp.SetInformation(value.Information);

        //            MarkModified();
        //        }
        //    }
        //}

        //public void RemoveWarp(ConstructionYarnDetail value)
        //{
        //    var warps = ListOfWarp.ToList();
        //    warps.Remove(value);
        //    ListOfWarp = warps;
        //    ReadModel.ListOfWarp = ListOfWarp.Serialize();

        //    MarkModified();
        //}

        //public void AddWeft(ConstructionYarnDetail value)
        //{
        //    if (!ListOfWeft.Any(o => o.YarnId == value.YarnId))
        //    {
        //        var wefts = ListOfWeft.ToList();
        //        wefts.Add(value);
        //        ListOfWeft = wefts;
        //        ReadModel.ListOfWeft = ListOfWeft.Serialize();

        //        MarkModified();
        //    }
        //}

        //public void UpdateWeft(ConstructionYarnDetail value)
        //{
        //    foreach (var weft in ListOfWeft)
        //    {
        //        if (weft.YarnId == value.YarnId)
        //        {

        //            weft.SetQuantity(value.Quantity);
        //            weft.SetInformation(value.Information);

        //            MarkModified();
        //        }
        //    }
        //}

        //public void RemoveWeft(ConstructionYarnDetail value)
        //{
        //    var wefts = ListOfWeft.ToList();
        //    wefts.Remove(value);
        //    ListOfWeft = wefts;
        //    ReadModel.ListOfWeft = ListOfWeft.Serialize();

        //    MarkModified();
        //}
        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override FabricConstructionDocument GetEntity()
        {
            return this;
        }
    }
}
