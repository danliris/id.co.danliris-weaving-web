using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Moonlay;

namespace Manufactures.Application.FabricConstructions.CommandHandlers
{
    public class RemoveConstructionCommandHandler : ICommandHandler<RemoveFabricConstructionCommand, FabricConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;

        public RemoveConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IFabricConstructionRepository>();
        }

        public async Task<FabricConstructionDocument> Handle(RemoveFabricConstructionCommand request, 
                                                       CancellationToken cancellationToken)
        {
            var constructionDocument = _constructionDocumentRepository.Find(entity => entity.Identity == request.Id)
                                                                      .FirstOrDefault();

            if(constructionDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Construction Document: " + request.Id));
            }

            constructionDocument.Remove();
            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
