using Infrastructure.Domain;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Events;
using Moonlay;
using System;
using System.Collections.Generic;

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
                                    MaterialType materialType) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => constructionNumber);
            Validator.ThrowIfNull(() => amountOfWarp);
            Validator.ThrowIfNull(() => amountOfWeft);
            Validator.ThrowIfNull(() => width);
            Validator.ThrowIfNullOrEmpty(() => wofenType);
            Validator.ThrowIfNullOrEmpty(() => warpType);
            Validator.ThrowIfNullOrEmpty(() => weftType);
            Validator.ThrowIfNull(() => totalYarn);
            Validator.ThrowIfNull(() => materialType);

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
            Warps = new List<ConstructionDetail>().AsReadOnly();
            Wefts = new List<ConstructionDetail>().AsReadOnly();

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
                MaterialType = this.MaterialType.Value
            };

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
            this.MaterialType = new MaterialType(readModel.MaterialType);
            this.Warps = readModel.Warps;
            this.Wefts = readModel.Wefts;
        }

        public string ConstructionNumber { get; private set; }
        public int AmountOfWarp { get; private set; }
        public int AmountOfWeft { get; private set; }
        public int Width { get; private set; }
        public string WovenType { get; private set; }
        public string WarpType { get; private set; }
        public string WeftType { get; private set; }
        public double TotalYarn { get; private set; }
        public MaterialType MaterialType { get; private set; }
        public IReadOnlyCollection<ConstructionDetail> Warps { get; private set; }
        public IReadOnlyCollection<ConstructionDetail> Wefts { get; private set; }

        public void SetMaterialType(MaterialType materialType)
        {
            Validator.ThrowIfNull(() => materialType);

            if(materialType != MaterialType)
            {
                MaterialType = materialType;
                ReadModel.MaterialType = MaterialType.Value;

                MarkModified();
            }
        }

        public void SetWarps(List<ConstructionDetail> warps)
        {
            Validator.ThrowIfNull(() => warps);

            Warps = warps;
            ReadModel.Warps = (List<ConstructionDetail>)Warps;
        }

        public void SetWefts(List<ConstructionDetail> wefts)
        {
            Validator.ThrowIfNull(() => wefts);

            Wefts = wefts;
            ReadModel.Wefts = (List<ConstructionDetail>) Wefts;
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            Validator.ThrowIfNullOrEmpty(() => constructionNumber);

            if(constructionNumber != ConstructionNumber)
            {
                ConstructionNumber = constructionNumber;
                ReadModel.ConstructionNumber = ConstructionNumber;

                MarkModified();
            }
        }

        public void SetAmountOfWarp(int amountOfWarp)
        {
            Validator.ThrowIfNull(() => amountOfWarp);

            if(amountOfWarp != AmountOfWarp)
            {
                AmountOfWarp = amountOfWarp;
                ReadModel.AmountOfWarp = AmountOfWarp;

                MarkModified();
            }
        }

        public void SetAmountOfWeft(int amountOfWeft)
        {
            Validator.ThrowIfNull(() => amountOfWeft);

            if(amountOfWeft != AmountOfWeft)
            {
                AmountOfWeft = amountOfWeft;
                ReadModel.AmountOfWeft = AmountOfWeft;

                MarkModified();
            }
        }

        public void SetWidth(int width)
        {
            Validator.ThrowIfNull(() => width);
            
            if(width != Width)
            {
                Width = width;
                ReadModel.Width = Width;

                MarkModified();
            }
        }

        public void SetWovenType(string wovenType)
        {
            Validator.ThrowIfNullOrEmpty(() => wovenType);

            if(wovenType != WovenType)
            {
                WovenType = wovenType;
                ReadModel.WovenType = WovenType;

                MarkModified();
            }
        }

        public void SetWarpType(string warpType)
        {
            Validator.ThrowIfNullOrEmpty(() => warpType);

            if(warpType != WarpType)
            {
                WarpType = warpType;
                ReadModel.WarpType = WarpType;

                MarkModified();
            }
        }

        public void SetWeftType(string weftType)
        {
            Validator.ThrowIfNullOrEmpty(() => weftType);

            if(weftType != WeftType)
            {
                WeftType = weftType;
                ReadModel.WeftType = WeftType;

                MarkModified();
            }
        }

        public void SetTotalYarn(double totalYarn)
        {
            Validator.ThrowIfNull(() => totalYarn);

            if(totalYarn != TotalYarn)
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
