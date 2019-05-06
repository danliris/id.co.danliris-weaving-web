﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class AddDailyOperationSizingCommandHandler : ICommandHandler<AddNewDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public AddDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument>
            Handle(AddNewDailyOperationSizingCommand request,
                   CancellationToken cancellationToken)
        {
            var dailyOperationSizingDocument =
                new DailyOperationSizingDocument(Guid.NewGuid(),
                                                    request.MachineDocumentId,
                                                    request.WeavingUnitId);
            var listOfDailyOperationDetail = new List<DailyOperationSizingDetail>();

            foreach (var operationDetail in request.DailyOperationSizingDetails)
            {
                var newOperation =
                    new DailyOperationSizingDetail(Guid.NewGuid(),
                                                      operationDetail.BeamDocumentId,
                                                      operationDetail.ConstructionDocumentId,
                                                      operationDetail.PIS,
                                                      operationDetail.Visco,
                                                      new DailyOperationSizingProductionTimeValueObject(operationDetail.ProductionTime),
                                                      new DailyOperationSizingBeamTimeValueObject(operationDetail.BeamTime),
                                                      operationDetail.BrokenBeam,
                                                      operationDetail.TroubledMachine,
                                                      operationDetail.Counter,
                                                      operationDetail.ShiftDocumentId,
                                                      operationDetail.Information);

                dailyOperationSizingDocument.AddDailyOperationSizingDetail(newOperation);
            }

            await _dailyOperationSizingDocumentRepository.Update(dailyOperationSizingDocument);

            _storage.Save();

            return dailyOperationSizingDocument;
        }
    }
}