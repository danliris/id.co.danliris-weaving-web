using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.CoreMicroservice;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Infrastructure.External.DanLirisClient.CoreMicroservice.MasterResult;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Domain.Suppliers.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.QueryHandlers
{
    public class DailyOperationLoomQueryHandler : IDailyOperationLoomQuery<DailyOperationLoomListDto>, IDailyOperationLoomBeamsUsedQuery<DailyOperationLoomBeamsUsedDto>
    {
        protected readonly IHttpClientService
            _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;
        private readonly IOrderRepository
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWeavingSupplierRepository
            _supplierRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;
        //private readonly IBeamRepository
        //    _beamRepository;

        public DailyOperationLoomQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _http =
                serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository =
                _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
            _orderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _supplierRepository =
                _storage.GetRepository<IWeavingSupplierRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
            //_beamRepository =
            //    _storage.GetRepository<IBeamRepository>();
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

        public async Task<IEnumerable<DailyOperationLoomListDto>> GetAll()
        {
            var loomQuery =
                _dailyOperationLoomDocumentRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            var dailyOperationLoomDocuments =
                    _dailyOperationLoomDocumentRepository
                        .Find(loomQuery);

            var result = new List<DailyOperationLoomListDto>();

            foreach (var loomDocument in dailyOperationLoomDocuments)
            {
                var loomHistories =
                    _dailyOperationLoomHistoryRepository
                        .Find(x => x.DailyOperationLoomDocumentId == loomDocument.Identity);
                var loomBeamsUsed =
                    _dailyOperationLoomBeamUsedRepository
                        .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity);

                //Get Order Number
                await Task.Yield();
                var orderDocument =
                    _orderDocumentRepository
                        .Find(o => o.Identity == loomDocument.OrderDocumentId)
                        .FirstOrDefault();
                var orderNumber = orderDocument.OrderNumber;

                //Get Weaving Unit Id
                SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
                var weavingUnitName = unitData.data.Name;

                //Get Construction Number
                await Task.Yield();
                var constructionId = orderDocument.ConstructionDocumentId.Value;
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(o => o.Identity == orderDocument.ConstructionDocumentId.Value)
                        .FirstOrDefault()?
                        .ConstructionNumber;

                //Get Warp Origin Code and Weft Origin Code
                await Task.Yield();
                var warpId = orderDocument.WarpOriginIdOne.Value;
                var weftId = orderDocument.WeftOriginIdOne.Value;

                await Task.Yield();
                var warpCode =
                    _supplierRepository
                        .Find(o => o.Identity == warpId)
                        .FirstOrDefault()?
                        .Code;

                await Task.Yield();
                var weftCode =
                    _supplierRepository
                        .Find(o => o.Identity == weftId)
                        .FirstOrDefault()?
                        .Code;

                await Task.Yield();
                var latestLoomHistory = loomHistories.OrderByDescending(o => o.AuditTrail.CreatedDate).FirstOrDefault();

                //Get Latest Date Time Machine
                var latestLoomHistoryDateTime = latestLoomHistory.DateTimeMachine;

                //Create Shell (DailyOperationLoomListDto Data Type) for Loom Dto
                var loomDto = new DailyOperationLoomListDto(loomDocument);

                loomDto.SetDateTimeOperation(latestLoomHistoryDateTime);
                loomDto.SetWeavingUnit(weavingUnitName);
                loomDto.SetOrderProductionNumber(orderNumber);
                loomDto.SetFabricConstructionNumber(constructionNumber);
                loomDto.SetWarpOrigin(warpCode);
                loomDto.SetWeftOrigin(weftCode);

                result.Add(loomDto);
            }

            return result;
        }

        public async Task<DailyOperationLoomListDto> GetById(Guid id)
        {
            var loomDocument =
                    _dailyOperationLoomDocumentRepository
                        .Find(s => s.Identity == id)
                        .FirstOrDefault();

            //Get Order Number
            await Task.Yield();
            var orderDocument =
                _orderDocumentRepository
                    .Find(o => o.Identity == loomDocument.OrderDocumentId)
                    .FirstOrDefault();
            var orderNumber = orderDocument.OrderNumber ?? "No. Perintah Produksi Tidak Ditemukan";

            //Get Weaving Unit Id
            SingleUnitResult unitData = GetUnit(orderDocument.UnitId.Value);
            var weavingUnitName = unitData.data.Name ?? "Unit Weaving Tidak Ditemukan";

            //Get Construction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity == orderDocument.ConstructionDocumentId.Value)
                    .FirstOrDefault()?
                    .ConstructionNumber ?? "No. Konstruksi Tidak Ditemukan";

            await Task.Yield();
            var warpCode =
                _supplierRepository
                    .Find(o => o.Identity == orderDocument.WarpOriginIdOne.Value)
                    .FirstOrDefault()?
                    .Code ?? "Kode Lusi Tidak Ditemukan";

            await Task.Yield();
            var weftCode =
                _supplierRepository
                    .Find(o => o.Identity == orderDocument.WeftOriginIdOne.Value)
                    .FirstOrDefault()?
                    .Code ?? "Kode Pakan Tidak Ditemukan";

            //Add Shell (DailyOperationLoomByIdDto Data Type) for Loom By Id Dto
            var result = new DailyOperationLoomByIdDto(loomDocument, loomDocument.BeamProcessed);
            result.SetWeavingUnit(weavingUnitName);
            result.SetOrderProductionNumber(orderNumber);
            result.SetFabricConstructionNumber(constructionNumber);
            result.SetWarpOrigin(warpCode);
            result.SetWeftOrigin(weftCode);

            var loomBeamsUsed =
                _dailyOperationLoomBeamUsedRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity);

            foreach (var loomBeamUsed in loomBeamsUsed)
            {
                await Task.Yield();
                var loomBeamProductDto = new DailyOperationLoomBeamsUsedDto(loomBeamUsed.Identity,
                                                                            loomBeamUsed.BeamOrigin,
                                                                            loomBeamUsed.BeamNumber,
                                                                            loomBeamUsed.BeamDocumentId,
                                                                            loomBeamUsed.TyingMachineNumber,
                                                                            loomBeamUsed.LoomMachineNumber,
                                                                            loomBeamUsed.LastTyingOperatorDocumentId ?? Guid.Empty,
                                                                            loomBeamUsed.StartCounter ?? 0,
                                                                            loomBeamUsed.FinishCounter ?? 0,
                                                                            loomBeamUsed.MachineSpeed ?? 0,
                                                                            loomBeamUsed.SCMPX ?? 0,
                                                                            loomBeamUsed.Efficiency ?? 0,
                                                                            loomBeamUsed.F ?? 0,
                                                                            loomBeamUsed.W ?? 0,
                                                                            loomBeamUsed.L ?? 0,
                                                                            loomBeamUsed.T ?? 0,
                                                                            loomBeamUsed.UomUnit ?? "-",
                                                                            loomBeamUsed.LastDateTimeProcessed,
                                                                            loomBeamUsed.BeamUsedStatus,
                                                                            loomBeamUsed.DailyOperationLoomDocumentId);

                //loomBeamProductDto.SetBeamDocumentId(loomBeamProduct.BeamDocumentId.Value);

                await Task.Yield();
                result.AddDailyOperationLoomBeamProducts(loomBeamProductDto);
            }
            result.DailyOperationLoomBeamsUsed = result.DailyOperationLoomBeamsUsed.OrderByDescending(o => o.BeamNumber).ThenByDescending(o => o.LatestDateTimeProcessed).ToList();

            var loomHistories =
                _dailyOperationLoomHistoryRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity);

            foreach (var loomHistory in loomHistories)
            {
                //Get Tying Machine Number
                await Task.Yield();
                var tyingMachineNumber =
                    _machineRepository
                        .Find(o => o.Identity == loomHistory.TyingMachineId.Value)
                        .FirstOrDefault()?
                        .MachineNumber ?? "-";

                //Get Tying Operator Name and Group
                var tyingOperatorDocument =
                    _operatorRepository
                        .Find(o => o.Identity == loomHistory.TyingOperatorId.Value)
                        .FirstOrDefault();
                var tyingOperatorName = "";
                var tyingOperatorGroup = "";
                if (tyingOperatorDocument != null)
                {
                    tyingOperatorName = tyingOperatorDocument.CoreAccount.Name;
                    tyingOperatorGroup = tyingOperatorDocument.Group;
                }
                else
                {
                    tyingOperatorName = "-";
                    tyingOperatorGroup = "-";
                }

                //Get Loom Machine Number
                await Task.Yield();
                var loomMachineNumber =
                    _machineRepository
                        .Find(o => o.Identity == loomHistory.LoomMachineId)
                        .FirstOrDefault()?
                        .MachineNumber ?? "-";

                //Get Loom Operator Name and Group
                var loomOperatorDocument =
                    _operatorRepository
                        .Find(o => o.Identity == loomHistory.LoomOperatorId)
                        .FirstOrDefault();
                var loomOperatorName = "";
                var loomOperatorGroup = "";
                if (loomOperatorDocument != null)
                {
                    loomOperatorName = loomOperatorDocument.CoreAccount.Name;
                    loomOperatorGroup = loomOperatorDocument.Group;
                }
                else
                {
                    loomOperatorName = "-";
                    loomOperatorGroup = "-";
                }

                //Get Shift Name
                await Task.Yield();
                var shiftId = loomHistory.ShiftDocumentId;
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity == shiftId)
                        .FirstOrDefault()
                        .Name ?? "-";

                await Task.Yield();
                var loomBeamHistoryDto = new DailyOperationLoomHistoryDto(loomHistory.Identity,
                                                                          loomHistory.BeamNumber,
                                                                          tyingMachineNumber,
                                                                          tyingOperatorName,
                                                                          tyingOperatorGroup,
                                                                          loomMachineNumber,
                                                                          loomOperatorName,
                                                                          loomOperatorGroup,
                                                                          loomHistory.DateTimeMachine,
                                                                          shiftName,
                                                                          loomHistory.Information,
                                                                          loomHistory.MachineStatus);

                await Task.Yield();
                result.AddDailyOperationLoomBeamHistories(loomBeamHistoryDto);
            }
            result.DailyOperationLoomBeamHistories = result.DailyOperationLoomBeamHistories.OrderByDescending(o => o.BeamNumber).ThenByDescending(o => o.DateTimeMachine).ToList();

            return result;
        }

        public async Task<IEnumerable<DailyOperationLoomBeamsUsedDto>> GetAllBeamsUsed()
        {
            await Task.Yield();
            var loomBeamsUsedQuery =
                _dailyOperationLoomBeamUsedRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            var loomBeamsUsedDto =
                    _dailyOperationLoomBeamUsedRepository
                        .Find(loomBeamsUsedQuery)
                        .Select(x => new DailyOperationLoomBeamsUsedDto(x.Identity,
                                                                        x.BeamOrigin,
                                                                        x.BeamNumber,
                                                                        x.BeamDocumentId,
                                                                        x.TyingMachineNumber,
                                                                        x.LoomMachineNumber,
                                                                        x.LastTyingOperatorDocumentId ?? Guid.Empty,
                                                                        x.StartCounter ?? 0,
                                                                        x.FinishCounter ?? 0,
                                                                        x.MachineSpeed ?? 0,
                                                                        x.SCMPX ?? 0,
                                                                        x.Efficiency ?? 0,
                                                                        x.F ?? 0,
                                                                        x.W ?? 0,
                                                                        x.L ?? 0,
                                                                        x.T ?? 0,
                                                                        x.UomUnit ?? "-",
                                                                        x.LastDateTimeProcessed,
                                                                        x.BeamUsedStatus,
                                                                        x.DailyOperationLoomDocumentId));

            return loomBeamsUsedDto;
        }

        //public async Task<IEnumerable<DailyOperationLoomBeamsUsedDto>> GetBeamsUsedById(Guid id)
        //{
        //    await Task.Yield();
        //    var loomBeamsUsedByIdDto =
        //            _dailyOperationLoomBeamUsedRepository
        //                .Find(o => o.DailyOperationLoomDocumentId == id)
        //                .Select(x=>new DailyOperationLoomBeamsUsedDto(x.Identity,
        //                                                              x.BeamOrigin,
        //                                                              x.BeamNumber,
        //                                                              x.StartCounter ?? 0,
        //                                                              x.FinishCounter ?? 0,
        //                                                              x.MachineSpeed ?? 0,
        //                                                              x.SCMPX ?? 0,
        //                                                              x.Efficiency ?? 0,
        //                                                              x.F ?? 0,
        //                                                              x.W ?? 0,
        //                                                              x.L ?? 0,
        //                                                              x.T ?? 0,
        //                                                              x.UomUnit ?? "-",
        //                                                              x.LastDateTimeProcessed,
        //                                                              x.BeamUsedStatus,
        //                                                              x.DailyOperationLoomDocumentId));

        //    return loomBeamsUsedByIdDto;
        //}

        //public async Task<IEnumerable<DailyOperationLoomBeamsUsedDto>> GetBeamsUsedByIdProcessed(Guid id)
        //{
        //    await Task.Yield();
        //    var loomBeamsUsedByIdProcessedDto =
        //            _dailyOperationLoomBeamUsedRepository
        //                .Find(o => o.DailyOperationLoomDocumentId == id && o.BeamUsedStatus == BeamStatus.ONPROCESS)
        //                .Select(x => new DailyOperationLoomBeamsUsedDto(x.Identity,
        //                                                              x.BeamOrigin,
        //                                                              x.BeamNumber,
        //                                                              x.StartCounter ?? 0,
        //                                                              x.FinishCounter ?? 0,
        //                                                              x.MachineSpeed ?? 0,
        //                                                              x.SCMPX ?? 0,
        //                                                              x.Efficiency ?? 0,
        //                                                              x.F ?? 0,
        //                                                              x.W ?? 0,
        //                                                              x.L ?? 0,
        //                                                              x.T ?? 0,
        //                                                              x.UomUnit ?? "-",
        //                                                              x.LastDateTimeProcessed,
        //                                                              x.BeamUsedStatus,
        //                                                              x.DailyOperationLoomDocumentId));

        //    return loomBeamsUsedByIdProcessedDto;
        //}
    }
}
