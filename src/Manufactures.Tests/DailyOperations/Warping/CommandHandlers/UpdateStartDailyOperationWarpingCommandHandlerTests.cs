using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Warping.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.CommandHandlers
{
    public class UpdateStartDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;

        public UpdateStartDailyOperationWarpingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationWarpingRepo = mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingRepository>())
                .Returns(mockDailyOperationWarpingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateStartDailyOperationWarpingCommandHandler CreateUpdateStartDailyOperationWarpingCommandHandler()
        {
            return new UpdateStartDailyOperationWarpingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONFINISH);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONFINISH);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            //var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
            //                                                              new BeamId(Guid.NewGuid()),
            //                                                              DateTimeOffset.UtcNow,
            //                                                              BeamStatus.ROLLEDUP);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow;
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Start. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            //var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
            //                                                              new BeamId(Guid.NewGuid()),
            //                                                              DateTimeOffset.UtcNow,
            //                                                              BeamStatus.ROLLEDUP);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddDays(-1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- StartDate: Start date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            //var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
            //                                                              new BeamId(Guid.NewGuid()),
            //                                                              DateTimeOffset.UtcNow,
            //                                                              BeamStatus.ROLLEDUP);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- StartTime: Start time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnEntry_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            //var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
            //                                                              new BeamId(Guid.NewGuid()),
            //                                                              DateTimeOffset.UtcNow,
            //                                                              BeamStatus.ROLLEDUP);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });
            this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnCompleteDifferentWarpingBeam_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONCOMPLETE);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });
            this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnCompleteSameWarpingBeam_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONCOMPLETE);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var warpingBeamId = new BeamId(Guid.NewGuid());
            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          warpingBeamId,
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Beam yang dipilih telah selesai diproses, harus Input Beam yang beda", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnEntryAndNotOnComplete_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

            //Instantiate Current Object
            //Assign Property to DailyOperationWarpingDocument
            var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                           new OrderId(Guid.NewGuid()),
                                                                           40,
                                                                           1,
                                                                           DateTimeOffset.UtcNow,
                                                                           OperationStatus.ONPROCESS);

            var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                         new ShiftId(Guid.NewGuid()),
                                                                         new OperatorId(Guid.NewGuid()),
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONSTART);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            //var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
            //                                                              new BeamId(Guid.NewGuid()),
            //                                                              DateTimeOffset.UtcNow,
            //                                                              BeamStatus.ROLLEDUP);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't start, latest machine status must ONENTRY or ONCOMPLETE", messageException.Message);
            }
        }
    }
}
