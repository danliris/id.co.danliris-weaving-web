using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
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
    public class ProduceBeamDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockSizingOperationRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public ProduceBeamDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockSizingOperationRepo.Object);

            this.mockBeamRepo =
                this.mockRepository.Create<IBeamRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IBeamRepository>())
                .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ProduceBeamDailyOperationSizingCommandHandler CreateProduceBeamDailyOperationSizingCommandHandler()
        {
            return new ProduceBeamDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
            var datetimeOperation = DateTimeOffset.UtcNow;
            var operationStatus = OperationStatus.ONFINISH;
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
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
            var weightNetto = 0;
            var weightBruto = 0;
            var weightTheoritical = 0;
            var pisMeter = 0;
            var spu = 0;
            var beamStatus = BeamStatus.ROLLEDUP;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow;
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Produce Beam. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnComplete_ThrowError()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow;
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam. This current Operation status already ONCOMPLETE", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingProduceBeamsDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var machineStatus = MachineStatus.ONRESUME;
            var information = "-";
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow.AddDays(-1);
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamDate: Produce Beam date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingProduceBeamsTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var machineStatus = MachineStatus.ONRESUME;
            var information = "-";
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamTime: Produce Beam time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStart_DataUpdated()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object//Assign Property to BeamDocument
            var beamId = new BeamId(Guid.NewGuid());
            var beamNumber = "S123";
            var beamType = "Sizing";
            var beamEmptyWeight = 23;
            var existingBeamDocument =
                new BeamDocument(beamId.Value,
                                 beamNumber,
                                 beamType,
                                 beamEmptyWeight);

            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
                .Returns(new List<BeamDocument>() { existingBeamDocument });
            this.mockStorage.Setup(x => x.Save());

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
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object//Assign Property to BeamDocument
            var beamId = new BeamId(Guid.NewGuid());
            var beamNumber = "S123";
            var beamType = "Sizing";
            var beamEmptyWeight = 23;
            var existingBeamDocument =
                new BeamDocument(beamId.Value,
                                 beamNumber,
                                 beamType,
                                 beamEmptyWeight);

            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var machineStatus = MachineStatus.ONRESUME;
            var information = "-";
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
                .Returns(new List<BeamDocument>() { existingBeamDocument });
            this.mockStorage.Setup(x => x.Save());

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
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
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
            var texSQ = "0";
            var visco = "0";
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
            var machineStatus = MachineStatus.ONSTOP;
            var information = "-";
            var brokenBeam = 1;
            var machineTroubled = 1;
            var sizingBeamNumber = "S123";
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
            var counterFinish = 0;
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

            //Instantiate Object for New Update Pause Object (Commands)
            var produceBeamCounterFinish = 1200;
            var produceBeamWeightNetto = 1160;
            var produceBeamWeightBruto = 40;
            var produceBeamWeightTheoritical = 1128;
            var produceBeamPISMeter = 40;
            var produceBeamSPU = 40;
            var produceBeamOperator = new OperatorId(Guid.NewGuid());
            var produceBeamDate = DateTimeOffset.UtcNow.AddDays(1);
            var produceBeamTime = TimeSpan.Parse("01:00");
            var produceBeamShift = new ShiftId(Guid.NewGuid());

            ProduceBeamDailyOperationSizingCommand request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = existingSizingDocument.Identity,
                    CounterFinish = produceBeamCounterFinish,
                    WeightNetto = produceBeamWeightNetto,
                    WeightBruto = produceBeamWeightBruto,
                    WeightTheoritical = produceBeamWeightTheoritical,
                    PISMeter = produceBeamPISMeter,
                    SPU = produceBeamSPU,
                    ProduceBeamOperator = produceBeamOperator,
                    ProduceBeamShift = produceBeamShift,
                    ProduceBeamDate = produceBeamDate,
                    ProduceBeamTime = produceBeamTime
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam, latest status is not ONSTART or ONRESUME", messageException.Message);
            }
        }
    }
}
