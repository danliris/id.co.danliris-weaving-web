using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Beams.CommandHandlers
{
    public class UpdateBeamCommandHandler
        : ICommandHandler<UpdateBeamCommand, BeamDocument>
    {
        private readonly IStorage _storage;
        private readonly IBeamRepository _beamRepository;

        public UpdateBeamCommandHandler(IStorage storage)
        {
            _storage = storage;
            _beamRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<BeamDocument> Handle(UpdateBeamCommand request,
                                               CancellationToken cancellationToken)
        {
            var existingBeam =
               _beamRepository
                   .Find(x => x.Identity.Equals(request.Id))
                   .FirstOrDefault();

            if (existingBeam == null)
            {
                Validator
                    .ErrorValidation(("Number",
                                      "Beam not available with number " +
                                        existingBeam.Number));
            }

            var existingBeamCode =
              _beamRepository
                  .Find(x => x.Number.Equals(request.Number))
                  .FirstOrDefault();

            if (request.Number == existingBeam.Number && existingBeamCode != null)
            {
                Validator
                    .ErrorValidation(("Number",
                                      "Beam Number has available"));
            }

            existingBeam.SetBeamNumber(request.Number);
            existingBeam.SetBeamType(request.Type);

            await _beamRepository.Update(existingBeam);
            _storage.Save();

            return existingBeam;
        }
    }
}
