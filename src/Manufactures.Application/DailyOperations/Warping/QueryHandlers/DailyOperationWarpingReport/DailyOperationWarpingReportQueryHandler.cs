using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.DailyOperationWarpingReport
{
    public class DailyOperationWarpingReportQueryHandler : IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IMaterialTypeRepository
            _materialTypeRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public DailyOperationWarpingReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _materialTypeRepository =
                _storage.GetRepository<IMaterialTypeRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
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

        public async Task<IEnumerable<DailyOperationWarpingReportListDto>> GetAll()
        {
            try
            {
                //Query for Daily Operation Warping
                var dailyOperationWarpingQuery =
                    _dailyOperationWarpingRepository
                        .Query
                        .OrderByDescending(o => o.CreatedDate);

                //Get Daily Operation Warping Data from Daily Operation Warping Repo
                await Task.Yield();
                var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(dailyOperationWarpingQuery);
                var result = new List<DailyOperationWarpingReportListDto>();

                foreach (var document in dailyOperationWarpingDocuments)
                {
                    //Get Order Production Number
                    await Task.Yield();
                    var orderDocumentId = document.OrderDocumentId.Value;
                    var orderDocumentQuery =
                        _weavingOrderDocumentRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);
                    var orderDocument =
                        _weavingOrderDocumentRepository
                            .Find(orderDocumentQuery)
                            .Where(o => o.Identity.Equals(orderDocumentId))
                            .FirstOrDefault();
                    var orderNumber = orderDocument.OrderNumber;

                    //Get Construction Number
                    await Task.Yield();
                    var fabricConstructionId = orderDocument.ConstructionId.Value;
                    var fabricConstructionQuery =
                        _fabricConstructionRepository
                            .Query
                            .OrderBy(o => o.CreatedDate);
                    var fabricConstructionDocument =
                        _fabricConstructionRepository
                            .Find(fabricConstructionQuery)
                            .Where(O => O.Identity.Equals(fabricConstructionId))
                            .FirstOrDefault();
                    var constructionNumber = fabricConstructionDocument.ConstructionNumber;

                    //Get Weaving Unit
                    await Task.Yield();
                    var weavingUnitId = orderDocument.UnitId.Value;

                    SingleUnitResult unitData = GetUnit(weavingUnitId);
                    var weavingUnitName = unitData.data.Name;

                    //Get Material Type
                    await Task.Yield();
                    var materialTypeId = document.MaterialTypeId.Value;
                    var materialTypeQuery =
                        _materialTypeRepository
                            .Query
                            .OrderBy(o => o.CreatedDate);
                    var materialTypeDocument =
                        _materialTypeRepository
                            .Find(materialTypeQuery)
                            .Where(o => o.Identity.Equals(materialTypeId))
                            .FirstOrDefault();
                    var materialTypeName = materialTypeDocument.Name;

                    //Get Amount of Cones
                    var amountOfCones = document.AmountOfCones;

                    //Get Colour of Cones
                    var colourOfCones = document.ColourOfCone;

                    //Get Latest History
                    var warpingHistories = document.WarpingHistories.OrderByDescending(o => o.CreatedDate);
                    var firstHistory = warpingHistories.Last();     //Use This History to Get History at Preparation State
                    var latestHistory = warpingHistories.First();   //Use This History to Get Latest History

                    //Get Operator Name (Latest History)
                    var operatorId = latestHistory.OperatorDocumentId;
                    var operatorQuery =
                        _operatorRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);
                    var operatorDocument =
                        _operatorRepository
                            .Find(operatorQuery)
                            .Where(o => o.Identity.Equals(operatorId))
                            .FirstOrDefault();
                    var operatorName = operatorDocument.CoreAccount.Name;

                    //Get Warping Operator Group (Latest History)
                    var warpingOperatorGroup = operatorDocument.Group;

                    //Get Preparation Date (First History (PreparationState))
                    var preparationDate = firstHistory.DateTimeMachine;

                    //Get Shift (Latest History)
                    var shiftId = latestHistory.ShiftDocumentId;
                    var shiftQuery =
                    _shiftRepository
                        .Query
                        .OrderByDescending(o => o.CreatedDate);
                    var shiftDocument =
                        _shiftRepository
                            .Find(shiftQuery)
                            .Where(o => o.Identity.Equals(shiftId))
                            .FirstOrDefault();
                    var shiftName = shiftDocument.Name;

                    //Instantiate Value to DailyOperationWarpingReportListDto
                    var dailyOperationWarpingReport = new DailyOperationWarpingReportListDto(document, 
                                                                                             orderNumber, 
                                                                                             constructionNumber, 
                                                                                             weavingUnitName, 
                                                                                             materialTypeName, 
                                                                                             operatorName, 
                                                                                             warpingOperatorGroup, 
                                                                                             preparationDate, 
                                                                                             shiftName);

                    //Add MachinePlanningDto to List of MachinePlanningDto
                    result.Add(dailyOperationWarpingReport);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetAllSpecified(Guid orderId, int weavingUnitId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByDateRangeOperationStatus(DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByMaterialType(Guid materialTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByMaterialTypeDateRange(Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByMaterialTypeDateRangeOperationStatus(Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByMaterialTypeOperationStatus(Guid materialTypeId, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOperationStatus(string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderDateRange(Guid orderId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderDateRangeOperationStatus(Guid orderId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderMaterialType(Guid orderId, Guid materialTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderMaterialTypeDateRange(Guid orderId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderMaterialTypeOperationStatus(Guid orderId, Guid materialTypeId, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderOperationStatus(Guid orderId, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderWeavingUnit(Guid orderId, int weavingUnitId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderWeavingUnitDateRange(Guid orderId, int weavingUnitId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderWeavingUnitMaterialType(Guid orderId, int weavingUnitId, Guid materialTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByOrderWeavingUnitOperationStatus(Guid orderId, int weavingUnitId, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnit(int weavingUnitId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnitDateRange(int weavingUnitId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnitMaterialType(int weavingUnitId, Guid materialTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnitMaterialTypeDateRange(int weavingUnitId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnitMaterialTypeOperationStatus(int weavingUnitId, Guid materialTypeId, string operationStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyOperationWarpingReportListDto>> GetByWeavingUnitOperationStatus(int weavingUnitId, string operationStatus)
        {
            throw new NotImplementedException();
        }
    }
}
