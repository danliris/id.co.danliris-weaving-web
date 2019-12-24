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
    public class ProduceBeamDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;

        public ProduceBeamDailyOperationWarpingCommandHandlerTests()
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

        private ProduceBeamDailyOperationWarpingCommandHandler CreateProduceBeamDailyOperationWarpingCommandHandler()
        {
            return new ProduceBeamDailyOperationWarpingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_MachineStatusOnComplete_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow;
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam. This current Operation status already ONCOMPLETE", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow;
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Produce Beam. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingProduceBeamDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow.AddDays(-1);
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsDate: Produce Beams date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingProduceBeamTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsTime: Produce Beams time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStart_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
        public async Task Handle_MachineStatusOnResume_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONRESUME);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
        public async Task Handle_MachineStatusNotOnStartOrNotOnResume_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONSTOP);
            currentWarpingDocument.AddDailyOperationWarpingHistory(currentWarpingHistory);

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ONPROCESS);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam, latest status is not ONSTART or ONRESUME", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_NoOnProcessBeam_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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
            //                                                              BeamStatus.ONPROCESS);
            //currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow;
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- WarpingBeamProductStatus: Can't Produce Beam. There isn't ONPROCESS Warping Beam on this Operation", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_BeamStatusRolledUp_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationWarpingCommandHandler();

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

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP);
            currentWarpingDocument.AddDailyOperationWarpingBeamProduct(currentBeamProduct);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var produceBeamsDate = DateTimeOffset.UtcNow;
            var produceBeamsTime = new TimeSpan(7);
            var produceBeamsShift = new ShiftId(Guid.NewGuid());
            var produceBeamsOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamLength = 400;

            //Create Update Start Object
            ProduceBeamsDailyOperationWarpingCommand request =
                new ProduceBeamsDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = produceBeamsDate,
                    ProduceBeamsTime = produceBeamsTime,
                    ProduceBeamsShift = produceBeamsShift,
                    ProduceBeamsOperator = produceBeamsOperator,
                    WarpingBeamLengthPerOperator = warpingBeamLength
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
                Assert.Equal("Validation failed: \r\n -- WarpingBeamProductStatus: Can't Produce Beam. There isn't ONPROCESS Warping Beam on this Operation", messageException.Message);
            }
        }
    }
}
