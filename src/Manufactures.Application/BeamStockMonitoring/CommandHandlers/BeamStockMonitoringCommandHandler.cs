using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.BeamStockMonitoring;
using Manufactures.Domain.BeamStockMonitoring.Commands;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.BeamStockMonitoring.CommandHandlers
{
    public class BeamStockMonitoringCommandHandler
        : ICommandHandler<BeamStockMonitoringCommand, BeamStockMonitoringDocument>
    {
        private readonly IStorage _storage;
        private readonly IBeamStockMonitoringRepository
            _beamStockMonitoringRepository;

        public BeamStockMonitoringCommandHandler(IStorage storage)
        {
            _storage = storage;
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
        }

        public async Task<BeamStockMonitoringDocument> Handle(BeamStockMonitoringCommand request, CancellationToken cancellationToken)
        {
            //Reformat Date Time
            var year = request.SizingEntryDate.Year;
            var month = request.SizingEntryDate.Month;
            var day = request.SizingEntryDate.Day;
            var hour = request.SizingEntryTime.Hours;
            var minutes = request.SizingEntryTime.Minutes;
            var seconds = request.SizingEntryTime.Seconds;
            var stockSizingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Instantiate Sizing Stock
            var sizingStock = new BeamStockMonitoringDocument(Guid.NewGuid(),
                                                              request.BeamDocumentId,
                                                              stockSizingDateTime,
                                                              request.OrderDocumentId,
                                                              request.SizingLengthStock,
                                                              1,
                                                              false);

            //Update and Save
            await _beamStockMonitoringRepository.Update(sizingStock);
            _storage.Save();

            //return as object  daily operation
            return sizingStock;
        }
    }
}
