using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Rings.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.Rings
{
    public class RingDocument : AggregateRoot<RingDocument, RingDocumentReadModel>
    {
        public string Code { get; private set; }
        public int Number { get; private set; }
        public string RingType { get; private set; }
        public string Description { get; private set; }

        public RingDocument(Guid identity,
                            string code,
                            int number,
                            string ringType,
                            string description) : base(identity)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => ringType);

            this.MarkTransient();

            // Set properties
            Identity = identity;
            Code = code;
            Number = number;
            Description = description;
            RingType = ringType;

            ReadModel = new RingDocumentReadModel(Identity)
            {
                Code = this.Code,
                Number = this.Number,
                RingType = this.RingType,
                Description = this.Description
            };

            ReadModel.AddDomainEvent(new OnRingDocumentPlaced(this.Identity));
        }

        public RingDocument(RingDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Number = readModel.Number;
            this.Description = readModel.Description;
            this.RingType = readModel.RingType;
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

        public void SetNumber(int number)
        {

            if (number != Number)
            {
                Number = number;
                ReadModel.Number = Number;

                MarkModified();
            }
        }

        public void SetRingType (string value)
        {
            Validator.ThrowIfNullOrEmpty(() => value);

            if (RingType != value)
            {
                RingType = value;
                ReadModel.RingType = RingType;

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

        protected override RingDocument GetEntity()
        {
            return this;
        }
    }
}
