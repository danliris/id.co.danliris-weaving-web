using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.CommandHandlers
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, OrderDocument>
    {
        private readonly IStorage _storage;
        private readonly IOrderRepository _orderDocumentRepository;

        public UpdateOrderCommandHandler(IStorage storage)
        {
            _storage = storage;
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }


        public async Task<OrderDocument> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            //Get Specified Order By Request Id
            var order =
                _orderDocumentRepository
                    .Find(o => o.Identity == request.Id)
                    .FirstOrDefault();

            //Throw if Order Not Found
            if (order == null)
            {
                throw Validator.ErrorValidation(("Id", "Id Produksi " + request.Id + " Tidak Valid"));
            }

            //Generate Period
            var year = request.Year;
            var month = request.Month;
            DateTime period = new DateTime(year, month, 1);

            //Tunggu Jawaban
            var warpCompositionPolyLimit = Math.Round(request.WarpCompositionPoly, 4);
            var warpCompositionCottonLimit = Math.Round(request.WarpCompositionCotton, 4);
            var warpCompositionOthersLimit = Math.Round(request.WarpCompositionOthers, 4);
            var weftCompositionPolyLimit = Math.Round(request.WeftCompositionPoly, 4);
            var weftCompositionCottonLimit = Math.Round(request.WeftCompositionCotton, 4);
            var weftCompositionOthersLimit = Math.Round(request.WeftCompositionOthers, 4);
            var allGradeLimit = Math.Round(request.AllGrade, 4);

            //Set Value Based on Request
            order.SetPeriod(period);
            order.SetConstructionDocumentId(request.ConstructionDocumentId);
            order.SetYarnType(request.YarnType);
            order.SetWarpOriginOne(request.WarpOriginIdOne);
            //order.SetWarpOriginTwo(request.WarpOriginIdTwo ?? order.WarpOriginIdTwo);
            order.SetWarpCompositionPoly(warpCompositionPolyLimit);
            order.SetWarpCompositionCotton(warpCompositionCottonLimit);
            order.SetWarpCompositionOthers(warpCompositionOthersLimit);
            order.SetWeftOriginOne(request.WeftOriginIdOne);
            order.SetWeftOriginTwo(request.WeftOriginIdTwo ?? order.WeftOriginIdTwo);
            order.SetWeftCompositionPoly(weftCompositionPolyLimit);
            order.SetWeftCompositionCotton(weftCompositionCottonLimit);
            order.SetWeftCompositionOthers(weftCompositionOthersLimit);
            order.SetAllGrade(allGradeLimit);
            order.SetUnit(request.UnitId);

            //Mark Order Modified
            order.SetModified();

            //Update
            await _orderDocumentRepository.Update(order);

            //Save
            _storage.Save();

            //Return as Order Object
            return order;
        }
    }
}
