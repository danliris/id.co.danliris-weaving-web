using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.Rings;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Rings.CommandHandlers
{
    public class UpdateRingDocumentCommandHandler : ICommandHandler<UpdateRingDocumentCommand, RingDocument>
    {
        private readonly IStorage _storage;
        private readonly IRingRepository _ringRepository;

        public UpdateRingDocumentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _ringRepository = storage.GetRepository<IRingRepository>();
        }

        public async Task<RingDocument> Handle(UpdateRingDocumentCommand request, CancellationToken cancellationToken)
        {
            var ringDocument = _ringRepository.Find(entity => entity.Identity == request.Id).FirstOrDefault();

            if(ringDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));
            }

            ringDocument.SetCode(request.Code);
            ringDocument.SetName(request.Name);
            ringDocument.SetDescription(request.Description);

            await _ringRepository.Update(ringDocument);

            _storage.Save();

            return ringDocument;
        }
    }
}
