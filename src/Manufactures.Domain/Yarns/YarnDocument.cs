using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Yarns.ReadModels;
using Manufactures.Domain.Yarns.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Yarns
{
    public class YarnDocument : AggregateRoot<YarnDocument, YarnDocumentReadModel>
    {
        public YarnDocument(Guid id,
                            string code,
                            string name,
                            string description,
                            string tags,
                            CurrencyValueObject coreCurrency,
                            UomValueObject coreUom,
                            MaterialTypeDocumentValueObject materialTypeDocument,
                            RingDocumentValueObject ringDocument,
                            double price) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => name);
            Validator.ThrowIfNullOrEmpty(() => description);
            Validator.ThrowIfNullOrEmpty(() => tags);
            Validator.ThrowIfNull(() => coreCurrency);
            Validator.ThrowIfNull(() => coreUom);
            Validator.ThrowIfNull(() => materialTypeDocument);
            Validator.ThrowIfNull(() => ringDocument);

            this.MarkTransient();

            Identity = id;
            Code = Code;
            Name = name;
            Description = description;
            Tags = tags;
            CoreCurrency = coreCurrency;
            CoreUom = coreUom;
            MaterialTypeDocument = materialTypeDocument;
            RingDocument = ringDocument;
            Price = price;

            ReadModel = new YarnDocumentReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description,
                Tags = this.Tags,
                CoreCurrency = this.CoreCurrency.Serialize(),
                CoreUom = this.CoreUom.Serialize(),
                MaterialTypeDocument = this.MaterialTypeDocument.Serialize(),
                RingDocument = this.RingDocument.Serialize(),
                Price = this.Price
            };

            ReadModel.AddDomainEvent(new OnYarnAddDocument(this.Identity));
        }

        public YarnDocument(YarnDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.Description = readModel.Description;
            this.Tags = readModel.Tags;
            this.CoreCurrency = readModel.CoreCurrency.Deserialize<CurrencyValueObject>();
            this.CoreUom = readModel.CoreUom.Deserialize<UomValueObject>();
            this.MaterialTypeDocument = readModel.MaterialTypeDocument.Deserialize<MaterialTypeDocumentValueObject>();
            this.RingDocument = readModel.RingDocument.Deserialize<RingDocumentValueObject>();
            this.Price = readModel.Price;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Tags { get; private set; }
        public CurrencyValueObject CoreCurrency { get; private set; }
        public UomValueObject CoreUom { get; private set; }
        public MaterialTypeDocumentValueObject MaterialTypeDocument { get; private set; }
        public RingDocumentValueObject RingDocument { get; private set; }
        public double Price { get; private set; }

        public void SetCode(string code)
        {
            Validator.ThrowIfNullOrEmpty(() => code);

            if (Code != code)
            {
                Code = code;
                ReadModel.Code = Code;

                MarkModified();
            }
        }

        public void SetName(string name)
        {
            Validator.ThrowIfNullOrEmpty(() => name);

            if (Name != name)
            {
                Name = name;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetDescription(string description)
        {
            Validator.ThrowIfNullOrEmpty(() => description);

            if (Description != description)
            {
                Description = description;
                ReadModel.Description = Description;

                MarkModified();
            }
        }

        public void SetTags(string tags)
        {
            Validator.ThrowIfNullOrEmpty(() => tags);

            if (Tags != tags)
            {
                Tags = tags;
                ReadModel.Tags = Tags;

                MarkModified();
            }
        }

        public void SetCurrency(CurrencyValueObject currency)
        {
            Validator.ThrowIfNull(() => currency);
            int indexDiffence = 0;


            if (CoreCurrency.Code != currency.Code)
            {
                indexDiffence++;
            }

            if (CoreCurrency.Name != currency.Name)
            {
                indexDiffence++;
            }

            if (indexDiffence > 0)
            {
                CoreCurrency = new CurrencyValueObject(currency.Code, currency.Name);
                ReadModel.CoreCurrency = CoreCurrency.Serialize();

                MarkModified();
            }
        }

        public void SetUom(UomValueObject uom)
        {
            Validator.ThrowIfNull(() => uom);
            int indexDiffence = 0;


            if (CoreUom.Code != uom.Code)
            {
                indexDiffence++;
            }

            if (CoreUom.Unit != uom.Unit)
            {
                indexDiffence++;
            }

            if (indexDiffence > 0)
            {
                CoreUom = new UomValueObject(uom.Code, uom.Unit);
                ReadModel.CoreUom = CoreUom.Serialize();

                MarkModified();
            }
        }

        public void SetMaterialTypeDocument(MaterialTypeDocumentValueObject document)
        {
            Validator.ThrowIfNull(() => document);
            int indexDiffence = 0;


            if (MaterialTypeDocument.Code != document.Code)
            {
                indexDiffence++;
            }

            if (MaterialTypeDocument.Name != document.Name)
            {
                indexDiffence++;
            }

            if (indexDiffence > 0)
            {
                MaterialTypeDocument = new MaterialTypeDocumentValueObject(document.Code, document.Name);
                ReadModel.MaterialTypeDocument = MaterialTypeDocument.Serialize();

                MarkModified();
            }
        }

        public void SetRingDocument(RingDocumentValueObject document)
        {
            Validator.ThrowIfNull(() => document);
            int indexDiffence = 0;


            if (RingDocument.Code != document.Code)
            {
                indexDiffence++;
            }

            if (RingDocument.Name != document.Name)
            {
                indexDiffence++;
            }

            if (indexDiffence > 0)
            {
                RingDocument = new RingDocumentValueObject(document.Code, document.Name);
                ReadModel.RingDocument = RingDocument.Serialize();

                MarkModified();
            }
        }

        public void SetPrice(double price)
        {
            if(Price != price)
            {
                Price = price;
                ReadModel.Price = Price;

                MarkModified();
            }
        }

        protected override YarnDocument GetEntity()
        {
            return this;
        }
    }
}
