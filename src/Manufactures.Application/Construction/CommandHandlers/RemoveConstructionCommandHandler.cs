using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Repositories;
using Moonlay;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class RemoveConstructionCommandHandler : ICommandHandler<RemoveConstructionCommand, ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;

        public RemoveConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
        }

        public async Task<ConstructionDocument> Handle(RemoveConstructionCommand request, 
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
