using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Beams.CommandHandlers
{
    public class AddBeamCommandHandler
        : ICommandHandler<AddBeamCommand, BeamDocument>
    {
        private readonly IStorage _storage;
        private readonly IBeamRepository _beamRepository;

        public AddBeamCommandHandler(IStorage storage)
        {
            _storage = storage;
            _beamRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<BeamDocument> Handle(AddBeamCommand request,
                                               CancellationToken cancellationToken)
        {
            var existingBeamCode =
               _beamRepository
                   .Find(x => x.Number.Equals(request.Number))
                   .FirstOrDefault();

            if (existingBeamCode != null)
            {
                Validator
                    .ErrorValidation(("Number",
                                      "Beam Number has available"));
            }

            var newBeam = new BeamDocument(Guid.NewGuid(), 
                                           request.Number, 
                                           request.Type, 
                                           request.EmtpyWeight);

            await _beamRepository.Update(newBeam);
            _storage.Save();

            return newBeam;
        }
    }
}
