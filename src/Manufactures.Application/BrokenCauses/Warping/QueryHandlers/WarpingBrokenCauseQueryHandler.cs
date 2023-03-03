using ExtCore.Data.Abstractions;
using Manufactures.Application.BrokenCauses.Warping.DataTransferObjects;
using Manufactures.Domain.BrokenCauses.Warping.Queries;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.BrokenCauses.Warping.QueryHandlers
{
    public class WarpingBrokenCauseQueryHandler : IWarpingBrokenCauseQuery<WarpingBrokenCauseDto>
    {
        private readonly IStorage _storage;
        private readonly IWarpingBrokenCauseRepository _warpingBrokenCauseRepository;

        public WarpingBrokenCauseQueryHandler(IStorage storage)
        {
            _storage = storage;
            _warpingBrokenCauseRepository = _storage.GetRepository<IWarpingBrokenCauseRepository>();
        }

        public async Task<IEnumerable<WarpingBrokenCauseDto>> GetAll()
        {
            var warpingBrokenCauseQuery =
                _warpingBrokenCauseRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var warpingBrokenCauses =
                _warpingBrokenCauseRepository
                    .Find(warpingBrokenCauseQuery);
            var result = new List<WarpingBrokenCauseDto>();
            foreach (var cause in warpingBrokenCauses)
            {
                var categoryValue = cause.IsOthers;
                var category = "-";

                switch (categoryValue)
                {
                    case true:
                        category = "Lain-lain";
                        break;
                    case false:
                        category = "Umum";
                        break;
                    default:
                        category = "-";
                        break;
                }

                //Instantiate Value to Dto
                await Task.Yield();
                var operationResult = new WarpingBrokenCauseDto(cause, category);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<WarpingBrokenCauseDto> GetById(Guid id)
        {
            var warpingBrokenCauseQuery =
                   _warpingBrokenCauseRepository
                       .Query
                       .Where(o => o.Identity.Equals(id))
                       .OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var warpingBrokenCauses =
                _warpingBrokenCauseRepository
                    .Find(warpingBrokenCauseQuery)
                    .FirstOrDefault();

            var categoryValue = warpingBrokenCauses.IsOthers;
            var category = "-";

            switch (categoryValue)
            {
                case true:
                    category = "Lain-lain";
                    break;
                case false:
                    category = "Umum";
                    break;
                default:
                    category = "-";
                    break;
            }

            //Instantiate Value to Dto
            await Task.Yield();
            var result = new WarpingBrokenCauseDto(warpingBrokenCauses, category);

            return result;
        }
    }
}
