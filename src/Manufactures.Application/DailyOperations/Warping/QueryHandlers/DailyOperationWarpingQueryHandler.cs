using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IDailyOperationWarpingDocumentQuery<DailyOperationWarpingListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;
        private readonly IMaterialTypeRepository
            _materialTypeRepository;
        private readonly IOrderRepository
            _weavingOrderDocumentRepository;
        private readonly IWarpingBrokenCauseRepository
            _warpingBrokenCauseRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IDailyOperationWarpingBrokenCauseRepository
            _dailyOperationWarpingBrokenCauseRepository;

        public DailyOperationWarpingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage = 
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _materialTypeRepository =
                _storage.GetRepository<IMaterialTypeRepository>();
            _warpingBrokenCauseRepository =
                _storage.GetRepository<IWarpingBrokenCauseRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _dailyOperationWarpingBrokenCauseRepository =
                _storage.GetRepository<IDailyOperationWarpingBrokenCauseRepository>();
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

        public async Task<IEnumerable<DailyOperationWarpingListDto>> GetAll()
        {
            var result = new List<DailyOperationWarpingListDto>();

            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(query);

            foreach(var document in dailyOperationWarpingDocuments)
            {
                //initiate Operation Result
                var operationResult = new DailyOperationWarpingListDto(document);

                //Get Order Document
                await Task.Yield();
                var OrderDocument =
                    _weavingOrderDocumentRepository
                        .Find(o => o.Identity == document.OrderDocumentId.Value)
                        .FirstOrDefault();

                //Get Order Number
                await Task.Yield();
                var orderNumber = OrderDocument.OrderNumber ?? "Not Found Order Number";

                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(entity => entity.Identity
                        .Equals(OrderDocument.ConstructionDocumentId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Weaving Unit
                await Task.Yield();
                SingleUnitResult unitData = GetUnit(OrderDocument.UnitId.Value);
                var weavingUnitName = unitData.data.Name;

                //Set Another Properties with Value
                operationResult.SetOrderProductionNumber(orderNumber);
                operationResult.SetConstructionNumber(constructionNumber);
                operationResult.SetWeavingUnit(weavingUnitName);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationWarpingListDto> GetById(Guid id)

        {
            var dailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(x => x.Identity == id).FirstOrDefault();

            var histories = 
                _dailyOperationWarpingHistoryRepository
                    .Find(x => x.DailyOperationWarpingDocumentId == dailyOperationWarpingDocument.Identity).ToList();

            var beamProducts = 
                _dailyOperationWarpingBeamProductRepository
                    .Find(x => x.DailyOperationWarpingDocumentId == dailyOperationWarpingDocument.Identity).ToList();

            foreach(var product in beamProducts)
            {
                var brokenCauses = 
                    _dailyOperationWarpingBrokenCauseRepository
                        .Find(x => x.DailyOperationWarpingBeamProductId == product.Identity);
                product.BrokenCauses = brokenCauses;
            }

            dailyOperationWarpingDocument.WarpingHistories = histories;
            dailyOperationWarpingDocument.WarpingBeamProducts = beamProducts;

            //Get Order Number
            await Task.Yield();
            var orderDocument =
                _weavingOrderDocumentRepository
                    .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.OrderDocumentId.Value))
                    .FirstOrDefault();

            //Get Construction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity.Equals(orderDocument.ConstructionDocumentId.Value))
                    .FirstOrDefault()
                    .ConstructionNumber;

            //Get MaterialName 
            await Task.Yield();
            var splittedConstruction = constructionNumber.Split(" ");
            var warpMaterialName = splittedConstruction[splittedConstruction.Length - 2];

            //Get Weaving Unit
            await Task.Yield();
            SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            //Get BeamProductResult
            await Task.Yield();
            var beamProductResult = dailyOperationWarpingDocument.BeamProductResult;

            //Not complete for detail
            var result = new DailyOperationWarpingByIdDto(dailyOperationWarpingDocument);
            //double totalWarpingBeamLength = 0;
            result.SetBeamProductResult(beamProductResult);
            result.SetConstructionNumber(constructionNumber);
            result.SetMaterialType(warpMaterialName);
            result.SetOrderProductionNumber(orderDocument.OrderNumber);
            result.SetWeavingUnit(weavingUnitName);

            // Add Beam Product to Data Transfer Objects
            foreach (var beamProduct in dailyOperationWarpingDocument.WarpingBeamProducts)
            {
                await Task.Yield();
                var beam =
                    _beamRepository
                        .Find(o => o.Identity.Equals(beamProduct.WarpingBeamId.Value))
                        .FirstOrDefault();

                var beamWarping = new DailyOperationWarpingBeamProductDto(beamProduct, beam);

                foreach (var brokenCauseDocument in beamProduct.BrokenCauses)
                {
                    await Task.Yield();
                    var brokenCause =
                        _warpingBrokenCauseRepository
                            .Find(o => o.Identity.Equals(brokenCauseDocument.BrokenCauseId))
                            .FirstOrDefault();

                    beamWarping.BrokenCauses.Add(new WarpingBrokenThreadsCausesDto(brokenCauseDocument, brokenCause.WarpingBrokenCauseName));
                }

                await Task.Yield();
                result.AddDailyOperationWarpingBeamProducts(beamWarping);
            }
            result.DailyOperationWarpingBeamProducts = result.DailyOperationWarpingBeamProducts.OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct).ToList();

            

            //Add History to Data Transfer Objects
            foreach (var history in dailyOperationWarpingDocument.WarpingHistories)
            {
                var warpingBeamDocument =
                    _beamRepository
                        .Find(o => o.Identity.Equals(history.WarpingBeamId.Value))
                        .FirstOrDefault();

                var warpingBeamNumber = "";

                if (warpingBeamDocument != null)
                {
                    switch (history.MachineStatus)
                    {
                        case "ENTRY":
                            warpingBeamNumber = "Belum ada Beam yang Diproses";
                            break;
                        case "FINISH":
                            warpingBeamNumber = "Operasi Selesai, Tidak ada Beam yang Diproses";
                            break;
                        default:
                            warpingBeamNumber = warpingBeamDocument.Number;
                            break;
                    }
                }
                else
                {
                    switch (history.MachineStatus)
                    {
                        case "ENTRY":
                            warpingBeamNumber = "Belum ada Beam yang Diproses";
                            break;
                        case "FINISH":
                            warpingBeamNumber = "Operasi Selesai, Tidak ada Beam yang Diproses";
                            break;
                    }
                }

                await Task.Yield();
                var shiftName =
                    _shiftRepository
                        .Find(entity => entity.Identity.Equals(history.ShiftDocumentId.Value))
                        .FirstOrDefault().Name ?? "Shift Not Found";

                await Task.Yield();
                var operatorBeam =
                    _operatorRepository
                        .Find(entity => entity.Identity == history.OperatorDocumentId.Value)
                        .FirstOrDefault();

                await Task.Yield();
                var warpingBeamLengthPerOperator = history.WarpingBeamLengthPerOperator.ToString() ?? "Belum ada Beam yang Diproses Operator";

                var dailyHistory =
                    new DailyOperationWarpingHistoryDto(history.Identity,
                                                        warpingBeamNumber,
                                                        history.DateTimeMachine,
                                                        shiftName,
                                                        operatorBeam == null ? "-" : operatorBeam.CoreAccount.Name,
                                                        operatorBeam == null ? "-" : operatorBeam.Group,
                                                        warpingBeamLengthPerOperator,
                                                        history.MachineStatus);

                await Task.Yield();
                result.AddDailyOperationWarpingHistories(dailyHistory);
            }
            result.DailyOperationWarpingHistories = result.DailyOperationWarpingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
