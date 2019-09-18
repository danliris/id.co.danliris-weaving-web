using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Manufactures.Domain.Beams.Repositories;
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
    public class DailyOperationWarpingQueryHandler : IWarpingQuery<DailyOperationWarpingListDto>
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
                operationResult.SetOrderNumber(orderNumber);
                operationResult.SetConstructionNumber(constructionNumber);
                operationResult.SetWeavingUnitId(weavingUnit);

                result.Add(operationResult);
            }

            return result;
        }

        public async Task<DailyOperationWarpingListDto> GetById(Guid id)
        {
            //Prepare daily operation warping
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.WarpingBeamProducts)
                    .Include(o => o.WarpingHistories)
                    .OrderByDescending(x => x.CreatedDate);

            //Request from query
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
            var materialName =
                _materialTypeRepository
                    .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.MaterialTypeId.Value))
                    .FirstOrDefault()
                    .Name;

            //Get Operator
            //await Task.Yield();
            //var operatorDocument =
            //    _operatorRepository
            //        .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.OperatorId.Value))
            //        .FirstOrDefault();

            //Not complete for detail
            var result = new DailyOperationWarpingByIdDto(dailyOperationWarpingDocument);
            result.SetConstructionNumber(constructionNumber);
            result.SetMaterialName(materialName);
            //result.SetOperator(operatorDocument);
            result.SetOrderNumber(orderDocument.OrderNumber);
            result.SetWeavingUnitId(orderDocument.UnitId.Value);

            // Add Beam Product to DTO
            foreach (var beamProduct in dailyOperationWarpingDocument.WarpingBeamProducts)
            {
                await Task.Yield();
                var beam =
                    _beamRepository
                        .Find(o => o.Identity.Equals(beamProduct.WarpingBeamId))
                        .FirstOrDefault();

                var beamWarping = new DailyOperationWarpingBeamProductDto(beamProduct, beam);

                await Task.Yield();
                result.AddDailyOperationBeamProducts(beamWarping);
            }
            result.DailyOperationWarpingBeamProducts = result.DailyOperationWarpingBeamProducts.OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct).ToList();

            //Add History to DTO
            foreach (var history in dailyOperationWarpingDocument.WarpingHistories)
            {
                var beamNumber = history.WarpingBeamNumber ?? "Belum ada Beam yang Diproses";

                await Task.Yield();
                var operatorBeam =
                    _operatorRepository
                        .Find(entity => entity.Identity.Equals(history.OperatorDocumentId))
                        .FirstOrDefault();

                await Task.Yield();
                var shiftName =
                    _shiftRepository
                        .Find(entity => entity.Identity.Equals(history.ShiftDocumentId))
                        .FirstOrDefault().Name ?? "Shift Not Found";

                var dailyHistory =
                    new DailyOperationWarpingHistoryDto(history.Identity,
                                                        beamNumber,
                                                        history.DateTimeMachine,
                                                        shiftName,
                                                        operatorBeam.CoreAccount.Name,
                                                        operatorBeam.Group,
                                                        history.MachineStatus);

                await Task.Yield();
                result.AddDailyOperationWarpingHistories(dailyHistory);
            }
            result.DailyOperationWarpingHistories = result.DailyOperationWarpingHistories.OrderByDescending(history => history.DateTimeMachine).ToList();

            return result;
        }
    }
}
