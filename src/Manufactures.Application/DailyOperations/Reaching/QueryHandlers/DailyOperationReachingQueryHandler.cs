using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.QueryHandlers
{
    public class DailyOperationReachingQueryHandler : IReachingQuery<DailyOperationReachingListDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IShiftRepository 
            _shiftRepository;
        private readonly IOperatorRepository
            _operatorRepository;

        public DailyOperationReachingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
        }
        public async Task<IEnumerable<DailyOperationReachingListDto>> GetAll()
        {
            var query =
                _dailyOperationReachingRepository
                    .Query
                    .Include(o => o.ReachingDetails)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationReachingDocuments =
                    _dailyOperationReachingRepository
                        .Find(query);
            var result = new List<DailyOperationReachingListDto>();

            foreach (var operation in dailyOperationReachingDocuments)
            {
                var reachingDetail = operation.ReachingDetails.OrderByDescending(d => d.DateTimeMachine).FirstOrDefault();

                //Get Machine Number
                await Task.Yield();
                var machineNumber =
                    _machineRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.MachineDocumentId.Value))
                        .FirstOrDefault()
                        .MachineNumber ?? "Not Found Machine Number";

                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.ConstructionDocumentId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Sizing Beam Number
                await Task.Yield();
                var sizingBeamNumber =
                    _beamRepository
                        .Find(entity => entity.Identity.Equals(operation.SizingBeamId))
                        .FirstOrDefault()
                        .Number ?? "Not Found Sizing Beam Number";

                var operationResult = new DailyOperationReachingListDto(operation, reachingDetail, machineNumber, operation.WeavingUnitId,
                    constructionNumber, sizingBeamNumber);

                //operationResult.SetConstructionNumber(constructionNumber);
                //operationResult.SetLatestBeamNumber(beamNumber);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationReachingListDto> GetById(Guid id)
        {
            var query =
                _dailyOperationReachingRepository
                    .Query
                    .Include(o => o.ReachingDetails)
                    .OrderByDescending(x => x.CreatedDate);

            //Get Daily Operation Reaching Document
            await Task.Yield();
            var dailyOperationReachingDocument =
                   _dailyOperationReachingRepository
                       .Find(x=>x.Identity.Equals(id))
                       .FirstOrDefault();

            //Get Daily Operation Reaching Detail
            await Task.Yield();
            var dailyOperationReachingDetail = 
                dailyOperationReachingDocument
                    .ReachingDetails
                    .OrderByDescending(e=>e.DateTimeMachine)
                    .FirstOrDefault();

            //Get Machine Number
            await Task.Yield();
            var machineNumber =
                _machineRepository
                    .Find(entity => entity.Identity
                    .Equals(dailyOperationReachingDocument.MachineDocumentId.Value))
                    .FirstOrDefault()
                    .MachineNumber ?? "Not Found Machine Number";

            //Get Unit
            await Task.Yield();
            var weavingUnitId = dailyOperationReachingDocument.WeavingUnitId;

            //Get Contruction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(entity => entity.Identity
                    .Equals(dailyOperationReachingDocument.ConstructionDocumentId.Value))
                    .FirstOrDefault()
                    .ConstructionNumber ?? "Not Found Construction Number";

            //Get Sizing Beam Number
            await Task.Yield();
            var sizingBeamNumber =
                _beamRepository
                    .Find(entity => entity.Identity.Equals(dailyOperationReachingDocument.SizingBeamId))
                    .FirstOrDefault()
                    .Number ?? "Not Found Sizing Beam Number";

            //Assign Parameter to Object Result
            var result = new DailyOperationReachingByIdDto(dailyOperationReachingDocument, dailyOperationReachingDetail, machineNumber, weavingUnitId, constructionNumber, sizingBeamNumber);

            return result;
        }
    }
}
