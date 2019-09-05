using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.ReachingTying.DataTransferObjects;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.ReachingTying.Queries;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.ReachingTying.QueryHandlers
{
    public class DailyOperationReachingTyingQueryHandler : IReachingTyingQuery<DailyOperationReachingTyingListDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingTyingRepository
            _dailyOperationReachingTyingRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWeavingOrderDocumentRepository
            _orderDocumentRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IShiftRepository
            _shiftRepository;
        private readonly IOperatorRepository
            _operatorRepository;

        public DailyOperationReachingTyingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingTyingRepository =
                _storage.GetRepository<IDailyOperationReachingTyingRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _orderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
        }
        public async Task<IEnumerable<DailyOperationReachingTyingListDto>> GetAll()
        {
            var query =
                _dailyOperationReachingTyingRepository
                    .Query
                    .Include(o => o.ReachingTyingDetails)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationReachingTyingDocuments =
                    _dailyOperationReachingTyingRepository
                        .Find(query);
            var result = new List<DailyOperationReachingTyingListDto>();

            foreach (var operation in dailyOperationReachingTyingDocuments)
            {
                var reachingDetail = operation.ReachingTyingDetails.OrderByDescending(d => d.DateTimeMachine).FirstOrDefault();

                //Get Machine Number
                await Task.Yield();
                var machineNumber =
                    _machineRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.MachineDocumentId.Value))
                        .FirstOrDefault()
                        .MachineNumber ?? "Not Found Machine Number";

                //Get Order Document
                await Task.Yield();
                var orderDocument =
                    _orderDocumentRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.OrderDocumentId.Value))
                        .FirstOrDefault();

                //Get Construction Id
                var constructionId = orderDocument.ConstructionId.Value;

                //Get Weaving Unit Id
                var weavingUnitId = orderDocument.UnitId;

                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(entity => entity.Identity
                        .Equals(constructionId))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Sizing Beam Number
                await Task.Yield();
                var sizingBeamNumber =
                    _beamRepository
                        .Find(entity => entity.Identity.Equals(operation.SizingBeamId.Value))
                        .FirstOrDefault()
                        .Number ?? "Not Found Sizing Beam Number";

                var operationResult = new DailyOperationReachingTyingListDto(operation, reachingDetail, machineNumber, weavingUnitId,
                    constructionNumber, sizingBeamNumber);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationReachingTyingListDto> GetById(Guid id)
        {
            var query =
                _dailyOperationReachingTyingRepository.Query
                    .Include(o => o.ReachingTyingDetails)
                    .OrderByDescending(x => x.CreatedDate);

            //Get Daily Operation Reaching Document
            await Task.Yield();
            var dailyOperationReachingTyingDocument =
                   _dailyOperationReachingTyingRepository
                       .Find(query)
                       .Where(x => x.Identity.Equals(id))
                       .FirstOrDefault();

            //Get Machine Number
            await Task.Yield();
            var machineNumber =
                _machineRepository
                    .Find(entity => entity.Identity
                    .Equals(dailyOperationReachingTyingDocument.MachineDocumentId.Value))
                    .FirstOrDefault()
                    .MachineNumber ?? "Not Found Machine Number";

            //Get Order Document
            await Task.Yield();
            var orderDocument =
                _orderDocumentRepository
                    .Find(entity => entity.Identity
                    .Equals(dailyOperationReachingTyingDocument.OrderDocumentId.Value))
                    .FirstOrDefault();

            //Get Construction Id
            var constructionId = orderDocument.ConstructionId.Value;

            //Get Weaving Unit Id
            var weavingUnitId = orderDocument.UnitId;

            //Get Contruction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(entity => entity.Identity
                    .Equals(constructionId))
                    .FirstOrDefault()
                    .ConstructionNumber ?? "Not Found Construction Number";

            //Get Sizing Beam Number
            await Task.Yield();
            var sizingBeamDocument =
                _beamRepository
                    .Find(entity => entity.Identity.Equals(dailyOperationReachingTyingDocument.SizingBeamId.Value))
                    .FirstOrDefault();
            var sizingBeamNumber = sizingBeamDocument.Number ?? "Not Found Sizing Beam Number";
            var sizingYarnStrands = sizingBeamDocument.YarnStrands;

            //Get Daily Operation Reaching Detail
            await Task.Yield();
            var dailyOperationReachingTyingDetail =
                dailyOperationReachingTyingDocument
                    .ReachingTyingDetails
                    .OrderByDescending(e => e.DateTimeMachine)
                    .FirstOrDefault();

            //Assign Parameter to Object Result
            var result = new DailyOperationReachingTyingByIdDto(dailyOperationReachingTyingDocument, dailyOperationReachingTyingDetail, machineNumber, weavingUnitId, constructionNumber, sizingBeamNumber, sizingYarnStrands);

            foreach (var detail in dailyOperationReachingTyingDocument.ReachingTyingDetails)
            {
                //Get Operator Name
                await Task.Yield();
                var operatorName = 
                    _operatorRepository
                        .Find(entity => entity.Identity.Equals(detail.OperatorDocumentId))
                        .FirstOrDefault()
                        .CoreAccount.Name ?? "Not Found Operator Name";

                //Get Shift Name
                await Task.Yield();
                var shiftName =
                    _shiftRepository
                        .Find(entity => entity.Identity.Equals(detail.ShiftDocumentId))
                        .FirstOrDefault()
                        .Name ?? "Not Found Shift Name";

                var reachingDetail = new DailyOperationReachingTyingDetailDto(detail.Identity, operatorName, detail.YarnStrandsProcessed, detail.DateTimeMachine, shiftName, detail.MachineStatus);

                result.ReachingHistories.Add(reachingDetail);
            }
            result.ReachingHistories = result.ReachingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
