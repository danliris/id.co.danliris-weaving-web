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
    //public class UpdateStartDailyOperationLoomCommandHandlerTests : IDisposable
    //{
    //    private readonly MockRepository mockRepository;
    //    private readonly Mock<IStorage> mockStorage;
    //    private readonly Mock<IDailyOperationLoomRepository>
    //        mockLoomOperationRepo;
    //    private readonly Mock<IDailyOperationLoomBeamProductRepository>
    //        mockLoomOperationProductRepo;
    //    private readonly Mock<IDailyOperationLoomHistoryRepository>
    //        mockLoomOperationHistoryRepo;

    //    public UpdateStartDailyOperationLoomCommandHandlerTests()
    //    {
    //        this.mockRepository = new MockRepository(MockBehavior.Default);
    //        this.mockStorage = this.mockRepository.Create<IStorage>();

    //        this.mockLoomOperationRepo =
    //            this.mockRepository.Create<IDailyOperationLoomRepository>();
    //        mockLoomOperationHistoryRepo = mockRepository.Create<IDailyOperationLoomHistoryRepository>();
    //        mockLoomOperationProductRepo = mockRepository.Create<IDailyOperationLoomBeamProductRepository>();

    //        this.mockStorage
    //            .Setup(x => x.GetRepository<IDailyOperationLoomRepository>())
    //            .Returns(mockLoomOperationRepo.Object);
    //        mockStorage
    //            .Setup(x => x.GetRepository<IDailyOperationLoomHistoryRepository>())
    //            .Returns(mockLoomOperationHistoryRepo.Object);

    //        mockStorage
    //            .Setup(x => x.GetRepository<IDailyOperationLoomBeamProductRepository>())
    //            .Returns(mockLoomOperationProductRepo.Object);
    //    }

    //    public void Dispose()
    //    {
    //        this.mockRepository.VerifyAll();
    //    }

    //    private UpdateStartDailyOperationLoomCommandHandler CreateUpdateStartDailyOperationLoomCommandHandler()
    //    {
    //        return new UpdateStartDailyOperationLoomCommandHandler(
    //            this.mockStorage.Object);
    //    }

    //    [Fact]
    //    public async Task Handle_ValidationPassedMachineStatusEntry_DataUpdated()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.ONPROCESS;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var machineStatus = MachineStatus.ONENTRY;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //             .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //             .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


    //        this.mockStorage.Setup(x => x.Save()).Verifiable();

    //        CancellationToken cancellationToken = CancellationToken.None;

    //        // Act
    //        var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //            request,
    //            cancellationToken);

    //        // Assert
    //        result.Identity.Should().NotBeEmpty();
    //        result.Should().NotBeNull();
    //    }

    //    [Fact]
    //    public async Task Handle_ValidationPassedMachineStatusComplete_DataUpdated()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.COMPLETED;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var warpBrokenThreads = 0;
    //        var weftBrokenThreads = 0;
    //        var lenoBrokenThreads = 0;
    //        var reprocessTo = "-";
    //        var information = "-";
    //        var machineStatus = MachineStatus.ONCOMPLETE;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
    //        loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
    //        loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
    //        loomBeamHistory.SetReprocessTo(reprocessTo);
    //        loomBeamHistory.SetInformation(information);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


    //        this.mockStorage.Setup(x => x.Save()).Verifiable();

    //        CancellationToken cancellationToken = CancellationToken.None;

    //        // Act
    //        var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //            request,
    //            cancellationToken);

    //        // Assert
    //        result.Identity.Should().NotBeEmpty();
    //        result.Should().NotBeNull();
    //    }

    //    [Fact]
    //    public async Task Handle_MachineStatusNotEntryOrNotComplete_ThrowError()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.ONPROCESS;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var warpBrokenThreads = 1;
    //        var weftBrokenThreads = 1;
    //        var lenoBrokenThreads = 1;
    //        var reprocessTo = "-";
    //        var information = "-";
    //        var machineStatus = MachineStatus.ONSTOP;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
    //        loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
    //        loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
    //        loomBeamHistory.SetReprocessTo(reprocessTo);
    //        loomBeamHistory.SetInformation(information);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //             .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //             .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });



    //        CancellationToken cancellationToken = CancellationToken.None;

    //        try
    //        {
    //            // Act
    //            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //                request,
    //                cancellationToken);
    //        }
    //        catch (Exception messageException)
    //        {
    //            // Assert
    //            Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't start, latest machine status must ONENTRY or ONCOMPLETE", messageException.Message);
    //        }
    //    }

    //    [Fact]
    //    public async Task Handle_BeamStatusEnd_ThrowError()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(-1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.END;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var warpBrokenThreads = 0;
    //        var weftBrokenThreads = 0;
    //        var lenoBrokenThreads = 0;
    //        var reprocessTo = "-";
    //        var information = "-";
    //        var machineStatus = MachineStatus.ONCOMPLETE;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
    //        loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
    //        loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
    //        loomBeamHistory.SetReprocessTo(reprocessTo);
    //        loomBeamHistory.SetInformation(information);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //             .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //             .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


    //        CancellationToken cancellationToken = CancellationToken.None;

    //        try
    //        {
    //            // Act
    //            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //                request,
    //                cancellationToken);
    //        }
    //        catch (Exception messageException)
    //        {
    //            // Assert
    //            Assert.Equal("Validation failed: \r\n -- StartBeamNumber: Status Beam ini Reproses, Tidak Dapat Diproses Kembali", messageException.Message);
    //        }
    //    }

    //    [Fact]
    //    public async Task Handle_LoomStartDateLessThanLatestDate_ThrowError()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddDays(1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.ONPROCESS;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var warpBrokenThreads = 0;
    //        var weftBrokenThreads = 0;
    //        var lenoBrokenThreads = 0;
    //        var reprocessTo = "-";
    //        var information = "-";
    //        var machineStatus = MachineStatus.ONSTART;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
    //        loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
    //        loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
    //        loomBeamHistory.SetReprocessTo(reprocessTo);
    //        loomBeamHistory.SetInformation(information);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //             .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //             .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });



    //        CancellationToken cancellationToken = CancellationToken.None;

    //        try
    //        {
    //            // Act
    //            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //                request,
    //                cancellationToken);
    //        }
    //        catch (Exception messageException)
    //        {
    //            // Assert
    //            Assert.Equal("Validation failed: \r\n -- StartDate: Start date cannot less than latest date log", messageException.Message);
    //        }
    //    }

    //    [Fact]
    //    public async Task Handle_LoomStartTimeLessThanLatestTime_ThrowError()
    //    {
    //        // Arrange
    //        // Set Start Command Handler Object
    //        var updateStartDailyOperationLoomCommandHandler = this.CreateUpdateStartDailyOperationLoomCommandHandler();

    //        //Mocking Loom Document Object
    //        var loomDocumentId = Guid.NewGuid();
    //        var orderDocumentId = new OrderId(Guid.NewGuid());
    //        var operationStatus = OperationStatus.ONPROCESS;
    //        DailyOperationLoomDocument loomDocument = new DailyOperationLoomDocument(loomDocumentId, orderDocumentId, operationStatus);

    //        //Mocking Loom Beam Product Object
    //        var beamProductId = Guid.NewGuid();
    //        var beamOrigin = "Reaching";
    //        var beamDocumentId = new BeamId(Guid.NewGuid());
    //        var combNumber = 44;
    //        var machineDocumentId = new MachineId(Guid.NewGuid());
    //        var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddMinutes(1);
    //        var loomProcess = "Normal";
    //        var beamProductStatus = BeamStatus.ONPROCESS;
    //        DailyOperationLoomBeamUsed loomBeamProduct = new DailyOperationLoomBeamUsed(beamProductId,
    //                                                                                          beamOrigin,
    //                                                                                          beamDocumentId,
    //                                                                                          combNumber,
    //                                                                                          machineDocumentId,
    //                                                                                          latestDateTimeBeamProduct,
    //                                                                                          loomProcess,
    //                                                                                          beamProductStatus,
    //                                                                                          loomDocument.Identity);
    //        //loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);

    //        //Mocking Loom History Object
    //        var historyId = Guid.NewGuid();
    //        var beamNumber = "S11";
    //        var machineNumber = "111";
    //        var operatorDocumentId = new OperatorId(Guid.NewGuid());
    //        var dateTimeMachine = DateTimeOffset.UtcNow.AddMinutes(1);
    //        var shiftDocumentId = new ShiftId(Guid.NewGuid());
    //        var warpBrokenThreads = 0;
    //        var weftBrokenThreads = 0;
    //        var lenoBrokenThreads = 0;
    //        var reprocessTo = "-";
    //        var information = "-";
    //        var machineStatus = MachineStatus.ONSTART;

    //        DailyOperationLoomHistory loomBeamHistory = new DailyOperationLoomHistory(historyId,
    //                                                                                          beamNumber,
    //                                                                                          machineNumber,
    //                                                                                          operatorDocumentId,
    //                                                                                          dateTimeMachine,
    //                                                                                          shiftDocumentId,
    //                                                                                          machineStatus,
    //                                                                                          loomDocument.Identity);
    //        loomBeamHistory.SetWarpBrokenThreads(warpBrokenThreads);
    //        loomBeamHistory.SetWeftBrokenThreads(weftBrokenThreads);
    //        loomBeamHistory.SetLenoBrokenThreads(lenoBrokenThreads);
    //        loomBeamHistory.SetReprocessTo(reprocessTo);
    //        loomBeamHistory.SetInformation(information);
    //        //loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

    //        UpdateStartDailyOperationLoomCommand request = new UpdateStartDailyOperationLoomCommand
    //        {
    //            Id = loomDocument.Identity,
    //            StartBeamDocumentId = loomBeamProduct.Identity,
    //            StartBeamNumber = loomBeamHistory.BeamNumber,
    //            StartMachineNumber = loomBeamHistory.MachineNumber,
    //            StartDateMachine = DateTimeOffset.UtcNow,
    //            StartTimeMachine = TimeSpan.Parse("07:00"),
    //            StartShiftDocumentId = new ShiftId(Guid.NewGuid()),
    //            StartLoomOperatorDocumentId = new OperatorId(Guid.NewGuid()),
    //        };

    //        //Setup Mock Object for Loom Repo
    //        mockLoomOperationRepo
    //             .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomDocumentReadModel, bool>>>()))
    //             .Returns(new List<DailyOperationLoomDocument>() { loomDocument });

    //        mockLoomOperationProductRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomBeamUsedReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomBeamUsed>() { loomBeamProduct });

    //        mockLoomOperationHistoryRepo
    //              .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomHistoryReadModel, bool>>>()))
    //              .Returns(new List<DailyOperationLoomHistory>() { loomBeamHistory });


    //        CancellationToken cancellationToken = CancellationToken.None;

    //        try
    //        {
    //            // Act
    //            var result = await updateStartDailyOperationLoomCommandHandler.Handle(
    //                request,
    //                cancellationToken);
    //        }
    //        catch (Exception messageException)
    //        {
    //            // Assert
    //            Assert.Equal("Validation failed: \r\n -- StartTime: Start time cannot less than or equal latest time log", messageException.Message);
    //        }
    //    }
    //}
}
