using Infrastructure.Domain;
using Manufactures.Domain.Products.ReadModels;
using System;

namespace Manufactures.Domain.Products
{
    public class ProductComposition : AggregateRoot<ProductComposition, ProductCompositionReadModel>
    {
        public ProductComposition(Guid identity) : base(identity)
        {
        }

        public ProductComposition(ProductCompositionReadModel readModel) : base(readModel)
        {
        }

        protected override ProductComposition GetEntity()
        {
            return this;
        }
    }
}