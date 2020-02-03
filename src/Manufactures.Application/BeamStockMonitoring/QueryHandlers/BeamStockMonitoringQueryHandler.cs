using ExtCore.Data.Abstractions;
using Manufactures.Application.BeamStockMonitoring.DataTransferObjects;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.BeamStockMonitoring.Queries;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.BeamStockMonitoring.QueryHandlers
{
    public class BeamStockMonitoringQueryHandler : IBeamStockMonitoringQuery<BeamStockMonitoringDto>
    {
        private readonly IStorage _storage;
        private readonly IBeamStockMonitoringRepository
            _beamStockMonitoringRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOrderRepository
            _orderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;

        public BeamStockMonitoringQueryHandler(IStorage storage)
        {
            _storage = storage;
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _orderDocumentRepository =
                _storage.GetRepository<IOrderRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
        }

        public async Task<IEnumerable<BeamStockMonitoringDto>> GetAll()
        {
            var beamStockMonitoringQuery =
                _beamStockMonitoringRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var beamStockMonitoringDocuments =
                    _beamStockMonitoringRepository
                        .Find(beamStockMonitoringQuery);

            var result = new List<BeamStockMonitoringDto>();
            foreach (var stock in beamStockMonitoringDocuments)
            {
                //Get Beam Stock Monitoring Id
                var stockId = stock.Identity;

                //Get Beam Number
                await Task.Yield();
                var beamNumber =
                    _beamRepository
                        .Find(beam => beam.Identity.Equals(stock.BeamDocumentId.Value))
                        .FirstOrDefault()
                        .Number ?? "Not Found Beam Number";

                //Get Entry Date and Stock Length based on Position
                await Task.Yield();
                var entryDate = new DateTimeOffset();
                double stockLength = 0;
                var beamPosition = stock.Position;
                switch (beamPosition)
                {
                    case 1:
                        entryDate = stock.SizingEntryDate;
                        stockLength = stock.SizingLengthStock;
                        break;
                    case 2:
                        entryDate = stock.ReachingEntryDate;
                        stockLength = stock.ReachingLengthStock;
                        break;
                    case 3:
                        entryDate = stock.LoomEntryDate;
                        stockLength = stock.LoomLengthStock;
                        break;
                    default:
                        entryDate = stock.EmptyEntryDate;
                        break;
                }

                //Get Order Number
                await Task.Yield();
                var orderDocument =
                    _orderDocumentRepository
                        .Find(order => order.Identity.Equals(stock.OrderDocumentId.Value))
                        .FirstOrDefault();
                var orderNumber = orderDocument.OrderNumber ?? "Not Found Order Number";

                //Get Construction Number based on Order Used
                await Task.Yield();
                var constructionId = orderDocument.ConstructionDocumentId;
                var constructionNumber =
                    _fabricConstructionRepository
                        .Find(construction => construction.Identity.Equals(constructionId.Value))
                        .FirstOrDefault()
                        .ConstructionNumber ?? "Not Found Construction Number";

                var beamStockMonitoring = new BeamStockMonitoringDto(stockId, beamNumber, entryDate, orderNumber, constructionNumber, stockLength, beamPosition);

                result.Add(beamStockMonitoring);
            }
            return result;
        }

        public Task<BeamStockMonitoringDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
