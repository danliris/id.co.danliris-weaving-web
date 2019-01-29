using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Rings.CommandHandlers
{
    public class RemoveRingDocumentCommandHandler : ICommandHandler<RemoveRingDocumentCommand, RingDocument>
    {
        private readonly IStorage _storage;
        private readonly IRingRepository _ringRepository;

        public RemoveRingDocumentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _ringRepository = _storage.GetRepository<IRingRepository>();
        }

        public async Task<RingDocument> Handle(RemoveRingDocumentCommand request, CancellationToken cancellationToken)
        {
            var ringDocument = _ringRepository.Find(entity => entity.Identity == request.Id).FirstOrDefault();

            if (ringDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Ring Id: " + request.Id));
            }

            ringDocument.Remove();

            await _ringRepository.Update(ringDocument);

            _storage.Save();

            return ringDocument;
        }
    }
}
