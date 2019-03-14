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
        public double TotalGramEstimation { get; private set; }
        public string ProductGrade { get; private set; }

        public Guid EstimatedProductionDocumentId { get; set; }
        public EstimatedProductionDocumentReadModel EstimatedProductionDocument { get; set; }
        
        public EstimationProduct(Guid identity) : base(identity)
        {
        }

        public EstimationProduct(Guid identity,
                                 OrderDocumentValueObject orderDocument,
                                 ProductGrade productGrade, 
                                 double totalGramEstimation) : base(identity)
        {
            Identity = identity;
            OrderDocument = orderDocument.Serialize();
            ProductGrade = productGrade.Serialize();
            TotalGramEstimation = totalGramEstimation;
        }

        public void SetProductGrade(ProductGrade productGrade)
        {
            Validator.ThrowIfNull(() => productGrade);

            ProductGrade = productGrade.Serialize();

            MarkModified();
        }
        public void SetTotalGramEstimation(double value)
        {
            if (TotalGramEstimation != value)
            {
                TotalGramEstimation = value;

                MarkModified();
            }
        }

        protected override EstimationProduct GetEntity()
        {
            return this;
        }
    }
}
