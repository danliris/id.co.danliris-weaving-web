using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IWarpingQuery<DailyOperationWarpingListDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository 
            _dailyOperationWarpingRepository;

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

            await Task.Yield();
            var dailyOperationWarpingDocument =
                    _dailyOperationWarpingRepository
                        .Find(query)
                        .Select(x => new DailyOperationWarpingListDto(x));

            // Not completed

            return dailyOperationWarpingDocument;
        }

        public async Task<DailyOperationWarpingListDto> GetById(Guid id)
        {
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.DailyOperationWarpingBeamProducts)
                    .Include(o => o.DailyOperationWarpingDetailHistory)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationWarpingDocument =
                   _dailyOperationWarpingRepository
                       .Find(query)
                       .Select(x => new DailyOperationWarpingByIdDto(x))
                       .FirstOrDefault();

            //Not complete for detail

            return dailyOperationWarpingDocument;
        }
    }
}
