using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Rings.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.Rings
{
    public class RingDocument : AggregateRoot<RingDocument, RingDocumentReadModel>
    {
        public RingDocument(Guid identity,
                            string code,
                            string number,
                            string description) : base(identity)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => number);
            Validator.ThrowIfNullOrEmpty(() => description);

            this.MarkTransient();

            // Set properties
            Identity = identity;
            Code = code;
            Number = number;
            Description = description;

            ReadModel = new RingDocumentReadModel(Identity)
            {
                Code = this.Code,
                Number = this.Number,
                Description = this.Description
            };

            ReadModel.AddDomainEvent(new OnRingDocumentPlaced(this.Identity));
        }

        public RingDocument(RingDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Number = readModel.Number;
            this.Description = readModel.Description;
        }

        public string Code { get; private set; }
        public string Number { get; private set; }
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

        public void SetName(string number)
        {
            Validator.ThrowIfNullOrEmpty(() => number);

            if (number != Number)
            {
                Number = number;
                ReadModel.Number = Number;

                MarkModified();
            }
        }

        public void SetDescription(string description)
        {
            Validator.ThrowIfNullOrEmpty(() => description);

            if (description != Description)
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
