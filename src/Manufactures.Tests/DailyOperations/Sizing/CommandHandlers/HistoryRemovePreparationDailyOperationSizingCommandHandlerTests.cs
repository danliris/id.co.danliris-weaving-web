using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class HistoryRemovePreparationDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;

        public HistoryRemovePreparationDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
                .Returns(mockSizingOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private HistoryRemovePreparationDailyOperationSizingCommandHandler CreateHistoryRemovePreparationDailyOperationSizingCommandHandler()
        {
            return new HistoryRemovePreparationDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_NoSizingHistoryIdFound_ThrowError()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemovePreparationDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Preparation State
            var sizingId = Guid.NewGuid();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());

            List<BeamId> beamsWarping = new List<BeamId>();
            var warpingBeamId = new BeamId(Guid.NewGuid());
            beamsWarping.Add(warpingBeamId);

            var emptyWeight = 13;
            var yarnStrands = 400;
            var recipeCode = "PCA 133R";
            var neReal = 40;
            var machineSpeed = 0;
            var texSQ = 0;
            var visco = 0;
            var datetimeOperation = DateTimeOffset.UtcNow;
            var operationStatus = OperationStatus.ONPROCESS;
            var existingSizingDocument =
                new DailyOperationSizingDocument(sizingId,
                                                 machineDocumentId,
                                                 orderDocumentId,
                                                 beamsWarping,
                                                 emptyWeight,
                                                 yarnStrands,
                                                 recipeCode,
                                                 neReal, machineSpeed,
                                                 texSQ,
                                                 visco,
                                                 datetimeOperation,
                                                 operationStatus);

            var sizingHistoryId = Guid.NewGuid();
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var machineStatus = MachineStatus.ONENTRY;
            var information = "-";
            var brokenBeam = 0;
            var machineTroubled = 0;
            var sizingBeamNumber = "";
            var existingSizingHistory =
                new DailyOperationSizingHistory(sizingHistoryId,
                                                shiftDocumentId,
                                                operatorDocumentId,
                                                dateTimeMachine,
                                                machineStatus,
                                                information,
                                                brokenBeam,
                                                machineTroubled,
                                                sizingBeamNumber);
            existingSizingDocument.AddDailyOperationSizingHistory(existingSizingHistory);

            var newSizingHistoryId = Guid.NewGuid();
            HistoryRemovePreparationDailyOperationSizingCommand request = new HistoryRemovePreparationDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = newSizingHistoryId
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- SizingHistory: Tidak ada Id History yang Cocok dengan " + request.HistoryId, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidationPassed_DeletedStatusOnDatabaseTrue()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemovePreparationDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Preparation State
            var sizingId = Guid.NewGuid();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());

            List<BeamId> beamsWarping = new List<BeamId>();
            var warpingBeamId = new BeamId(Guid.NewGuid());
            beamsWarping.Add(warpingBeamId);

            var emptyWeight = 13;
            var yarnStrands = 400;
            var recipeCode = "PCA 133R";
            var neReal = 40;
            var machineSpeed = 0;
            var texSQ = 0;
            var visco = 0;
            var datetimeOperation = DateTimeOffset.UtcNow;
            var operationStatus = OperationStatus.ONPROCESS;
            var existingSizingDocument =
                new DailyOperationSizingDocument(sizingId,
                                                 machineDocumentId,
                                                 orderDocumentId,
                                                 beamsWarping,
                                                 emptyWeight,
                                                 yarnStrands,
                                                 recipeCode,
                                                 neReal, machineSpeed,
                                                 texSQ,
                                                 visco,
                                                 datetimeOperation,
                                                 operationStatus);

            var sizingHistoryId = Guid.NewGuid();
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var machineStatus = MachineStatus.ONENTRY;
            var information = "-";
            var brokenBeam = 0;
            var machineTroubled = 0;
            var sizingBeamNumber = "";
            var existingSizingHistory =
                new DailyOperationSizingHistory(sizingHistoryId,
                                                shiftDocumentId,
                                                operatorDocumentId,
                                                dateTimeMachine,
                                                machineStatus,
                                                information,
                                                brokenBeam,
                                                machineTroubled,
                                                sizingBeamNumber);
            existingSizingDocument.AddDailyOperationSizingHistory(existingSizingHistory);
            
            HistoryRemovePreparationDailyOperationSizingCommand request = new HistoryRemovePreparationDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = existingSizingHistory.Identity
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
