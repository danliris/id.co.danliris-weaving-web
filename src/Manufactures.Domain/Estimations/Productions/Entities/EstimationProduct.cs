using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Estimations.Productions.Entities
{
    public class EstimationProduct : EntityBase<EstimationProduct>
    {
        public string OrderDocument { get; private set; }
        public string ProductGrade { get; private set; }
        public EstimatedProductionDocumentReadModel EstimatedProductionDocument { get; set; }
        public Guid EstimatedProductionDocumentId { get; set; }

        public EstimationProduct(Guid identity) : base(identity)
        {
        }

        public EstimationProduct(Guid identity,
                                 OrderDocumentValueObject orderDocument,
                                 ProductGrade productGrade) : base(identity)
        {
            Identity = identity;
            OrderDocument = orderDocument.Serialize();
            ProductGrade = productGrade.Serialize();
        }

        public void SetProductGrade(ProductGrade productGrade)
        {
            Validator.ThrowIfNull(() => productGrade);

            ProductGrade = productGrade.Serialize();

            MarkModified();
        }

        protected override EstimationProduct GetEntity()
        {
            return this;
        }
    }
}
