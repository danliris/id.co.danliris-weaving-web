using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Loom.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Xunit;
using Manufactures.Domain.DailyOperations.Loom.Entities;

namespace Manufactures.Tests.DailyOperations.Loom.CommandHandlers
{
    public class UpdatePauseDailyOperationLoomCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationLoomRepository>
            mockLoomOperationRepo;
        private readonly Mock<IDailyOperationLoomBeamProductRepository>
           mockLoomOperationProductRepo;
        private readonly Mock<IDailyOperationLoomHistoryRepository>
            mockLoomOperationHistoryRepo;

        public UpdatePauseDailyOperationLoomCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockLoomOperationRepo =
                this.mockRepository.Create<IDailyOperationLoomRepository>();
            mockLoomOperationHistoryRepo = mockRepository.Create<IDailyOperationLoomHistoryRepository>();
            mockLoomOperationProductRepo = mockRepository.Create<IDailyOperationLoomBeamProductRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomRepository>())
                .Returns(mockLoomOperationRepo.Object);
            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomHistoryRepository>())
                .Returns(mockLoomOperationHistoryRepo.Object);

            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomBeamProductRepository>())
                .Returns(mockLoomOperationProductRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdatePauseDailyOperationLoomCommandHandler CreateUpdatePauseDailyOperationLoomCommandHandler()
        {
            return new UpdatePauseDailyOperationLoomCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_LoomPauseDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- PauseDate: Pause date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_LoomPauseTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddMinutes(1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddMinutes(1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- PauseTime: Pause time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusStartReprocess_DataUpdated()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "Sizing";
            var information = "Test 1";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });

            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusStartNotReprocess_DataUpdated()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "Test 1";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });

            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusResumeReprocess_DataUpdated()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONRESUME;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "Sizing";
            var information = "Test 1";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });

            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusResumeNotReprocess_DataUpdated()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONRESUME;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "Test 1";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });

            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotStartAndNotResume_ThrowError()
        {
            // Arrange
            // Set Pause Command Handler Object
            var updatePauseDailyOperationLoomCommandHandler = this.CreateUpdatePauseDailyOperationLoomCommandHandler();

            //Mocking Loom Document Object
            var loomDocumentId = Guid.NewGuid();
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

            //Mocking Loom Beam Product Object
            var beamProductId = Guid.NewGuid();
            var beamOrigin = "Reaching";
            var beamDocumentId = new BeamId(Guid.NewGuid());
            var combNumber = 44;
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
            var loomProcess = "Normal";
            var beamProductStatus = BeamStatus.ONPROCESS;
            DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTOP;

            DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            //Instantiate Value to Command Object
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "Test 1";
            UpdatePauseDailyOperationLoomCommand request = new UpdatePauseDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                PauseBeamProductBeamId = loomBeamProduct.BeamDocumentId.Value,
                PauseBeamNumber = loomBeamHistory.BeamNumber,
                PauseMachineNumber = loomBeamHistory.MachineNumber,
                WarpBrokenThreads = warpBrokenThreads,
                WeftBrokenThreads = weftBrokenThreads,
                LenoBrokenThreads = lenoBrokenThreads,
                ReprocessTo = reprocessTo,
                Information = information,
                PauseDateMachine = DateTimeOffset.UtcNow,
                PauseTimeMachine = TimeSpan.Parse("07:00"),
                PauseShiftDocumentId = new ShiftId(Guid.NewGuid()),
                PauseOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updatePauseDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't pause, latest machine status must ONSTART or ONRESUME", messageException.Message);
            }
        }
    }
}
