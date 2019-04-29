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
    public class RemoveBeamCommandHandler : ICommandHandler<RemoveBeamCommand, BeamDocument>
    {
        private readonly IStorage _storage;
        private readonly IBeamRepository _beamRepository;

        public RemoveBeamCommandHandler(IStorage storage)
        {
            _storage = storage;
            _beamRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<BeamDocument> Handle(RemoveBeamCommand request, CancellationToken cancellationToken)
        {
            var existingBeam = 
                _beamRepository
                    .Query
                    .Where(x => x.Identity.Equals(request.Id))
                    .Select(readModel => new BeamDocument(readModel))
                    .FirstOrDefault();

            if (existingBeam == null)
            {
                Validator.ErrorValidation(("BeamNumber", "Beam not available with number " + existingBeam.BeamNumber  ));
            }

            existingBeam.Remove();

            await _beamRepository.Update(existingBeam);

            _storage.Save();

            return existingBeam;
        }
    }
}
