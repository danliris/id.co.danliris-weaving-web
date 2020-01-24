using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.QueryHandlers
{
    public class DailyOperationLoomQueryHandler : IDailyOperationLoomQuery<DailyOperationLoomListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomRepository;
        private readonly IOrderRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWeavingSupplierRepository
            _weavingSupplierRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public DailyOperationLoomQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _weavingSupplierRepository =
                _storage.GetRepository<IWeavingSupplierRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public async Task<IEnumerable<DailyOperationLoomListDto>> GetAll()
        {
            var loomQuery =
                _dailyOperationLoomRepository
                    .Query
                    .Include(o => o.LoomBeamProducts)
                    .Include(o => o.LoomBeamHistories)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationLoomDocuments =
                    _dailyOperationLoomRepository
                        .Find(loomQuery);
            var result = new List<DailyOperationLoomListDto>();

            foreach (var loomDocument in dailyOperationLoomDocuments)
            {
                //Get Order Number
                await Task.Yield();
                var orderDocument =
                    _weavingOrderDocumentRepository
                        .Find(o => o.Identity.Equals(loomDocument.OrderDocumentId.Value))
                        .FirstOrDefault();
                var orderNumber = orderDocument.OrderNumber;

                //Get Weaving Unit
                var unitWeavingId = orderDocument.UnitId.Value;

                //Get Construction Number
                await Task.Yield();
                var constructionId = orderDocument.ConstructionId.Value;
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(o => o.Identity.Equals(constructionId))
                        .FirstOrDefault()
                        .ConstructionNumber;

                //Get Warp Origin Code and Weft Origin Code
                await Task.Yield();
                var warpId = orderDocument.WarpOrigin;
                var weftId = orderDocument.WeftOrigin;
                
                await Task.Yield();
                var warpCode =
                    _weavingSupplierRepository
                        .Find(o => o.Identity.ToString().Equals(warpId))
                        .FirstOrDefault()
                        .Code;

                await Task.Yield();
                var weftCode =
                    _weavingSupplierRepository
                        .Find(o => o.Identity.ToString().Equals(weftId))
                        .FirstOrDefault()
                        .Code;

                await Task.Yield();
                var loomHistories = loomDocument.LoomBeamHistories.OrderByDescending(o => o.CreatedDate);
                var latestLoomHistory = loomHistories.FirstOrDefault();

                //Get Latest Date Time Machine
                var latestLoomHistoryDateTime = latestLoomHistory.DateTimeMachine;

                //Create Shell (DailyOperationLoomListDto Data Type) for Loom Dto
                var loomDto = new DailyOperationLoomListDto(loomDocument);

                loomDto.SetDateTimeMachine(latestLoomHistoryDateTime);
                loomDto.SetWeavingUnitId(unitWeavingId);
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
            var loomQuery =
                _dailyOperationLoomRepository
                    .Query
                    .Include(o => o.LoomBeamHistories)
                    .Include(o => o.LoomBeamProducts)
                    .Where(o => o.Identity.Equals(id))
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationLoomDocument =
                    _dailyOperationLoomRepository
                        .Find(loomQuery)
                        .FirstOrDefault();

            //Get Order Number
            await Task.Yield();
            var orderDocument =
                _weavingOrderDocumentRepository
                    .Find(o => o.Identity.Equals(dailyOperationLoomDocument.OrderDocumentId.Value))
                    .FirstOrDefault();
            var orderNumber = orderDocument.OrderNumber;

            //Get Weaving Unit
            var unitWeavingId = orderDocument.UnitId.Value;

            //Get Construction Number
            await Task.Yield();
            var constructionId = orderDocument.ConstructionId.Value;
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity.Equals(constructionId))
                    .FirstOrDefault()
                    .ConstructionNumber;

            //Get Warp Origin Code and Weft Origin Code
            await Task.Yield();
            var warpId = orderDocument.WarpOrigin;
            var weftId = orderDocument.WeftOrigin;

            await Task.Yield();
            var warpCode =
                _weavingSupplierRepository
                    .Find(o => o.Identity.ToString().Equals(warpId))
                    .FirstOrDefault()
                    .Code;

            await Task.Yield();
            var weftCode =
                _weavingSupplierRepository
                    .Find(o => o.Identity.ToString().Equals(weftId))
                    .FirstOrDefault()
                    .Code;

            //Add Shell (DailyOperationLoomByIdDto Data Type) for Loom By Id Dto
            var result = new DailyOperationLoomByIdDto(dailyOperationLoomDocument);
            result.SetWeavingUnitId(unitWeavingId);
            result.SetOrderProductionNumber(orderNumber);
            result.SetFabricConstructionNumber(constructionNumber);
            result.SetWarpOrigin(warpCode);
            result.SetWeftOrigin(weftCode);

            foreach (var loomBeamProduct in dailyOperationLoomDocument.LoomBeamProducts)
            {
                //Get Beam Number
                await Task.Yield();
                //var beamQuery =
                //    _beamRepository
                //        .Query
                //        .Where(o => o.Identity.Equals(loomBeamProduct.BeamDocumentId))
                //        .OrderByDescending(o => o.CreatedDate);
                var beamNumber =
                    _beamRepository
                        .Find(o => o.Identity.Equals(loomBeamProduct.BeamDocumentId))
                        .FirstOrDefault()
                        .Number;

                //Get Machine Number
                await Task.Yield();
                //var machineQuery =
                //    _machineRepository
                //        .Query
                //        .OrderByDescending(o => o.CreatedDate);
                var machineNumber =
                    _machineRepository
                        .Find(o => o.Identity.Equals(loomBeamProduct.MachineDocumentId))
                        .FirstOrDefault()
                        .MachineNumber;

                //Get Date Time Beam Product
                await Task.Yield();
                var dateTimeBeamProduct = loomBeamProduct.LatestDateTimeBeamProduct;

                //Get Beam Process Status
                var beamProductProcess = loomBeamProduct.LoomProcess;

                //Get Beam Product Status
                var beamStatus = loomBeamProduct.BeamProductStatus;

                await Task.Yield();
                var loomBeamProductDto = new DailyOperationLoomBeamProductDto(loomBeamProduct.Identity,
                                                                              loomBeamProduct.BeamOrigin,
                                                                              beamNumber,
                                                                              loomBeamProduct.CombNumber,
                                                                              machineNumber,
                                                                              dateTimeBeamProduct,
                                                                              beamProductProcess,
                                                                              beamStatus);

                loomBeamProductDto.SetBeamDocumentId(loomBeamProduct.BeamDocumentId);

                await Task.Yield();
                result.AddDailyOperationLoomBeamProducts(loomBeamProductDto);
            }
            result.DailyOperationLoomBeamProducts = result.DailyOperationLoomBeamProducts.OrderByDescending(o => o.LatestDateTimeBeamProduct).ToList();

            foreach (var loomBeamHistory in dailyOperationLoomDocument.LoomBeamHistories)
            {
                //Get Beam Number
                var beamNumber = loomBeamHistory.BeamNumber;

                //Get Machine Number
                var machineNumber = loomBeamHistory.MachineNumber;

                //Get Operator Name and Group
                await Task.Yield();
                var operatorId = loomBeamHistory.OperatorDocumentId;
                //var operatorQuery =
                //    _operatorRepository
                //        .Query
                //        .Where(o => o.Identity.Equals(operatorId))
                //        .OrderByDescending(o => o.CreatedDate);
                var operatorDocument =
                    _operatorRepository
                        .Find(o => o.Identity.Equals(operatorId))
                        .FirstOrDefault();
                var operatorName = operatorDocument.CoreAccount.Name;
                var operatorGroup = operatorDocument.Group;

                //Get Date Time Machine
                await Task.Yield();
                var dateTimeMachine = loomBeamHistory.DateTimeMachine;

                //Get Shift Name
                await Task.Yield();
                var shiftId = loomBeamHistory.ShiftDocumentId;
                //var shiftQuery =
                //    _shiftRepository
                //        .Query
                //        .Where(o => o.Identity.Equals(shiftId))
                //        .OrderByDescending(o => o.CreatedDate);
                var shiftName =
                    _shiftRepository
                        .Find(o => o.Identity.Equals(shiftId))
                        .FirstOrDefault()
                        .Name;

                //Get Reprocess To
                await Task.Yield();
                var reprocessTo = loomBeamHistory.ReprocessTo ?? "-";

                //Get Information
                var information = loomBeamHistory.Information ?? "-";

                //Get Machine Status
                var machineStatus = loomBeamHistory.MachineStatus;

                await Task.Yield();
                var loomBeamHistoryDto = new DailyOperationLoomBeamHistoryDto(loomBeamHistory.Identity,
                                                                              beamNumber,
                                                                              machineNumber,
                                                                              operatorName,
                                                                              operatorGroup,
                                                                              dateTimeMachine,
                                                                              shiftName,
                                                                              reprocessTo,
                                                                              information,
                                                                              machineStatus);

                loomBeamHistoryDto.SetWarpBrokenThreads(loomBeamHistory.WarpBrokenThreads ?? 0);
                loomBeamHistoryDto.SetWeftBrokenThreads(loomBeamHistory.WeftBrokenThreads ?? 0);
                loomBeamHistoryDto.SetLenoBrokenThreads(loomBeamHistory.LenoBrokenThreads ?? 0);

                await Task.Yield();
                result.AddDailyOperationLoomBeamHistories(loomBeamHistoryDto);
            }
            result.DailyOperationLoomBeamHistories = result.DailyOperationLoomBeamHistories.OrderByDescending(o => o.DateTimeMachine).ToList();

            return result;
        }
    }
}
