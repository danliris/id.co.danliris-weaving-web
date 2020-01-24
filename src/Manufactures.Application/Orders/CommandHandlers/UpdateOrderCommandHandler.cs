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
            var year = request.Period.Year;
            var month = request.Period.Month;
            var day = request.Period.Day;
            var period = new DateTime(year, month, day);

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
            order.SetWarpOrigin(request.WarpOrigin);
            order.SetWarpCompositionPoly(request.WarpCompositionPoly);
            order.SetWarpCompositionCotton(request.WarpCompositionCotton);
            order.SetWarpCompositionOthers(request.WarpCompositionOthers);
            order.SetWeftOrigin(request.WeftOrigin);
            order.SetWeftCompositionPoly(request.WeftCompositionPoly);
            order.SetWeftCompositionCotton(request.WeftCompositionCotton);
            order.SetWeftCompositionOthers(request.WeftCompositionOthers);
            order.SetAllGrade(request.AllGrade);
            order.SetUnit(request.Unit);

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
