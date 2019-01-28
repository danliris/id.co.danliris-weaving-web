using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Yarns.CommandHandlers
{
    public class RemoveExsistingYarnCommandHandler : ICommandHandler<RemoveExsistingYarnCommand, YarnDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;

        public RemoveExsistingYarnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
        }

        public async Task<YarnDocument> Handle(RemoveExsistingYarnCommand request, CancellationToken cancellationToken)
        {
            var exsistingYarn = _yarnDocumentRepository.Find(entity => entity.Identity == request.Id).FirstOrDefault();

            if (exsistingYarn == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Ring Id: " + request.Id));
            }

            exsistingYarn.Remove();

            await _yarnDocumentRepository.Update(exsistingYarn);

            _storage.Save();

            return exsistingYarn;
        }
    }
}
