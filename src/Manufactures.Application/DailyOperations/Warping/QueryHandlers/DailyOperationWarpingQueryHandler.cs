using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
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
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandler : IDailyOperationWarpingQuery<DailyOperationWarpingListDto>
    {
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
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IWarpingBrokenCauseRepository
            _warpingBrokenCauseRepository;

        public DailyOperationWarpingQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
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
        }

        public async Task<IEnumerable<DailyOperationWarpingListDto>> GetAll()
        {
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.WarpingBeamProducts)
                    .Include(o => o.WarpingHistories)
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(query);
            var result = new List<DailyOperationWarpingListDto>();

            foreach (var operation in dailyOperationWarpingDocuments)
            {
                //initiate Operation Result
                var operationResult = new DailyOperationWarpingListDto(operation);

                //Get Order Document
                await Task.Yield();
                var OrderDocument =
                    _weavingOrderDocumentRepository
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
                operationResult.SetOrderProductionNumber(orderNumber);
                operationResult.SetConstructionNumber(constructionNumber);
                operationResult.SetWeavingUnitId(weavingUnit);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationWarpingListDto> GetById(Guid id)

        {
            //Prepare Daily Operation Warping
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.WarpingBeamProducts)
                    .ThenInclude(o => o.WarpingBrokenThreadsCauses)
                    .Include(o => o.WarpingHistories)
                    .Where(doc => doc.Identity.Equals(id))
                    .OrderByDescending(x => x.CreatedDate);

            //Get Daily Operation Sizing from Query
            await Task.Yield();
            var dailyOperationWarpingDocument =
                   _dailyOperationWarpingRepository
                       .Find(query)
                       .FirstOrDefault();

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
                    .Find(o => o.Identity.Equals(orderDocument.ConstructionId.Value))
                    .FirstOrDefault()
                    .ConstructionNumber;

            //Get MaterialName 
            await Task.Yield();
            var splittedConstruction = constructionNumber.Split(" ");
            var warpMaterialName = splittedConstruction[splittedConstruction.Length - 2];

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
            result.SetWeavingUnitId(orderDocument.UnitId.Value);

            // Add Beam Product to Data Transfer Objects
            foreach (var beamProduct in dailyOperationWarpingDocument.WarpingBeamProducts)
            {
                await Task.Yield();
                var beam =
                    _beamRepository
                        .Find(o => o.Identity.Equals(beamProduct.WarpingBeamId))
                        .FirstOrDefault();

                var beamWarping = new DailyOperationWarpingBeamProductDto(beamProduct, beam);

                foreach (var brokenCauseDocument in beamProduct.WarpingBrokenThreadsCauses)
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

            //int beamProductLength = result.DailyOperationWarpingBeamProducts.Count();
            //var isFinishFlagBeamProduct = false;
            //if(beamProductLength == result.BeamProductResult)
            //{
            //    isFinishFlagBeamProduct = true;
            //}
            //result.SetIsFinishFlag(isFinishFlagBeamProduct);

            //foreach(var beam in result.DailyOperationWarpingBeamProducts)
            //{
            //    totalWarpingBeamLength = beam.WarpingTotalBeamLength + totalWarpingBeamLength;
            //}
            //result.SetTotalWarpingBeamLength(totalWarpingBeamLength);
            //int countWarpingBeamProducts = result.DailyOperationWarpingBeamProducts.Count();
            //result.SetCountWarpingBeamProducts(countWarpingBeamProducts);

            //Add History to Data Transfer Objects
            foreach (var history in dailyOperationWarpingDocument.WarpingHistories)
            {
                var warpingBeamDocument =
                    _beamRepository
                        .Find(o => o.Identity.Equals(history.WarpingBeamId))
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
                        .Find(entity => entity.Identity.Equals(history.ShiftDocumentId))
                        .FirstOrDefault().Name ?? "Shift Not Found";

                await Task.Yield();
                var operatorBeam =
                    _operatorRepository
                        .Find(entity => entity.Identity.Equals(history.OperatorDocumentId))
                        .FirstOrDefault();

                await Task.Yield();
                var warpingBeamLengthPerOperator = history.WarpingBeamLengthPerOperator.ToString() ?? "Belum ada Beam yang Diproses Operator";

                var dailyHistory =
                    new DailyOperationWarpingHistoryDto(history.Identity,
                                                        warpingBeamNumber,
                                                        history.DateTimeMachine,
                                                        shiftName,
                                                        operatorBeam.CoreAccount.Name,
                                                        operatorBeam.Group,
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
