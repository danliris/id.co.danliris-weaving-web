using Infrastructure.Domain;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Construction.Entities
{
    public class ConstructionDetail : EntityBase<ConstructionDetail>
    {
        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public string Yarn { get; private set; }
        public string Detail { get; private set; }

        public Guid ConstructionDocumentId { get; set; }
        public ConstructionDocumentReadModel ConstructionDocument { get; set; }

        public ConstructionDetail(Guid identity) : base(identity) { }

        public ConstructionDetail(Guid identity,
                                  double quantity,
                                  string information,
                                  YarnValueObject yarn,
                                  string detail) : base(identity)
        {
            // Validate Value
            Validator.ThrowIfNullOrEmpty(() => information);
            Validator.ThrowIfNullOrEmpty(() => detail);
            Validator.ThrowIfNull(() => yarn);

            this.MarkTransient();

            // Set Value
            Identity = identity;
            Quantity = quantity;
            Information = information;
            Yarn = yarn.Serialize();
            Detail = detail;
        }

        public void SetDetail(string detail)
        {
            Validator.ThrowIfNullOrEmpty(() => detail);

            if (detail != Detail)
            {
                Detail = detail;

                MarkModified();
            }
        }

        public void SetQuantity(double quantity)
        {
            if (quantity != Quantity)
            {
                Quantity = quantity;

                MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            Validator.ThrowIfNullOrEmpty(() => information);

            if (information != Information)
            {
                Information = information;

                MarkModified();
            }
        }

        public void SetYarn(YarnValueObject yarn)
        {
            Validator.ThrowIfNull(() => yarn);

            if (yarn.Code != Yarn.Deserialize<YarnValueObject>().Code)
            {
                Yarn = yarn.Serialize();

                MarkModified();
            }
        }

        protected override ConstructionDetail GetEntity()
        {
            return this;
        }
    }
}
