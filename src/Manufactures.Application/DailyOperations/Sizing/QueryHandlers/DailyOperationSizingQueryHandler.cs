using ExtCore.Data.Abstractions;
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers
{
    public class DailyOperationSizingQueryHandler : IDailyOperationSizingQuery<DailyOperationSizingListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;
        private readonly IWeavingOrderDocumentRepository
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

        public DailyOperationSizingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
            _orderProductionRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
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
        public async Task<IEnumerable<DailyOperationSizingListDto>> GetAll()
        {
            var query =
                _dailyOperationSizingRepository
                    .Query
                    .Include(o => o.SizingBeamProducts)
                    .Include(o => o.SizingHistories)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationSizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(query);
            var result = new List<DailyOperationSizingListDto>();

            foreach (var operation in dailyOperationSizingDocuments)
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
                        .Equals(OrderDocument.ConstructionId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Weaving Unit
                await Task.Yield();
                var weavingUnit = OrderDocument.UnitId.Value;

                //Set Another Properties with Value
                operationResult.SetMachineNumber(machineNumber);
                operationResult.SetOrderProductionNumber(orderNumber);
                operationResult.SetFabricConstructionNumber(constructionNumber);
                operationResult.SetWeavingUnitId(weavingUnit);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationSizingListDto> GetById(Guid id)
        {
            //Prepare Daily Operation Sizing
            var query =
                _dailyOperationSizingRepository
                    .Query
                    .Include(o => o.SizingBeamProducts)
                    .Include(o => o.SizingHistories)
                    .Where(doc => doc.Identity.Equals(id))
                    .OrderByDescending(x => x.CreatedDate);

            //Get Daily Operation Sizing from Query
            await Task.Yield();
            var dailyOperationSizingDocument =
                   _dailyOperationSizingRepository
                       .Find(query)
                       .FirstOrDefault();

            //Get Order Production Number
            await Task.Yield();
            var orderProductionDocument =
                _orderProductionRepository
                    .Find(o => o.Identity.Equals(dailyOperationSizingDocument.OrderDocumentId.Value))
                    .FirstOrDefault();
            var orderProductionNumber = orderProductionDocument.OrderNumber;

            //Get Construction Number
            await Task.Yield();
            var fabricConstructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity.Equals(orderProductionDocument.ConstructionId.Value))
                    .FirstOrDefault()
                    .ConstructionNumber;

            //Get Machine Number
            await Task.Yield();
            var machineDocument =
                _machineRepository
                    .Find(o => o.Identity.Equals(dailyOperationSizingDocument.MachineDocumentId.Value))
                    .FirstOrDefault();
            var machineNumber = machineDocument.MachineNumber;

            //Get Machine Type
            await Task.Yield();
            var machineTypeName =
                _machineTypeRepository
                    .Find(o => o.Identity.Equals(machineDocument.MachineTypeId.Value))
                    .FirstOrDefault()
                    .TypeName;

            //Not complete for detail
            var result = new DailyOperationSizingByIdDto(dailyOperationSizingDocument);
            //double totalWarpingBeamLength = 0;
            result.SetOrderProductionNumber(orderProductionNumber);
            result.SetFabricConstructionNumber(fabricConstructionNumber);
            result.SetWeavingUnitId(orderProductionDocument.UnitId.Value);
            result.SetMachineNumber(machineNumber);

            result.SetMachineType(machineTypeName);
            result.SetEmptyWeight(dailyOperationSizingDocument.EmptyWeight);
            result.SetYarnStrands(dailyOperationSizingDocument.YarnStrands);
            result.SetNeReal(dailyOperationSizingDocument.NeReal);

            //Add Beams Warping Used in Sizing Operation to Data Transfer Object
            //Get Beam Product of Warping That Used Same Order With Current Sizing Operation
            await Task.Yield();
            //var warpingQuery =
            //    _dailyOperationWarpingRepository
            //            .Query
            //            .Include(x => x.WarpingHistories)
            //            .Include(x => x.WarpingBeamProducts)
            //            .Where(doc => doc.OrderDocumentId.Equals(dailyOperationSizingDocument.OrderDocumentId.Value));

            await Task.Yield();
            var warpingDocument =
                _dailyOperationWarpingRepository
                        .Find(x => x.OrderDocumentId == dailyOperationSizingDocument.OrderDocumentId.Value);

            //Get ALL BEAM PRODUCT OF WARPING That Used Same Order With Current Sizing Operation And Add to Warping Beam Data Transfer Object
            List<DailyOperationWarpingBeamDto> warpingListBeamProducts = new List<DailyOperationWarpingBeamDto>();
            foreach (var warping in warpingDocument)
            {
                foreach(var warpingBeamProduct in warping.WarpingBeamProducts)
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

            //Get ONLY BEAM PRODUCT OF WARPING Used in The Current Sizing Operation And Add to Warping Beam Warping Used in Sizing Data Transfer Object
            foreach (var warpingBeamProduct in warpingListBeamProducts)
            {
                foreach(var beamWarpingId in dailyOperationSizingDocument.BeamsWarping)
                {
                    await Task.Yield();
                    if (warpingBeamProduct.Id.Equals(beamWarpingId.Value))
                    {
                        //Get Beam Document
                        await Task.Yield();
                        var beamDocument =
                            _beamRepository
                                .Find(o => o.Identity.Equals(beamWarpingId.Value))
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
                        .Find(entity => entity.Identity.Equals(history.ShiftDocumentId))
                        .FirstOrDefault().Name ?? "Shift Not Found";

                await Task.Yield();
                var operatorDocument =
                    _operatorRepository
                        .Find(entity => entity.Identity.Equals(history.OperatorDocumentId))
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
                                                       history.BrokenBeam,
                                                       history.MachineTroubled);

                await Task.Yield();
                result.AddDailyOperationSizingHistories(dailyHistory);
            }
            result.DailyOperationSizingHistories = result.DailyOperationSizingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
