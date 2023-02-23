using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.CommandHandlers
{
    public class AddOrderCommandHandler : ICommandHandler<AddOrderCommand, OrderDocument>
    {
        private readonly IStorage _storage;
        private readonly IOrderRepository _orderDocumentRepository;

        public AddOrderCommandHandler(IStorage storage)
        {
            _storage = storage;
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<OrderDocument> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            //Orders Query
            var orders =
                _orderDocumentRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            //Generate Period
            var year = request.Year;
            var month = request.Month;
            var period = new DateTime(year, month, 1);

            //Generate Order Number
            //var countOrderNumber = (orders.Where(o => o.Period.Year == year && o.Period.Month == month).Count() + 1).ToString();
            var countOrderNumber = (orders.Where(o => o.Period.Year == year).Count() + 1).ToString();
            var orderNumber = countOrderNumber.PadLeft(4, '0') + "/" + month.ToString().PadLeft(2, '0') + "-" + year.ToString();

            //Set Status
            var orderStatus = Constants.ONORDER;

            //Tunggu Jawaban
            var warpCompositionPolyLimit = Math.Round(request.WarpCompositionPoly, 4);
            var warpCompositionCottonLimit = Math.Round(request.WarpCompositionCotton, 4);
            var warpCompositionOthersLimit = Math.Round(request.WarpCompositionOthers, 4);
            var weftCompositionPolyLimit = Math.Round(request.WeftCompositionPoly, 4);
            var weftCompositionCottonLimit = Math.Round(request.WeftCompositionCotton, 4);
            var weftCompositionOthersLimit = Math.Round(request.WeftCompositionOthers, 4);
            var allGradeLimit = Math.Round(request.AllGrade, 4);

            //Assign Value from Request to New Order Object
            var order = new OrderDocument(Guid.NewGuid(),
                                          orderNumber,
                                          period,
                                          request.ConstructionDocumentId,
                                          request.YarnType,
                                          request.WarpOriginIdOne,
                                          request.WeftOriginIdOne, 
                                          allGradeLimit,
                                          request.UnitId,
                                          orderStatus);

            order.SetWarpCompositionPoly(warpCompositionPolyLimit);
            order.SetWarpCompositionCotton(warpCompositionCottonLimit);
            order.SetWarpCompositionOthers(warpCompositionOthersLimit);
            order.SetWeftCompositionPoly(weftCompositionPolyLimit);
            order.SetWeftCompositionCotton(weftCompositionCottonLimit);
            order.SetWeftCompositionOthers(weftCompositionOthersLimit);
            //order.SetWarpOriginTwo(request.WarpOriginIdTwo ?? new SupplierId(Guid.Empty));
            order.SetWeftOriginTwo(request.WeftOriginIdTwo ?? new SupplierId(Guid.Empty));

            //Update
            await _orderDocumentRepository.Update(order);

            //Save
            _storage.Save();

            //Return as Order Object
            return order;
        }
    }
}
