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
    public class UpdateYarnNumberCommandHandler
        : ICommandHandler<UpdateYarnNumberCommand, YarnNumberDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnNumberRepository
            _YarnNumberRepository;

        public UpdateYarnNumberCommandHandler(IStorage storage)
        {
            _storage = storage;
            _YarnNumberRepository = storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<YarnNumberDocument> Handle(UpdateYarnNumberCommand request,
                                                     CancellationToken cancellationToken)
        {
            var yarnNumberDocument =
                _YarnNumberRepository
                    .Find(entity => entity.Identity.Equals(request.Id))
                    .FirstOrDefault();

            if (yarnNumberDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Yarn Number Id: " + request.Id));
            }

            if (yarnNumberDocument.RingType != request.RingType)
            {
                var existingYarnNumbers =
                    _YarnNumberRepository
                        .Find(entity => entity.RingType.Equals(request.RingType))
                        .ToList();

                foreach (var yarnNumber in existingYarnNumbers)
                {
                    if (yarnNumber.RingType == request.RingType && 
                        yarnNumber.Number == request.Number)
                    {
                        throw Validator.ErrorValidation(("RingType", "Type with Yarn Number has Available"));
                    }
                }
            }

            if (yarnNumberDocument.Number != request.Number)
            {
                var existingYarnNumbers = 
                    _YarnNumberRepository
                        .Find(entity => entity.Number.Equals(request.Number))
                        .ToList();

                foreach (var yarnNumber in existingYarnNumbers)
                {
                    if (yarnNumber.RingType == request.RingType && 
                        yarnNumber.Number == request.Number)
                    {
                        throw Validator.ErrorValidation(("Number", "Number with Yarn Type has Available"));
                    }
                }
            }

            yarnNumberDocument.SetCode(request.Code);
            yarnNumberDocument.SetNumber(request.Number);
            yarnNumberDocument.SetRingType(request.RingType);
            yarnNumberDocument.SetDescription(request.Description);

            if (request.AdditionalNumber != 0)
            {
                yarnNumberDocument.AddAditionalNumber(request.AdditionalNumber);
            }

            await _YarnNumberRepository.Update(yarnNumberDocument);

            _storage.Save();

            return yarnNumberDocument;
        }
    }
}
