using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Rings.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Rings
{
    public class RingDocument : AggregateRoot<RingDocument, RingDocumentReadModel>
    {
        public RingDocument(Guid identity,
                            string code,
                            string name,
                            string description) : base(identity)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => name);
            Validator.ThrowIfNullOrEmpty(() => description);

            this.MarkTransient();

            // Set properties
            Identity = identity;
            Code = code;
            Name = name;
            Description = description;

            ReadModel = new RingDocumentReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description
            };

            ReadModel.AddDomainEvent(new OnRingDocumentPlaced(this.Identity));
        }

        public RingDocument(RingDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.Description = readModel.Description;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

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
            Validator.ThrowIfNullOrEmpty(() => description);

            if (description != Name)
            {
                Description = description;
                ReadModel.Description = Description;

                MarkModified();
            }
        }

        protected override RingDocument GetEntity()
        {
            return this;
        }
    }
}
