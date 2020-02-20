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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.CommandHandlers
{
    public class UpdateStartDailyOperationLoomCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationLoomRepository>
            mockLoomOperationRepo;
        private readonly Mock<IDailyOperationLoomBeamProductRepository>
            mockLoomOperationProductRepo;
        private readonly Mock<IDailyOperationLoomBeamHistoryRepository>
            mockLoomOperationHistoryRepo;

        public UpdateStartDailyOperationLoomCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockLoomOperationRepo =
                this.mockRepository.Create<IDailyOperationLoomRepository>();
            mockLoomOperationHistoryRepo = mockRepository.Create<IDailyOperationLoomBeamHistoryRepository>();
            mockLoomOperationProductRepo = mockRepository.Create<IDailyOperationLoomBeamProductRepository>();

            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomRepository>())
                .Returns(mockLoomOperationRepo.Object);
            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomBeamHistoryRepository>())
                .Returns(mockLoomOperationHistoryRepo.Object);

            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomBeamProductRepository>())
                .Returns(mockLoomOperationProductRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateStartDailyOperationLoomCommandHandler CreateUpdateStartDailyOperationLoomCommandHandler()
        {
            return new UpdateStartDailyOperationLoomCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_ValidationPassedMachineStatusEntry_DataUpdated()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var machineStatus = MachineStatus.ONENTRY;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });


            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ValidationPassedMachineStatusComplete_DataUpdated()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var beamProductStatus = BeamStatus.COMPLETED;
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
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
            var warpBrokenThreads = 0;
            var weftBrokenThreads = 0;
            var lenoBrokenThreads = 0;
            var reprocessTo = "-";
            var information = "-";
            var machineStatus = MachineStatus.ONCOMPLETE;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });


            this.mockStorage.Setup(x => x.Save()).Verifiable();

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotEntryOrNotComplete_ThrowError()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var warpBrokenThreads = 1;
            var weftBrokenThreads = 1;
            var lenoBrokenThreads = 1;
            var reprocessTo = "-";
            var information = "-";
            var machineStatus = MachineStatus.ONSTOP;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });



            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't start, latest machine status must ONENTRY or ONCOMPLETE", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_BeamStatusEnd_ThrowError()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var beamProductStatus = BeamStatus.END;
            DailyOperationLoomBeamProduct loomBeamProduct = new DailyOperationLoomBeamProduct(beamProductId,
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
            var warpBrokenThreads = 0;
            var weftBrokenThreads = 0;
            var lenoBrokenThreads = 0;
            var reprocessTo = "-";
            var information = "-";
            var machineStatus = MachineStatus.ONCOMPLETE;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });


            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- StartBeamNumber: Status Beam ini Reproses, Tidak Dapat Diproses Kembali", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_LoomStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var warpBrokenThreads = 0;
            var weftBrokenThreads = 0;
            var lenoBrokenThreads = 0;
            var reprocessTo = "-";
            var information = "-";
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });



            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- StartDate: Start date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_LoomStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Start Command Handler Object
            var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

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
            var warpBrokenThreads = 0;
            var weftBrokenThreads = 0;
            var lenoBrokenThreads = 0;
            var reprocessTo = "-";
            var information = "-";
            var machineStatus = MachineStatus.ONSTART;

            DailyOperationLoomBeamHistory loomBeamHistory = new DailyOperationLoomBeamHistory(historyId,
                                                                                              beamNumber,
                                                                                              machineNumber,
                                                                                              operatorDocumentId,
                                                                                              dateTimeMachine,
                                                                                              shiftDocumentId,
                                                                                              machineStatus,
                                                                                              loomDocument.Identity);
            loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
            loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
            loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
            loomBeamHistory.SetReprocessTo(reprocessTo);
            loomBeamHistory.SetInformation(information);
            //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
            {
                Id = loomDocument.Identity,
                StartBeamProductId = loomBeamProduct.Identity,
                StartBeamNumber = loomBeamHistory.BeamNumber,
                StartMachineNumber = loomBeamHistory.MachineNumber,
                StartDateMachine = DateTimeOffset.UtcNow,
                StartTimeMachine = TimeSpan.Parse("07:00"),
                StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
                StartOperatorDocumentId = new OperatorId(Guid.NewGuid()),
            };

            //Setup Mock Object for Loom Repo
            mockLoomOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
                 .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

            mockLoomOperationProductRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamProductReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamProduct>() { loomBeamProduct });

            mockLoomOperationHistoryRepo
                  .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamHistoryReadModel, bool>>>()))
                  .Returns(new List<DailyOperationLoomBeamHistory>() { loomBeamHistory });


            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateStartDailyOperationLoomCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- StartTime: Start time cannot less than or equal latest time log", messageException.Message);
            }
        }
    }
}
