using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Queries;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers
{
    public class DailyOperationSizingQueryHandler : IDailyOperationSizingDocumentQuery<DailyOperationSizingListDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingRepository;
        private readonly IDailyOperationSizingBeamsWarpingRepository
            _dailyOperationBeamsWarpingRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;
        private readonly IOrderRepository
            _orderProductionRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IShiftRepository
            _shiftRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public DailyOperationSizingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationBeamsWarpingRepository =
                _storage.GetRepository<IDailyOperationSizingBeamsWarpingRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
            _orderProductionRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
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

        public async Task<IEnumerable<DailyOperationSizingListDto>> GetAll()
        {
            var sizingDocumentQuery =
                _dailyOperationSizingRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var sizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(sizingDocumentQuery);

            var result = new List<DailyOperationSizingListDto>();

            foreach (var operation in sizingDocuments)
            {
                //initiate Operation Result
                var operationResult = new DailyOperationSizingListDto(operation);

                //Get Machine Document to Get Machine Number
                await Task.Yield();
                var machineDocument =
                    _machineRepository
                        .Find(o => o.Identity.Equals(operation.MachineDocumentId.Value))
                        .FirstOrDefault();

                //Get Machine Number
                var machineNumber = machineDocument.MachineNumber ?? "Not Found Machine Number";

                //Get Order Document to Get Order Number
                await Task.Yield();
                var OrderDocument =
                    _orderProductionRepository
                        .Find(o => o.Identity.Equals(operation.OrderDocumentId.Value))
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
                operationResult.SetMachineNumber(machineNumber);
                operationResult.SetOrderProductionNumber(orderNumber);
                operationResult.SetFabricConstructionNumber(constructionNumber);
                operationResult.SetWeavingUnit(weavingUnitName);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationSizingListDto> GetById(Guid id)
        {
            //Get Daily Operation Sizing
            await Task.Yield();
            var dailyOperationSizingDocument =
                   _dailyOperationSizingRepository
                       .Find(o => o.Identity == id)
                       .FirstOrDefault();

            //Get Order Production Number
            await Task.Yield();
            var orderProductionDocument =
                _orderProductionRepository
                    .Find(o => o.Identity == dailyOperationSizingDocument.OrderDocumentId.Value)
                    .FirstOrDefault();
            var orderProductionNumber = orderProductionDocument.OrderNumber;

            //Get Construction Number
            await Task.Yield();
            var fabricConstructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity == orderProductionDocument.ConstructionDocumentId.Value)
                    .FirstOrDefault()
                    .ConstructionNumber;

            //Get Machine Number
            await Task.Yield();
            var machineDocument =
                _machineRepository
                    .Find(o => o.Identity == dailyOperationSizingDocument.MachineDocumentId.Value)
                    .FirstOrDefault();
            var machineNumber = machineDocument.MachineNumber;

            //Get Machine Type
            await Task.Yield();
            var machineTypeName =
                _machineTypeRepository
                    .Find(o => o.Identity == machineDocument.MachineTypeId.Value)
                    .FirstOrDefault()
                    .TypeName;

            //Get Weaving Unit
            await Task.Yield();
            SingleUnitResult unitData = GetUnit(orderProductionDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name;

            //Not complete for detail
            var result = new DailyOperationSizingByIdDto(dailyOperationSizingDocument);
            //double totalWarpingBeamLength = 0;
            result.SetOrderProductionNumber(orderProductionNumber);
            result.SetFabricConstructionNumber(fabricConstructionNumber);
            result.SetWeavingUnit(weavingUnitName);
            result.SetMachineNumber(machineNumber);

            result.SetMachineType(machineTypeName);
            result.SetEmptyWeight(dailyOperationSizingDocument.EmptyWeight);
            result.SetYarnStrands(dailyOperationSizingDocument.YarnStrands);
            result.SetNeReal(dailyOperationSizingDocument.NeReal);
                    
            //Add Beams Warping Used in Sizing Operation to Data Transfer Object
            //Get Beam Product of Warping That Used Same Order With Current Sizing Operation
            await Task.Yield();
            var warpingDocument =
                _dailyOperationWarpingRepository
                        .Find(x => x.OrderDocumentId == dailyOperationSizingDocument.OrderDocumentId.Value);

            //Get ALL BEAM PRODUCT OF WARPING That Used Same Order With Current Sizing Operation And Add to Warping Beam Data Transfer Object
            List<DailyOperationWarpingBeamDto> warpingListBeamProducts = new List<DailyOperationWarpingBeamDto>();
            foreach (var warping in warpingDocument)
            {
                foreach (var warpingBeamProduct in warping.WarpingBeamProducts)
                {
                    await Task.Yield();
                    var warpingBeamStatus = warpingBeamProduct.BeamStatus;
                    if (warpingBeamStatus.Equals(BeamStatus.ROLLEDUP))
                    {
                        await Task.Yield();
                        var warpingBeamYarnStrands = warping.AmountOfCones;
                        var warpingBeam = new DailyOperationWarpingBeamDto(warpingBeamProduct.WarpingBeamId.Value, warpingBeamYarnStrands);
                        warpingListBeamProducts.Add(warpingBeam);
                    }
                }
            }

            var sizingBeamsWarping =
                _dailyOperationBeamsWarpingRepository
                    .Find(o => o.DailyOperationSizingDocumentId == dailyOperationSizingDocument.Identity)
                    .OrderByDescending(x => x.AuditTrail.CreatedDate);
            //Get ONLY BEAM PRODUCT OF WARPING Used in The Current Sizing Operation And Add to Warping Beam Warping Used in Sizing Data Transfer Object
            foreach (var warpingBeamProduct in warpingListBeamProducts)
            {
                foreach (var beamWarpingId in sizingBeamsWarping)
                {
                    await Task.Yield();
                    if (warpingBeamProduct.Id == beamWarpingId.Identity)
                    {
                        //Get Beam Document
                        await Task.Yield();
                        var beamDocument =
                            _beamRepository
                                .Find(o => o.Identity == beamWarpingId.Identity)
                                .FirstOrDefault();

                        await Task.Yield();
                        var warpingBeamUsedInSizing = new DailyOperationSizingBeamsWarpingDto(warpingBeamProduct, beamDocument.Number);
                        result.AddBeamsWarping(warpingBeamUsedInSizing);
                    }
                }
            }

            // Add Beam Product to Data Transfer Object
            foreach (var beamProduct in dailyOperationSizingDocument.SizingBeamProducts)
            {
                await Task.Yield();
                var beam =
                    _beamRepository
                        .Find(o => o.Identity.Equals(beamProduct.SizingBeamId))
                        .FirstOrDefault();

                var beamSizing = new DailyOperationSizingBeamProductDto(beamProduct, beam);

                await Task.Yield();
                result.AddDailyOperationSizingBeamProducts(beamSizing);
            }
            result.DailyOperationSizingBeamProducts = result.DailyOperationSizingBeamProducts
                                                            .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct)
                                                            .ToList();

            //Add History to Data Transfer Object
            foreach (var history in dailyOperationSizingDocument.SizingHistories)
            {
                //Determine Sizing Beam Number
                var sizingBeamNumber = history.SizingBeamNumber;
                switch (history.MachineStatus)
                {
                    case "ENTRY":
                        sizingBeamNumber = "Belum ada Beam yang Diproses";
                        break;
                    case "FINISH":
                        sizingBeamNumber = "Operasi Selesai, Tidak ada Beam yang Diproses";
                        break;
                    default:
                        sizingBeamNumber = history.SizingBeamNumber;
                        break;
                }

                await Task.Yield();
                var shiftName =
                    _shiftRepository
                        .Find(entity => entity.Identity == history.ShiftDocumentId.Value)
                        .FirstOrDefault().Name ?? "Shift Not Found";

                await Task.Yield();
                var operatorDocument =
                    _operatorRepository
                        .Find(entity => entity.Identity == history.OperatorDocumentId.Value)
                        .FirstOrDefault();

                var dailyHistory =
                    new DailyOperationSizingHistoryDto(history.Identity,
                                                       sizingBeamNumber,
                                                       history.DateTimeMachine,
                                                       shiftName,
                                                       operatorDocument.CoreAccount.Name,
                                                       operatorDocument.Group,
                                                       history.MachineStatus,
                                                       history.Information,
                                                       history.BrokenPerShift,
                                                       history.MachineTroubled);

                await Task.Yield();
                result.AddDailyOperationSizingHistories(dailyHistory);
            }
            result.DailyOperationSizingHistories = result.DailyOperationSizingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
