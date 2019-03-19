using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.YarnNumbers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.Repositories;
using Moonlay;
using System;
using System.Linq;
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
            //Check for Existing YarnNumber with Type
            var existingYarnNumber =
                _YarnNumberRepository.Find(yarnNumber => yarnNumber.RingType == request.RingType &&
                                                         yarnNumber.Number == request.Number).FirstOrDefault();
            
            if(existingYarnNumber != null)
            {
                throw Validator.ErrorValidation(("RingType", "Type with Yarn Number has available"), 
                                                ("Number", "Number with Yarn Type has available"));
            }
            
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
