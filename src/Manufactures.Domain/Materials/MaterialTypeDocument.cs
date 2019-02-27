using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Domain.Materials
{
    public class MaterialTypeDocument : AggregateRoot<MaterialTypeDocument, MaterialTypeReadModel>
    {

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyCollection<RingDocumentValueObject> RingDocuments { get; private set; }

        public MaterialTypeDocument(Guid id, 
                            string code, 
                            string name, 
                            string description) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => name);

            this.MarkTransient();

            Code = code;
            Name = name;
            Description = description;
            RingDocuments = new List<RingDocumentValueObject>();

            ReadModel = new MaterialTypeReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description
            };

            ReadModel.AddDomainEvent(new OnMaterialTypePlace(this.Identity));
        }

        public MaterialTypeDocument(MaterialTypeReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.Description = readModel.Description;
            this.RingDocuments = 
                String.IsNullOrEmpty(readModel.RingDocuments) ? 
                    new List<RingDocumentValueObject>() : readModel.RingDocuments
                                                                   .Deserialize<List<RingDocumentValueObject>>();
        }

        public void SetRingNumber(RingDocumentValueObject value)
        {
            var list = RingDocuments.ToList();
            list.Add(value);
            RingDocuments = list;
            ReadModel.RingDocuments = RingDocuments.Serialize();
        }

        public void RemoveRingNumber(RingDocumentValueObject  value)
        {
            var list = RingDocuments.ToList();
            list.Remove(value);
            RingDocuments = list;
            ReadModel.RingDocuments = RingDocuments.Serialize();
        }

        public void SetCode(string code)
        {
            Validator.ThrowIfNullOrEmpty(() => code);

            if (code != Code)
            {
                Code = code;
                ReadModel.Code = Code;

                MarkModified();
            }
        }

        public void SetName(string name)
        {
            Validator.ThrowIfNullOrEmpty(() => name);

            if (name != Name)
            {
                Name = name;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetDescription(string description)
        {
            if (description != Description)
            {
                Description = description;
                ReadModel.Description = Description;

                MarkModified();
            }
        }

        protected override MaterialTypeDocument GetEntity()
        {
            return this;
        }
    }
}
