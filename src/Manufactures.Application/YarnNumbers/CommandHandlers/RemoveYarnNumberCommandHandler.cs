using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.YarnNumbers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.YarnNumbers.CommandHandlers
{
    public class RemoveYarnNumberCommandHandler : ICommandHandler<RemoveYarnNumberCommand, YarnNumberDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnNumberRepository _YarnNumberRepository;

        public RemoveYarnNumberCommandHandler(IStorage storage)
        {
            _storage = storage;
            _YarnNumberRepository = _storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<YarnNumberDocument> Handle(RemoveYarnNumberCommand request, CancellationToken cancellationToken)
        {
            var yarnNumberDocument = _YarnNumberRepository.Find(entity => entity.Identity == request.Id).FirstOrDefault();

            if (yarnNumberDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Ring Id: " + request.Id));
            }

            yarnNumberDocument.Remove();

            await _YarnNumberRepository.Update(yarnNumberDocument);

            _storage.Save();

            return yarnNumberDocument;
        }
    }
}
