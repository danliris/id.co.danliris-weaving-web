using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.QueryHandlers
{
    public class DailyOperationReachingQueryHandler : IDailyOperationReachingQuery<DailyOperationReachingListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IOrderRepository
            _orderDocumentRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IShiftRepository
            _shiftRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IDailyOperationReachingHistoryRepository
           _dailyOperationReachingHistoryRepository;

        public DailyOperationReachingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage = storage;
            _dailyOperationReachingRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _orderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }

        protected SingleUnitResult GetUnit(int id)
        {
            var masterUnitUri = MasterDataSettings.Endpoint + $"master/units/{id}";
            var unitResponse = _http.GetAsync(masterUnitUri).Result;

            if (unitResponse.IsSuccessStatusCode)
            {
                SingleUnitResult unitResult = JsonConvert.DeserializeObject<SingleUnitResult>(unitResponse.Content.ReadAsStringAsync().Result);
                return unitResult;
            }
            else
            {
                return new SingleUnitResult();
            }
        }

        public async Task<IEnumerable<DailyOperationReachingListDto>> GetAll()
        {
            var query =
                _dailyOperationReachingRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationReachingDocuments =
                    _dailyOperationReachingRepository
                        .Find(query);
            var result = new List<DailyOperationReachingListDto>();

            foreach (var operation in dailyOperationReachingDocuments)
            {
                var histories = _dailyOperationReachingHistoryRepository.Find(s => s.DailyOperationReachingDocumentId == operation.Identity);
                var reachingDetail = histories.OrderByDescending(d => d.DateTimeMachine).FirstOrDefault();

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
                var constructionId = orderDocument.ConstructionDocumentId.Value;

                //Get Weaving Unit Id
                SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
                var weavingUnitName = unitData.data.Name;

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

                var operationResult = new DailyOperationReachingListDto(operation, reachingDetail, machineNumber, weavingUnitName,
                    constructionNumber, sizingBeamNumber);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationReachingListDto> GetById(Guid id)
        {
            //var query =
            //    _dailyOperationReachingRepository.Query
            //        .OrderByDescending(x => x.CreatedDate);

            //Get Daily Operation Reaching Document
            await Task.Yield();
            var dailyOperationReachingTyingDocument =
                   _dailyOperationReachingRepository
                       .Find(x => x.Identity == id)
                       .FirstOrDefault();

            var histories = _dailyOperationReachingHistoryRepository.Find(s => s.DailyOperationReachingDocumentId == dailyOperationReachingTyingDocument.Identity);


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
            var constructionId = orderDocument.ConstructionDocumentId.Value;

            //Get Weaving Unit Id
            SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

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
                histories
                    .OrderByDescending(e => e.DateTimeMachine)
                    .FirstOrDefault();

            //Assign Parameter to Object Result
            var result = new DailyOperationReachingByIdDto(dailyOperationReachingTyingDocument, dailyOperationReachingTyingDetail, machineNumber, weavingUnitName, constructionNumber, sizingBeamNumber, sizingYarnStrands);

            foreach (var detail in histories)
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

                var reachingDetail = new DailyOperationReachingHistoryDto(detail.Identity, operatorName, detail.YarnStrandsProcessed, detail.DateTimeMachine, shiftName, detail.MachineStatus);

                result.ReachingHistories.Add(reachingDetail);
            }
            result.ReachingHistories = result.ReachingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
