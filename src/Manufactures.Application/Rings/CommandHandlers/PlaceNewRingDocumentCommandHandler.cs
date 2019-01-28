using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Rings.CommandHandlers
{
    public class PlaceNewRingDocumentCommandHandler : ICommandHandler<CreateRingDocumentCommand, RingDocument>
    {
        private readonly IStorage _storage;
        private readonly IRingRepository _ringRepository;

        public PlaceNewRingDocumentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _ringRepository = _storage.GetRepository<IRingRepository>();
        }

        public async Task<RingDocument> Handle(CreateRingDocumentCommand request, CancellationToken cancellationToken)
        {
            var hasRingDocument = _ringRepository.Find(ring => ring.Code.Equals(request.Code)).Count() >= 1;

            // Check if has same ring code
            if(hasRingDocument)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            var ringDocument = new RingDocument(identity: Guid.NewGuid(),
                                                code: request.Code,
                                                name: request.Name,
                                                description: request.Description);

            await _ringRepository.Update(ringDocument);

            _storage.Save();

            return ringDocument;
        }
    }
}
