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
    public class UpdateWarpingBrokenCauseCommandHandler : ICommandHandler<UpdateWarpingBrokenCauseCommand, WarpingBrokenCauseDocument>
    {
        private readonly IStorage _storage;
        private readonly IWarpingBrokenCauseRepository _warpingBrokenCauseRepository;

        public UpdateWarpingBrokenCauseCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingBrokenCauseRepository = _storage.GetRepository<IWarpingBrokenCauseRepository>();
        }

        public async Task<WarpingBrokenCauseDocument> Handle(UpdateWarpingBrokenCauseCommand request, CancellationToken cancellationToken)
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

            bool isOthersCategory;
            switch (request.WarpingBrokenCauseCategory)
            {
                case "Umum":
                    isOthersCategory = false;
                    break;
                case "Lain-lain":
                    isOthersCategory = true;
                    break;
                default:
                    isOthersCategory = false;
                    break;
            }

            existingWarpingBrokenCauseById.SetWarpingBrokenCauseName(request.WarpingBrokenCauseName);
            existingWarpingBrokenCauseById.SetInformation(request.Information);
            existingWarpingBrokenCauseById.SetIsOthers(isOthersCategory);

            await _warpingBrokenCauseRepository.Update(existingWarpingBrokenCauseById);
            _storage.Save();

            return existingWarpingBrokenCauseById;
        }
    }
}
