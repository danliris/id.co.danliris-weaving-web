using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.repositories;
using System;
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
            // Check if has same ring code
            if(await _ringRepository.isAvailableRingCode(request.Code))
            {
                throw new ArgumentException("Code with " + request.Code + " has define before, check for availble code");
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
