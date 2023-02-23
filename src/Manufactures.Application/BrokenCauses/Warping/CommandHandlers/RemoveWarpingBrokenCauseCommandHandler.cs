using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.Commands;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.BrokenCauses.Warping.CommandHandlers
{
    public class RemoveWarpingBrokenCauseCommandHandler : ICommandHandler<RemoveWarpingBrokenCauseCommand, WarpingBrokenCauseDocument>
    {
        private readonly IStorage _storage;
        private readonly IWarpingBrokenCauseRepository _warpingBrokenCauseRepository;

        public RemoveWarpingBrokenCauseCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingBrokenCauseRepository = _storage.GetRepository<IWarpingBrokenCauseRepository>();
        }

        public async Task<WarpingBrokenCauseDocument> Handle(RemoveWarpingBrokenCauseCommand request, CancellationToken cancellationToken)
        {
            var warpingBrokenCauseQuery =
                _warpingBrokenCauseRepository
                    .Query
                    .Where(o => o.Identity.Equals(request.Id));

            var existingWarpingBrokenCauseById =
                _warpingBrokenCauseRepository
                    .Find(warpingBrokenCauseQuery)
                    .FirstOrDefault();

            if (existingWarpingBrokenCauseById == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Warping Broken Cause with : " + request.Id));
            }

            existingWarpingBrokenCauseById.Remove();

            await _warpingBrokenCauseRepository.Update(existingWarpingBrokenCauseById);
            _storage.Save();

            return existingWarpingBrokenCauseById;
        }
    }
}
