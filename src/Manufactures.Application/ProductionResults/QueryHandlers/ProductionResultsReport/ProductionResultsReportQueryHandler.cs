//using System;
//using System.Collections.Generic;
//using System.Linq;
//using ExtCore.Data.Abstractions;
//using Infrastructure.External.DanLirisClient.CoreMicroservice;
//using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
//using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
//using Manufactures.Application.ProductionResults.DataTransferObjects.ProductionResultsReport;
//using Manufactures.Domain.Beams.Repositories;
//using Manufactures.Domain.DailyOperations.Sizing.Repositories;
//using Manufactures.Domain.DailyOperations.Warping.Repositories;
//using Manufactures.Domain.FabricConstructions.Repositories;
//using Manufactures.Domain.Machines.Repositories;
//using Manufactures.Domain.MachineTypes.Repositories;
//using Manufactures.Domain.Operators.Repositories;
//using Manufactures.Domain.Orders.Repositories;
//using Manufactures.Domain.ProductionResults.Queries.IProductionResultsReport;
//using Manufactures.Domain.Shifts.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;

//namespace Manufactures.Application.ProductionResults.QueryHandlers.ProductionResultsReport
//{
//    public class ProductionResultsReportQueryHandler : IProductionResultReportQuery<ProductionResultsReportListDto>
//    {
//        protected readonly IHttpClientService
//            _http;
//        private readonly IStorage
//            _storage;
//        private readonly IDailyOperationSizingRepository
//            _dailyOperationSizingRepository;
//        private readonly IMachineRepository
//            _machineRepository;
//        private readonly IMachineTypeRepository
//            _machineTypeRepository;
//        private readonly IWeavingOrderDocumentRepository
//            _orderProductionRepository;
//        private readonly IFabricConstructionRepository
//            _fabricConstructionRepository;
//        private readonly IShiftRepository
//            _shiftRepository;
//        private readonly IBeamRepository
//            _beamRepository;
//        private readonly IOperatorRepository
//            _operatorRepository;
//        private readonly IDailyOperationWarpingRepository
//            _dailyOperationWarpingRepository;

//        public ProductionResultsReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
//        {
//            _http =
//                serviceProvider.GetService<IHttpClientService>();
//            _storage = storage;
//            _dailyOperationSizingRepository =
//                _storage.GetRepository<IDailyOperationSizingRepository>();
//            _machineRepository =
//                _storage.GetRepository<IMachineRepository>();
//            _machineTypeRepository =
//                _storage.GetRepository<IMachineTypeRepository>();
//            _orderProductionRepository =
//                _storage.GetRepository<IWeavingOrderDocumentRepository>();
//            _fabricConstructionRepository =
//                _storage.GetRepository<IFabricConstructionRepository>();
//            _shiftRepository =
//                _storage.GetRepository<IShiftRepository>();
//            _beamRepository =
//                _storage.GetRepository<IBeamRepository>();
//            _operatorRepository =
//                _storage.GetRepository<IOperatorRepository>();
//            _dailyOperationWarpingRepository =
//                _storage.GetRepository<IDailyOperationWarpingRepository>();
//        }

//        protected SingleUnitResult GetUnit(int id)
//        {
//            var masterUnitUri = MasterDataSettings.Endpoint + $"master/units/{id}";
//            var unitResponse = _http.GetAsync(masterUnitUri).Result;

//            if (unitResponse.IsSuccessStatusCode)
//            {
//                SingleUnitResult unitResult = JsonConvert.DeserializeObject<SingleUnitResult>(unitResponse.Content.ReadAsStringAsync().Result);
//                return unitResult;
//            }
//            else
//            {
//                return new SingleUnitResult();
//            }
//        }

//        public Task<(IEnumerable<ProductionResultsReportListDto>, int)> GetReports(string machineId, 
//                                                                                                          string orderId, 
//                                                                                                          string shiftId, 
//                                                                                                          int weavingUnitId, 
//                                                                                                          DateTimeOffset? dateFrom, 
//                                                                                                          DateTimeOffset? dateTo, 
//                                                                                                          int page, 
//                                                                                                          int size, 
//                                                                                                          string order)
//        {
//            //try
//            //{
//            //    //Add Shell (result) for Daily Operation Sizing Report Dto
//            //    var result = new List<ProductionResultsReportListDto>();

//            //    //Query for Daily Operation Sizing
//            //    var dailyOperationSizingQuery =
//            //        _dailyOperationSizingRepository
//            //            .Query
//            //            .Include(o => o.SizingBeamProducts)
//            //            .Include(o => o.SizingHistories)
//            //            .AsQueryable();
//            //}
//            //catch (Exception)
//            //{

//            //    throw;
//            //}

//            throw new NotImplementedException();
//        }
//    }
//}
