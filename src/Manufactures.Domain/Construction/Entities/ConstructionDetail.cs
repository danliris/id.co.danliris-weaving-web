using Infrastructure.Domain;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Construction.Entities
{
    public class ConstructionDetail : EntityBase<ConstructionDetail>
    {
        public ConstructionDetail(Guid id) : base(id)
        {

        }

        public ConstructionDetail(Guid id, 
                                  double quantity,
                                  string information,
                                  Yarn yarn,
                                  bool isNew) : base(id)
        {
            // Validate Value
            Validator.ThrowIfNull(() => quantity);
            Validator.ThrowIfNullOrEmpty(() => information);
            
            if(isNew)
            {
                this.MarkTransient();
            } else
            {
                this.MarkModified();
            }
            

            // Set Value
            Quantity = quantity;
            Information = information;
            Yarn = yarn.Serialize();
        }

        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public string Yarn { get; private set; }
        public ConstructionDocumentReadModel ConstructionDocument { get; set; }
        public Guid ConstructionDocumentId { get; set; }

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

        public void SetYarn(Yarn yarn)
        {
            Validator.ThrowIfNull(() => yarn);

            if(yarn.Serialize() != Yarn)
            {
                Yarn = yarn.Serialize();

                this.MarkModified();
            }
        }

        protected override ConstructionDetail GetEntity()
        {
            return this;
        }
    }
}
