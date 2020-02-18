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
            var reachingQuery =
                _dailyOperationReachingRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var reachingDocuments =
                    _dailyOperationReachingRepository
                        .Find(reachingQuery);

            var result = new List<DailyOperationReachingListDto>();

            foreach (var reachingDocument in reachingDocuments)
            {
                var reachingHistories = 
                    _dailyOperationReachingHistoryRepository
                        .Find(o => o.DailyOperationReachingDocumentId == reachingDocument.Identity);
                var latestReachingHistory = 
                    reachingHistories
                        .OrderByDescending(d => d.DateTimeMachine)
                        .FirstOrDefault();

                //Get Machine Number
                await Task.Yield();
                var machineNumber =
                    _machineRepository
                        .Find(entity => entity.Identity
                        .Equals(reachingDocument.MachineDocumentId.Value))
                        .FirstOrDefault()?
                        .MachineNumber ?? "Not Found Machine Number";

                //Get Order Document
                await Task.Yield();
                var orderDocument =
                    _orderDocumentRepository
                        .Find(o => o.Identity == reachingDocument.OrderDocumentId.Value)
                        .FirstOrDefault();

                //Get Weaving Unit Id
                SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
                var weavingUnitName = unitData.data.Name;

                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(o => o.Identity == orderDocument.ConstructionDocumentId.Value)
                        .FirstOrDefault()?
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Sizing Beam Number
                await Task.Yield();
                var sizingBeamNumber =
                    _beamRepository
                        .Find(o => o.Identity == reachingDocument.SizingBeamId.Value)
                        .FirstOrDefault()?
                        .Number ?? "Not Found Sizing Beam Number";

                var resultDto = new DailyOperationReachingListDto(reachingDocument, 
                                                                  latestReachingHistory, 
                                                                  machineNumber, 
                                                                  weavingUnitName,
                                                                  constructionNumber, 
                                                                  sizingBeamNumber);

                result.Add(resultDto);
            }

            return result;
        }

        public async Task<DailyOperationReachingListDto> GetById(Guid id)
        {
            //Get Daily Operation Reaching Document
            await Task.Yield();
            var reachingDocument =
                _dailyOperationReachingRepository
                    .Find(o => o.Identity == id)
                    .FirstOrDefault();

            var reachingHistories = 
                _dailyOperationReachingHistoryRepository
                    .Find(o => o.DailyOperationReachingDocumentId == reachingDocument.Identity);

            //Get Machine Number
            await Task.Yield();
            var machineNumber =
                _machineRepository
                    .Find(o => o.Identity == reachingDocument.MachineDocumentId.Value)
                    .FirstOrDefault()?
                    .MachineNumber ?? "Not Found Machine Number";

            //Get Order Document
            await Task.Yield();
            var orderDocument =
                _orderDocumentRepository
                    .Find(o => o.Identity == reachingDocument.OrderDocumentId.Value)
                    .FirstOrDefault();

            //Get Weaving Unit Id
            SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            //Get Contruction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity == orderDocument.ConstructionDocumentId.Value)
                    .FirstOrDefault()?
                    .ConstructionNumber ?? "Not Found Construction Number";

            //Get Sizing Beam Number
            await Task.Yield();
            var sizingBeamDocument =
                _beamRepository
                    .Find(o => o.Identity == reachingDocument.SizingBeamId.Value)
                    .FirstOrDefault();
            var sizingBeamNumber = sizingBeamDocument.Number ?? "Not Found Sizing Beam Number";
            var sizingYarnStrands = sizingBeamDocument.YarnStrands;

            //Get Daily Operation Reaching Detail
            await Task.Yield();
            var lastReachingHistory =
                reachingHistories
                    .OrderByDescending(e => e.DateTimeMachine)
                    .FirstOrDefault();

            //Assign Parameter to Object Result
            var result = new DailyOperationReachingByIdDto(reachingDocument, 
                                                           lastReachingHistory, 
                                                           machineNumber, 
                                                           weavingUnitName, 
                                                           constructionNumber, 
                                                           sizingBeamNumber, 
                                                           sizingYarnStrands);

            foreach (var reachingHistory in reachingHistories)
            {
                //Get Operator Name
                await Task.Yield();
                var operatorName = 
                    _operatorRepository
                        .Find(o => o.Identity == reachingHistory.OperatorDocumentId.Value)
                        .FirstOrDefault()?
                        .CoreAccount.Name ?? "Not Found Operator Name";

                //Get Shift Name
                await Task.Yield();
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity == reachingHistory.ShiftDocumentId.Value)
                        .FirstOrDefault()?
                        .Name ?? "Not Found Shift Name";

                var reachingHistoryDto = new DailyOperationReachingHistoryDto(reachingHistory.Identity, 
                                                                              operatorName, 
                                                                              reachingHistory.YarnStrandsProcessed, 
                                                                              reachingHistory.DateTimeMachine, 
                                                                              shiftName, 
                                                                              reachingHistory.MachineStatus);

                result.ReachingHistories.Add(reachingHistoryDto);
            }
            result.ReachingHistories = result.ReachingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
