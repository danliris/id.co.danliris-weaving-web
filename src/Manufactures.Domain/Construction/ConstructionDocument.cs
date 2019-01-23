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
                                    string wofenType,
                                    string warpType,
                                    string weftType,
                                    int amountOfWarp,
                                    int amountOfWeft,
                                    int width,
                                    double totalYarn,
                                    MaterialType materialType, 
                                    List<ConstructionDetail> constructionDetails) : base(id)
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
            ConstructionDetails = constructionDetails;

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
                ConstructionDetails = (List<ConstructionDetail>)this.ConstructionDetails
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
            this.ConstructionDetails = readModel.ConstructionDetails;
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
        public IReadOnlyCollection<ConstructionDetail> ConstructionDetails { get; private set; }

        public void SetConstructionDetail(IReadOnlyCollection<ConstructionDetail> constructionDetails)
        {
            ConstructionDetails = constructionDetails;
            ReadModel.ConstructionDetails = (List<ConstructionDetail>)ConstructionDetails;

            MarkModified();
        }

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
