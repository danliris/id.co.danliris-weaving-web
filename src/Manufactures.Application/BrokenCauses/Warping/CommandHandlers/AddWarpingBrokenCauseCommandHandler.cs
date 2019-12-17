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
    public class AddWarpingBrokenCauseCommandHandler : ICommandHandler<AddWarpingBrokenCauseCommand, WarpingBrokenCauseDocument>
    {
        private readonly IStorage _storage;
        private readonly IWarpingBrokenCauseRepository _warpingBrokenCauseRepository;

        public AddWarpingBrokenCauseCommandHandler(IStorage storage)
        {
            _storage = storage;
            _warpingBrokenCauseRepository = _storage.GetRepository<IWarpingBrokenCauseRepository>();
        }

        public async Task<WarpingBrokenCauseDocument> Handle(AddWarpingBrokenCauseCommand request, CancellationToken cancellationToken)
        {
            var existingWarpingBrokenCauseByName =
                _warpingBrokenCauseRepository
                    .Find(o => o.WarpingBrokenCauseName.Equals(request.WarpingBrokenCauseName))
                    .FirstOrDefault();

            if (existingWarpingBrokenCauseByName != null)
            {
                throw Validator.ErrorValidation(("WarpingBrokenCauseName", "Nama Penyebab Putus Sudah Ada"));
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

            var newWarpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(),
                                                                       request.WarpingBrokenCauseName,
                                                                       request.Information,
                                                                       isOthersCategory);

            await _warpingBrokenCauseRepository.Update(newWarpingBrokenCause);
            _storage.Save();

            return newWarpingBrokenCause;
        }
    }
}
