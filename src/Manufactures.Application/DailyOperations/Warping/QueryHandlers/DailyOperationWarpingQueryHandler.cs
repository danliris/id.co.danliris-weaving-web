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
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<IEnumerable<DailyOperationWarpingListDto>> GetAll()
        {
            var query =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(o => o.DailyOperationWarpingBeamProducts)
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

                //Get Contruction Number
                await Task.Yield();
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(entity => entity.Identity
                        .Equals(operation.ConstructionId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                //Get Order Number
                await Task.Yield();
                var OrderNumber =
                    _weavingOrderDocumentRepository
                        .Find(o => o.Identity.Equals(operation.OrderId.Value))
                        .FirstOrDefault()
                        .OrderNumber;

                //Get Latest BeamId
                await Task.Yield();
                var latestBeam =
                    operation
                        .DailyOperationWarpingBeamProducts
                        .OrderByDescending(datetime => datetime.CreatedDate)
                        .FirstOrDefault();

                //Check if has latest beamId
                if (latestBeam != null)
                {
                    //Get Beam Number
                    await Task.Yield();
                    var beamNumber =
                        _beamRepository
                            .Find(entity => entity.Identity.Equals(latestBeam.BeamId))
                            .FirstOrDefault()
                            .Number ?? "Not Found Beam Number";

                    operationResult.SetLatestBeamNumber(beamNumber);
                } else
                {
                    operationResult.SetLatestBeamNumber("Not Available beam on process, status: new entry");
                }

                //complete another field
                operationResult.SetConstructionNumber(constructionNumber);
                operationResult.SetOrderNumber(OrderNumber);

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
                    .Include(o => o.DailyOperationWarpingBeamProducts)
                    .Include(o => o.DailyOperationWarpingDetailHistory)
                    .OrderByDescending(x => x.CreatedDate);

            //Request from query
            await Task.Yield();
            var dailyOperationWarpingDocument =
                   _dailyOperationWarpingRepository
                       .Find(query)
                       .FirstOrDefault();

            //Get Order Number
            await Task.Yield();
            var OrderNumber =
                _weavingOrderDocumentRepository
                    .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.OrderId.Value))
                    .FirstOrDefault()
                    .OrderNumber;

            //Get Construction Number
            await Task.Yield();
            var constructionNumber =
                _fabricConstructionRepository
                    .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.ConstructionId.Value))
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
            await Task.Yield();
            var operatorDocument =
                _operatorRepository
                    .Find(o => o.Identity.Equals(dailyOperationWarpingDocument.OperatorId.Value))
                    .FirstOrDefault();
            
            //Not complete for detail
            var result = new DailyOperationWarpingByIdDto(dailyOperationWarpingDocument);
            result.SetConstructionNumber(constructionNumber);
            result.SetMaterialName(materialName);
            result.SetOperator(operatorDocument);
            result.SetOrderNumber(OrderNumber);

            // Add Beam Product to DTO
            foreach(var beamProduct  in dailyOperationWarpingDocument.DailyOperationWarpingBeamProducts)
            {
                var beamWarping = new DailyOperationBeamProduct(beamProduct);

                await Task.Yield();
                result.AddDailyOperationBeamProducts(beamWarping);
            }

            //Add History to DTO
            foreach(var history in dailyOperationWarpingDocument.DailyOperationWarpingDetailHistory)
            {
                var beamNumber = history.BeamNumber ?? "no beam input, preparing state";

                await Task.Yield();
                var operatorBeam = 
                    _operatorRepository
                        .Find(entity => entity.Identity.Equals(history.BeamOperatorId))
                        .FirstOrDefault();

                await Task.Yield();
                var shiftName = 
                    _shiftRepository
                        .Find(entity => entity.Identity.Equals(history.ShiftId))
                        .FirstOrDefault().Name;

                var dailyHistory = 
                    new DailyOperationLoomHistoryDto(history.Identity, 
                                                     beamNumber, 
                                                     operatorBeam.CoreAccount.Name, 
                                                     operatorBeam.Group, 
                                                     history.DateTimeOperation, 
                                                     history.OperationStatus, 
                                                     shiftName);

                await Task.Yield();
                result.AddDailyOperationLoomHistories(dailyHistory);
            }

            return result;
        }
    }
}
