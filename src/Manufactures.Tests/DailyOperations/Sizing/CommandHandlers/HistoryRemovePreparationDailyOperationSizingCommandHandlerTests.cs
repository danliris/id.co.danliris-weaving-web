using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
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
using System.Linq.Expressions;
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
        private readonly Mock<IDailyOperationSizingHistoryRepository>
            mockSizingHistoryRepo;

        public HistoryRemovePreparationDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
                .Returns(mockSizingOperationRepo.Object);

            this.mockSizingHistoryRepo =
                this.mockRepository.Create<IDailyOperationSizingHistoryRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingHistoryRepository>())
                .Returns(mockSizingHistoryRepo.Object);
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
            var existingBeamDocument =
                new BeamDocument(Guid.NewGuid(),
                                 "S123",
                                 "Sizing",
                                 23);

            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONENTRY,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var request = new HistoryRemovePreparationDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                HistoryId = Guid.NewGuid()
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
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
                Assert.Equal("Validation failed: \r\n -- SizingDocument: Tidak ada Id History yang Cocok dengan " + request.HistoryId, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidationPassed_DeletedStatusOnDatabaseTrue()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemovePreparationDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Preparation State
            var existingBeamDocument =
                new BeamDocument(Guid.NewGuid(),
                                 "S123",
                                 "Sizing",
                                 23);

            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONENTRY,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var request = new HistoryRemovePreparationDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                HistoryId = sizingHistory.Identity
            };

            //Setup Mock Object for Sizing Repo
            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
