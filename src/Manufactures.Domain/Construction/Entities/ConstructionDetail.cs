using Infrastructure.Domain;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Yarns;
using Moonlay;
using System;

namespace Manufactures.Domain.Construction.Entities
{
    public class ConstructionDetail : EntityBase<ConstructionDetail>
    {
        public ConstructionDetail(Guid identity) : base(identity) { }

        public ConstructionDetail(Guid identity, 
                                  double quantity,
                                  string information,
                                  YarnDocument yarn,
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
            Yarn = yarn;
            Detail = detail;
        }

        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public YarnDocument Yarn { get; private set; }
        public string Detail { get; private set; }
        public ConstructionDocumentReadModel ConstructionDocument { get; set; }
        public Guid ConstructionDocumentId { get; set; }

        public void SetDetail (string detail)
        {
            Validator.ThrowIfNullOrEmpty(() => detail);

            if(detail != Detail)
            {
                Detail = detail;

                this.MarkModified();
            }
        }

        public void SetQuantity(double quantity)
        {
            Validator.ThrowIfNull(() => quantity);

            if(quantity != Quantity)
            {
                Quantity = quantity;

                this.MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            Validator.ThrowIfNullOrEmpty(() => information);

            if(information != Information)
            {
                Information = information;

                this.MarkModified();
            }
        }

        public void SetYarn(YarnDocument yarn)
        {
            Validator.ThrowIfNull(() => yarn);

            if(yarn.Code != Yarn.Code)
            {
                Yarn = yarn;
                
                MarkModified();
            }
        }

        protected override ConstructionDetail GetEntity()
        {
            return this;
        }
    }
}
