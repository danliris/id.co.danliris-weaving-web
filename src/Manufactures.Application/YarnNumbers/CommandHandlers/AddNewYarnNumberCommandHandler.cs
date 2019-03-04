using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.YarnNumbers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.YarnNumbers.CommandHandlers
{
    public class AddNewYarnNumberCommandHandler : ICommandHandler<AddNewYarnNumberCommand, YarnNumberDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnNumberRepository _YarnNumberRepository;

        public AddNewYarnNumberCommandHandler(IStorage storage)
        {
            _storage = storage;
            _YarnNumberRepository = _storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<YarnNumberDocument> Handle(AddNewYarnNumberCommand request, 
                                               CancellationToken cancellationToken)
        {
            var yarnNumberDocument = new YarnNumberDocument(identity: Guid.NewGuid(),
                                                code: request.Code,
                                                number: request.Number,
                                                ringType: request.RingType,
                                                description: request.Description);

            await _YarnNumberRepository.Update(yarnNumberDocument);

            _storage.Save();

            return yarnNumberDocument;
        }
    }
}
