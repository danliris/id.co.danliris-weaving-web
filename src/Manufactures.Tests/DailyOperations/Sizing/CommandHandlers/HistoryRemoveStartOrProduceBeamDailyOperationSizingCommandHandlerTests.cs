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
    public class HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockSizingOperationRepo;

        public HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockSizingOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler()
        {
            return new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_HistoryStatusStartHistoryIdMatchBeamProductMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONSTART;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = existingSizingHistory.Identity,
                BeamProductId = existingSizingBeamProduct.Identity,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_HistoryStatusStartHistoryIdNotMatchBeamProductMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONSTART;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            var newSizingHistoryId = Guid.NewGuid();
            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = newSizingHistoryId,
                BeamProductId = existingSizingBeamProduct.Identity,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });

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
        public async Task Handle_HistoryStatusStartHistoryIdMatchBeamProductNotMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONSTART;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            var newSizingBeamProductId = Guid.NewGuid();
            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = existingSizingHistory.Identity,
                BeamProductId = newSizingBeamProductId,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- SizingHistory: Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_HistoryStatusCompletedHistoryIdMatchBeamProductMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONCOMPLETE;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 320;
            var weightBruto = 240;
            var weightTheoritical = 256;
            var pisMeter = 3;
            var spu = 44;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = existingSizingHistory.Identity,
                BeamProductId = existingSizingBeamProduct.Identity,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            existingSizingBeamProduct.CounterFinish.Should().Equals(0);
            existingSizingBeamProduct.WeightNetto.Should().Equals(0);
            existingSizingBeamProduct.WeightBruto.Should().Equals(0);
            existingSizingBeamProduct.WeightTheoritical.Should().Equals(0);
            existingSizingBeamProduct.PISMeter.Should().Equals(0);
            existingSizingBeamProduct.SPU.Should().Equals(0);
            existingSizingBeamProduct.BeamStatus.Should().Equals(BeamStatus.ONPROCESS);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_HistoryStatusCompletedHistoryIdNotMatchBeamProductMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONCOMPLETE;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            var newSizingHistoryId = Guid.NewGuid();
            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = newSizingHistoryId,
                BeamProductId = existingSizingBeamProduct.Identity,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });

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
        public async Task Handle_HistoryStatusCompletedHistoryIdMatchBeamProductNotMatch_DataUpdated()
        {
            // Arrange
            var unitUnderTest = this.CreateHistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Object for Remove Preparation Object (Commands)
            //Assign Property to DailyOperationSizingDocument in Start or Completed State
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
            var machineStatus = MachineStatus.ONCOMPLETE;
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

            var sizingBeamProduct = Guid.NewGuid();
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow;
            var counterStart = 0;
            var counterFinish = 1200;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ONPROCESS;
            var existingSizingBeamProduct =
                new DailyOperationSizingBeamProduct(sizingBeamProduct,
                                                    sizingBeamId,
                                                    latestDateTimeBeamProduct,
                                                    counterStart,
                                                    counterFinish,
                                                    weightNetto,
                                                    weightBruto,
                                                    weightTheoritical,
                                                    pisMeter,
                                                    spu,
                                                    beamStatus);
            existingSizingDocument.AddDailyOperationSizingBeamProduct(existingSizingBeamProduct);

            var newSizingBeamProductId = Guid.NewGuid();
            HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request = new HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                HistoryId = existingSizingHistory.Identity,
                BeamProductId = newSizingBeamProductId,
                HistoryStatus = existingSizingHistory.MachineStatus
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- SizingHistory: Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId, messageException.Message);
            }
        }
    }
}
