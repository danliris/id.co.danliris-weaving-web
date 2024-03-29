﻿using ExtCore.Data.Abstractions;
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
                throw Validator.ErrorValidation(("Number", "Beam not available with number " + request.Number));
            }

            var existingBeamCode =
              _beamRepository
                  .Find(x => x.Number.Equals(request.Number))
                  .FirstOrDefault();

            if (existingBeamCode != null && request.Number == existingBeam.Number)
            {
                throw Validator.ErrorValidation(("Number", "Beam Number has available"));
            }

            existingBeam.SetBeamNumber(request.Number);
            existingBeam.SetBeamType(request.Type);
            existingBeam.SetEmptyWeight(request.EmptyWeight);

            await _beamRepository.Update(existingBeam);
            _storage.Save();

            return existingBeam;
        }
    }
}
