using System;
using Infrastructure.Domain.ReadModels;

namespace Manufactures.Domain.ReadModels
{
    public class ProductCompositionReadModel : ReadModelBase
    {
        public ProductCompositionReadModel(Guid identity) : base(identity)
        {
        }
    }
}