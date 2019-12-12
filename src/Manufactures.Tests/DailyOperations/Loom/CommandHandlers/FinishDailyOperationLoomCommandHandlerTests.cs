using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Loom.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.CommandHandlers
{
    public class FinishDailyOperationLoomCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationLoomRepository>
            mockLoomOperationRepo;

        public FinishDailyOperationLoomCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockLoomOperationRepo =
                this.mockRepository.Create<IDailyOperationLoomRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomRepository>())
                .Returns(mockLoomOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FinishDailyOperationLoomCommandHandler CreateFinishDailyOperationLoomCommandHandler()
        {
            return new FinishDailyOperationLoomCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_LoomFinishDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Finish Command Handler Object
            var finishDailyOperationLoomCommandHandler = this.CreateFinishDailyOperationLoomCommandHandler();

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
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            var machineStatus = MachineStatus.ONRESUME;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            FinishDailyOperationLoomCommand request = new FinishDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                FinishBeamProductBeamId = loomBeamProduct.BeamDocumentId,
                FinishBeamNumber = loomBeamHistory.BeamNumber,
                FinishMachineNumber = loomBeamHistory.MachineNumber,
                FinishDateMachine = DateTimeOffset.UtcNow,
                FinishTimeMachine = TimeSpan.Parse("07:00"),
                FinishShiftDocumentId = new ShiftId(Guid.NewGuid()),
                FinishOperatorDocumentId = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await finishDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- FinishDate: Finish date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_LoomFinishTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Finish Command Handler Object
            var finishDailyOperationLoomCommandHandler = this.CreateFinishDailyOperationLoomCommandHandler();

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
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddMinutes(1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            var machineStatus = MachineStatus.ONRESUME;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            FinishDailyOperationLoomCommand request = new FinishDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                FinishBeamProductBeamId = loomBeamProduct.BeamDocumentId,
                FinishBeamNumber = loomBeamHistory.BeamNumber,
                FinishMachineNumber = loomBeamHistory.MachineNumber,
                FinishDateMachine = DateTimeOffset.UtcNow,
                FinishTimeMachine = TimeSpan.Parse("07:00"),
                FinishShiftDocumentId = new ShiftId(Guid.NewGuid()),
                FinishOperatorDocumentId = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await finishDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- FinishTime: Finish time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusStart_DataUpdated()
        {
            // Arrange
            // Set Finish Command Handler Object
            var finishDailyOperationLoomCommandHandler = this.CreateFinishDailyOperationLoomCommandHandler();

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
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);

            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            FinishDailyOperationLoomCommand request = new FinishDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                FinishBeamProductBeamId = loomBeamProduct.BeamDocumentId,
                FinishBeamNumber = loomBeamHistory.BeamNumber,
                FinishMachineNumber = loomBeamHistory.MachineNumber,
                FinishDateMachine = DateTimeOffset.UtcNow,
                FinishTimeMachine = TimeSpan.Parse("07:00"),
                FinishShiftDocumentId = new ShiftId(Guid.NewGuid()),
                FinishOperatorDocumentId = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await finishDailyOperationLoomCommandHandler.Handle(
            request,
            cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusResume_DataUpdated()
        {
            // Arrange
            // Set Finish Command Handler Object
            var finishDailyOperationLoomCommandHandler = this.CreateFinishDailyOperationLoomCommandHandler();

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
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            var machineStatus = MachineStatus.ONRESUME;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);

            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            FinishDailyOperationLoomCommand request = new FinishDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                FinishBeamProductBeamId = loomBeamProduct.BeamDocumentId,
                FinishBeamNumber = loomBeamHistory.BeamNumber,
                FinishMachineNumber = loomBeamHistory.MachineNumber,
                FinishDateMachine = DateTimeOffset.UtcNow,
                FinishTimeMachine = TimeSpan.Parse("07:00"),
                FinishShiftDocumentId = new ShiftId(Guid.NewGuid()),
                FinishOperatorDocumentId = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await finishDailyOperationLoomCommandHandler.Handle(
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
            // Set Finish Command Handler Object
            var finishDailyOperationLoomCommandHandler = this.CreateFinishDailyOperationLoomCommandHandler();

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
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
                                                                                              beamOrigin,
                                                                                              beamDocumentId,
                                                                                              combNumber,
                                                                                              machineDocumentId,
                                                                                              latestDateTimeBeamProduct,
                                                                                              loomProcess,
                                                                                              beamProductStatus);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

            //Mocking Loom History Object
            var historyId = Guid.NewGuid();
            var beamNumber = "S11";
            var machineNumber = "111";
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftDocumentId = new ShiftId(Guid.NewGuid());
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "";
            var information = "-";
            var machineStatus = MachineStatus.ONSTOP;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            FinishDailyOperationLoomCommand request = new FinishDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                FinishBeamProductBeamId = loomBeamProduct.BeamDocumentId,
                FinishBeamNumber = loomBeamHistory.BeamNumber,
                FinishMachineNumber = loomBeamHistory.MachineNumber,
                FinishDateMachine = DateTimeOffset.UtcNow,
                FinishTimeMachine = TimeSpan.Parse("07:00"),
                FinishShiftDocumentId = new ShiftId(Guid.NewGuid()),
                FinishOperatorDocumentId = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await finishDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't finish, latest machine status must ONSTART or ONRESUME", messageException.Message);
            }
        }
    }
}
