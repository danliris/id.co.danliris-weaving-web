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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.CommandHandlers
{
    public class UpdatePauseDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;

        public UpdatePauseDailyOperationWarpingCommandHandlerTests()
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

        private UpdatePauseDailyOperationWarpingCommandHandler CreateUpdatePauseDailyOperationWarpingCommandHandler()
        {
            return new UpdatePauseDailyOperationWarpingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONFINISH,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow;
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
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
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(-1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
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
                Assert.Equal("Validation failed: \r\n -- PauseDate: Pause date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
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
                Assert.Equal("Validation failed: \r\n -- PauseTime: Pause time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnStartOrOnResume_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTOP,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't stop, latest status is not on START or on RESUME", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartAllCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartOnlyLooseThreadsCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 0;
            var coneDeficient = 0;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartOnlyBrokenThreadsCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 0;
            var rightLooseCreel = 0;
            var leftLooseCreel = 0;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnResumeAllCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnResumeOnlyLooseThreadsCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 0;
            var coneDeficient = 0;
            var looseThreadsAmount = 40;
            var rightLooseCreel = 20;
            var leftLooseCreel = 20;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnResumeOnlyBrokenThreadsCauseFilledUp_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 1;
            var coneDeficient = 100;
            var looseThreadsAmount = 0;
            var rightLooseCreel = 0;
            var leftLooseCreel = 0;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
               };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingReadModel>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartThereIsNoCauseFilledUp_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdatePauseDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTART,
                                                                         "TS122");
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var pauseDate = DateTimeOffset.UtcNow.AddDays(1);
            var pauseTime = new TimeSpan(7);
            var pauseShift = new ShiftId(Guid.NewGuid());
            var pauseOperator = new OperatorId(Guid.NewGuid());
            var information = "Test 1";
            var brokenThreadsCause = 0;
            var coneDeficient = 0;
            var looseThreadsAmount = 0;
            var rightLooseCreel = 0;
            var leftLooseCreel = 0;

            //Create Update Start Object
            UpdatePauseDailyOperationWarpingCommand request =
               new UpdatePauseDailyOperationWarpingCommand
               {
                   Id = warpingDocumentTestId,
                   PauseDate = pauseDate,
                   PauseTime = pauseTime,
                   PauseShift = pauseShift,
                   PauseOperator = pauseOperator,
                   Information = information,
                   BrokenThreadsCause = brokenThreadsCause,
                   ConeDeficient = coneDeficient,
                   LooseThreadsAmount = looseThreadsAmount,
                   RightLooseCreel = rightLooseCreel,
                   LeftLooseCreel = leftLooseCreel
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
                Assert.Equal("Validation failed: \r\n -- BrokenThreadsCause: Penyebab Putus Benang harus Diisi\r\n -- LooseThreadsAmount: Jumlah Benang Lolos harus Diisi", messageException.Message);
            }
        }
    }
}
