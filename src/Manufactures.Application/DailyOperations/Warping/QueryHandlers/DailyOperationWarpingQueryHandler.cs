using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IWarpingQuery<DailyOperationWarpingListDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository 
            _dailyOperationWarpingRepository;
        public int _page { get; private set; }

        public DailyOperationWarpingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<IEnumerable<DailyOperationWarpingListDto>> GetAll()
        {
            var query = 
                _dailyOperationWarpingRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);
            var dailyOperationWarpingDocument =
                    _dailyOperationWarpingRepository
                        .Find(query)
                        .Select(x => new DailyOperationWarpingListDto(x)).ToList();

            return await Task.FromResult(dailyOperationWarpingDocument);
        }
    }
}
