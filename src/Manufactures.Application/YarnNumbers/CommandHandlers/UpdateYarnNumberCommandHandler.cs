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
    public class UpdateYarnNumberCommandHandler : ICommandHandler<UpdateYarnNumberCommand, YarnNumberDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnNumberRepository _YarnNumberRepository;

        public UpdateYarnNumberCommandHandler(IStorage storage)
        {
            _storage = storage;
            _YarnNumberRepository = storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<YarnNumberDocument> Handle(UpdateYarnNumberCommand request, 
                                               CancellationToken cancellationToken)
        {
            var yarnNumberDocument = 
                _YarnNumberRepository.Find(entity => entity.Identity.Equals(request.Id))
                                                              .FirstOrDefault();

            if(yarnNumberDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid ring Id: " + request.Id));
            }
            
            yarnNumberDocument.SetCode(request.Code);
            yarnNumberDocument.SetNumber(request.Number);
            yarnNumberDocument.SetRingType(request.RingType);
            yarnNumberDocument.SetDescription(request.Description);

            await _YarnNumberRepository.Update(yarnNumberDocument);

            _storage.Save();

            return yarnNumberDocument;
        }
    }
}
