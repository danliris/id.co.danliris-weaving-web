using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateResumeDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockSizingOperationRepo;
        //private readonly Mock<IBeamRepository>
        //    mockBeamRepo;

        public UpdateResumeDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockSizingOperationRepo.Object);

            //this.mockBeamRepo =
            //    this.mockRepository.Create<IBeamRepository>();
            //this.mockStorage
            //    .Setup(x => x.GetRepository<IBeamRepository>())
            //    .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateResumeDailyOperationSizingCommandHandler CreateUpdateResumeDailyOperationSizingCommandHandler()
        {
            return new UpdateResumeDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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
            var counterFinish = 1200;
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow;
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                ResumeDate = resumeDate,
                ResumeTime = resumeTime,
                ResumeShift = resumeShift,
                ResumeOperator = resumeOperator
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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Resume. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingResumeDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(-1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                ResumeDate = resumeDate,
                ResumeTime = resumeTime,
                ResumeShift = resumeShift,
                ResumeOperator = resumeOperator
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
                Assert.Equal("Validation failed: \r\n -- ResumeDate: Resume date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingResumeTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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
            var datetimeOperation = DateTimeOffset.UtcNow.AddMinutes(-1);
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
            var dateTimeMachine = DateTimeOffset.UtcNow.AddMinutes(-1);
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
            var latestDateTimeBeamProduct = DateTimeOffset.UtcNow.AddMinutes(-1);
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow;
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                ResumeDate = resumeDate,
                ResumeTime = resumeTime,
                ResumeShift = resumeShift,
                ResumeOperator = resumeOperator
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
                Assert.Equal("Validation failed: \r\n -- ResumeTime: Resume time cannot less than or equal latest operation", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStop_DataUpdated()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                ResumeDate = resumeDate,
                ResumeTime = resumeTime,
                ResumeShift = resumeShift,
                ResumeOperator = resumeOperator
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
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnStop_ThrowError()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = existingSizingDocument.Identity,
                ResumeDate = resumeDate,
                ResumeTime = resumeTime,
                ResumeShift = resumeShift,
                ResumeOperator = resumeOperator
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Resume. This current Operation status isn't ONSTOP", messageException.Message);
            }
        }
    }
}
