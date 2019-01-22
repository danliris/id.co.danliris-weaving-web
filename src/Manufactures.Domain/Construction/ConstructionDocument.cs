using Infrastructure.Domain;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Events;
using Moonlay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manufactures.Domain.Construction
{
    public class ConstructionDocument : AggregateRoot<ConstructionDocument, ConstructionDocumentReadModel>
    {
        public ConstructionDocument(Guid id,
                                    string constructionNumber,
                                    int amountOfWarp,
                                    int amountOfWeft,
                                    int width,
                                    string wofenType,
                                    string warpType,
                                    string weftType,
                                    double totalYarn,
                                    MaterialType materialType, List<Warp> warps, List<Weft> wefts) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => constructionNumber);
            Validator.ThrowIfNullOrEmpty(() => wofenType);
            Validator.ThrowIfNullOrEmpty(() => warpType);
            Validator.ThrowIfNullOrEmpty(() => weftType);

            this.MarkTransient();

            // Set Properties
            Identity = id;
            ConstructionNumber = constructionNumber;
            AmountOfWarp = amountOfWarp;
            AmountOfWeft = amountOfWeft;
            Width = width;
            WovenType = wofenType;
            WarpType = warpType;
            WeftType = weftType;
            TotalYarn = totalYarn;
            MaterialType = materialType;
            Warps = warps;
            Wefts = wefts;

            var warpsObj = new List<WarpEntity>();
            var weftsObj = new List<WeftEntity>();

            foreach (var warp in warps)
            {
                var warpObj = new WarpEntity(warp.Id, warp.Quantity, warp.Information, warp.Yarn, true);

                warpsObj.Add(warpObj);
            }

            foreach (var weft in Wefts)
            {
                var weftObj = new WeftEntity(weft.Id, weft.Quantity, weft.Information, weft.Yarn, true);

                weftsObj.Add(weftObj);
            }

            ReadModel = new ConstructionDocumentReadModel(Identity)
            {
                ConstructionNumber = this.ConstructionNumber,
                AmountOfWarp = this.AmountOfWarp,
                AmountOfWeft = this.AmountOfWeft,
                Width = this.Width,
                WovenType = this.WovenType,
                WarpType = this.WarpType,
                WeftType = this.WeftType,
                TotalYarn = this.TotalYarn,
                Warps = warpsObj,
                Wefts = weftsObj
            };

            if (this.MaterialType != null)
            {
                ReadModel.MaterialType = MaterialType.Value;
            }

            ReadModel.AddDomainEvent(new OnConstructionPlaced(this.Identity));
        }

        public ConstructionDocument(ConstructionDocumentReadModel readModel) : base(readModel)
        {
            this.ConstructionNumber = readModel.ConstructionNumber;
            this.AmountOfWarp = readModel.AmountOfWarp;
            this.AmountOfWeft = readModel.AmountOfWeft;
            this.Width = readModel.Width;
            this.WovenType = readModel.WovenType;
            this.WarpType = readModel.WarpType;
            this.WeftType = readModel.WeftType;
            this.TotalYarn = readModel.TotalYarn;
            this.MaterialType = new MaterialType(ReadModel.MaterialType);

            var warps = new List<Warp>();
            var wefts = new List<Weft>();

            foreach(var warp in readModel.Warps)
            {
                var warpObj = new Warp(warp.Identity, warp.Quantity, warp.Information, warp.Yarn.Deserialize<Yarn>());

                warps.Add(warpObj);
            }

            foreach(var weft in readModel.Wefts)
            {
                var weftObj = new Weft(weft.Identity, weft.Quantity, weft.Information, weft.Yarn.Deserialize<Yarn>());

                wefts.Add(weftObj);
            }

            this.Warps = warps;
            this.Wefts = wefts;
        }

        public string ConstructionNumber { get; private set; }
        public int AmountOfWarp { get; private set; }
        public int AmountOfWeft { get; private set; }
        public int Width { get; private set; }
        public string WovenType { get; private set; }
        public string WarpType { get; private set; }
        public string WeftType { get; private set; }
        public double TotalYarn { get; private set; }
        [NotMapped]
        public MaterialType MaterialType { get; private set; }
        public IReadOnlyCollection<Warp> Warps { get; private set; }
        public IReadOnlyCollection<Weft> Wefts { get; private set; }

        public void SetMaterialType(MaterialType materialType)
        {
            Validator.ThrowIfNull(() => materialType);

            if (materialType != MaterialType)
            {
                MaterialType = materialType;
                ReadModel.MaterialType = MaterialType.Value;

                MarkModified();
            }
        }

        public void SetWarps(List<Warp> warps)
        {
            Validator.ThrowIfNull(() => warps);

            Warps = warps;
            var warpEntitys = new List<WarpEntity>();

            foreach (var warp in Warps)
            {
                var warpObj = new WarpEntity(warp.Id, warp.Quantity, warp.Information, warp.Yarn, false);
                warpEntitys.Add(warpObj);
            }

            ReadModel.Warps = warpEntitys;
            MarkModified();
        }

        public void SetWefts(List<Weft> wefts)
        {
            Validator.ThrowIfNull(() => wefts);

            Wefts = wefts;

            var weftEntitys = new List<WeftEntity>();

            foreach (var warp in Warps)
            {
                var warpObj = new WeftEntity(warp.Id, warp.Quantity, warp.Information, warp.Yarn, false);
                weftEntitys.Add(warpObj);
            }
            ReadModel.Wefts = weftEntitys;
            MarkModified();
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            Validator.ThrowIfNullOrEmpty(() => constructionNumber);

            if (constructionNumber != ConstructionNumber)
            {
                ConstructionNumber = constructionNumber;
                ReadModel.ConstructionNumber = ConstructionNumber;

                MarkModified();
            }
        }

        public void SetAmountOfWarp(int amountOfWarp)
        {
            Validator.ThrowIfNull(() => amountOfWarp);

            if (amountOfWarp != AmountOfWarp)
            {
                AmountOfWarp = amountOfWarp;
                ReadModel.AmountOfWarp = AmountOfWarp;

                MarkModified();
            }
        }

        public void SetAmountOfWeft(int amountOfWeft)
        {
            Validator.ThrowIfNull(() => amountOfWeft);

            if (amountOfWeft != AmountOfWeft)
            {
                AmountOfWeft = amountOfWeft;
                ReadModel.AmountOfWeft = AmountOfWeft;

                MarkModified();
            }
        }

        public void SetWidth(int width)
        {
            Validator.ThrowIfNull(() => width);

            if (width != Width)
            {
                Width = width;
                ReadModel.Width = Width;

                MarkModified();
            }
        }

        public void SetWovenType(string wovenType)
        {
            Validator.ThrowIfNullOrEmpty(() => wovenType);

            if (wovenType != WovenType)
            {
                WovenType = wovenType;
                ReadModel.WovenType = WovenType;

                MarkModified();
            }
        }

        public void SetWarpType(string warpType)
        {
            Validator.ThrowIfNullOrEmpty(() => warpType);

            if (warpType != WarpType)
            {
                WarpType = warpType;
                ReadModel.WarpType = WarpType;

                MarkModified();
            }
        }

        public void SetWeftType(string weftType)
        {
            Validator.ThrowIfNullOrEmpty(() => weftType);

            if (weftType != WeftType)
            {
                WeftType = weftType;
                ReadModel.WeftType = WeftType;

                MarkModified();
            }
        }

        public void SetTotalYarn(double totalYarn)
        {
            Validator.ThrowIfNull(() => totalYarn);

            if (totalYarn != TotalYarn)
            {
                TotalYarn = totalYarn;
                ReadModel.TotalYarn = TotalYarn;

                MarkModified();
            }
        }

        protected override ConstructionDocument GetEntity()
        {
            return this;
        }
    }
}
