using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IWarpingQuery<DailyOperationWarpingListDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IBeamRepository
            _beamRepository;

        public DailyOperationWarpingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
        }

        public async Task<IEnumerable<DailyOperationWarpingListDto>> GetAll()
        {
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.DailyOperationWarpingBeamProducts)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(query);
            var result = new List<DailyOperationWarpingListDto>();

            foreach (var operation in dailyOperationWarpingDocuments)
            {
                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.ConstructionId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";
                //Get Latest BeamId
                await Task.Yield();
                var latestBeamId =
                    operation
                        .DailyOperationWarpingBeamProducts
                        .OrderByDescending(datetime => datetime.CreatedDate)
                        .FirstOrDefault()
                        .BeamId;
                //Get Beam Number
                await Task.Yield();
                var beamNumber =
                    _beamRepository
                        .Find(entity => entity.Identity.Equals(latestBeamId))
                        .FirstOrDefault()
                        .Number ?? "Not Found Beam Number";

                var operationResult = new DailyOperationWarpingListDto(operation);

                operationResult.SetConstructionNumber(constructionNumber);
                operationResult.SetLatestBeamNumber(beamNumber);

                result.Add(operationResult);
            }

            return result;
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
                       .FirstOrDefault();

            //Not complete for detail
            var result = new DailyOperationWarpingByIdDto(dailyOperationWarpingDocument);

            return result;
        }
    }
}
