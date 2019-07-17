using System.Collections.Generic;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IWarpingQuery<DailyOperationWarpingDocument>
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

        public Task<List<DailyOperationWarpingDocument>> Get(int page, int size, string order, string keyword, string filter)
        {
            throw new System.NotImplementedException();
        }
    }
}
