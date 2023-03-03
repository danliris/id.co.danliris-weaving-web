using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
            var beamType = "";
            var existingBeamCode =
               _beamRepository
                   .Find(x => x.Number.Equals(request.Number) && x.Type.Equals(request.Type))
                   .FirstOrDefault();

            if (existingBeamCode != null)
            {
                throw Validator.ErrorValidation(("Number", "No. Beam Sudah Digunakan"), ("Type", "No. Beam Dengan Tipe " + request.Type + " Sudah Digunakan"));
            }

            if (request.Type.Equals(BeamStatus.SIZING))
            {
                beamType = BeamStatus.SIZING;
            }
            else if (request.Type.Equals(BeamStatus.WARPING))
            {
                beamType = BeamStatus.WARPING;
            }

            var newBeam = new BeamDocument(Guid.NewGuid(),
                                           request.Number,
                                           beamType,
                                           request.EmptyWeight);

            await _beamRepository.Update(newBeam);
            _storage.Save();

            return newBeam;
        }
    }
}
