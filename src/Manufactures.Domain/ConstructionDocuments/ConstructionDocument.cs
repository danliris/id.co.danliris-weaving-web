using Infrastructure.Domain;
using Manufactures.Domain.ConstructionDocuments.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.ConstructionDocuments
{
    public class ConstructionDocument : AggregateRoot<ConstructionDocument, ConstructionDocumentReadModel>
    {
        public ConstructionDocument(Guid id, 
                                    string constructionNumber,
                                    int amountOfWarp,
                                    int amountOfWeft,
                                    int width) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => constructionNumber);

            // Set Properties
            Identity = id;
        }
        

        protected override ConstructionDocument GetEntity()
        {
            return this;
        }
    }
}
