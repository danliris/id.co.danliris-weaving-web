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
    public class CompletedDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;
        private readonly Mock<IDailyOperationWarpingHistoryRepository>
            mockDailyOperationWarpingHistoryRepo;
        private readonly Mock<IDailyOperationWarpingBeamProductRepository>
            mockDailyOperationWarpingBeamProductRepo;
        private readonly Mock<IDailyOperationWarpingBrokenCauseRepository>
            mockDailyOperationWarpingBrokenCauseRepo;

        public CompletedDailyOperationWarpingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationWarpingRepo = mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingRepository>())
                .Returns(mockDailyOperationWarpingRepo.Object);

            this.mockDailyOperationWarpingHistoryRepo = mockRepository.Create<IDailyOperationWarpingHistoryRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingHistoryRepository>())
                .Returns(mockDailyOperationWarpingHistoryRepo.Object);

            this.mockDailyOperationWarpingBeamProductRepo = mockRepository.Create<IDailyOperationWarpingBeamProductRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBeamProductRepository>())
                .Returns(mockDailyOperationWarpingBeamProductRepo.Object);

            this.mockDailyOperationWarpingBrokenCauseRepo = mockRepository.Create<IDailyOperationWarpingBrokenCauseRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBrokenCauseRepository>())
                .Returns(mockDailyOperationWarpingBrokenCauseRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private CompletedDailyOperationWarpingCommandHandler CreateFinishDailyOperationWarpingCommandHandler()
        {
            return new CompletedDailyOperationWarpingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_WarpingStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateFinishDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONCOMPLETE, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP,
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

            var brokenCauseList = new List<WarpingBrokenThreadsCausesCommand>();
            WarpingBrokenThreadsCausesCommand brokenCause = new WarpingBrokenThreadsCausesCommand
            {
                WarpingBrokenCauseId = Guid.NewGuid(),
                TotalBroken = 3
            };
            brokenCauseList.Add(brokenCause);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var finishDate = DateTimeOffset.UtcNow.AddDays(-1);
            var finishTime = new TimeSpan(7);
            var finishShift = new ShiftId(Guid.NewGuid());
            var finishOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            CompletedDailyOperationWarpingCommand request =
                new CompletedDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = finishDate,
                    ProduceBeamsTime = finishTime,
                    ProduceBeamsShift = finishShift,
                    ProduceBeamsOperator = finishOperator,
                    WarpingBeamLengthPerOperator = 100,
                    WarpingBeamLengthUomId = 195,
                    Tention = 33,
                    MachineSpeed = 3500,
                    PressRoll = 44,
                    PressRollUom = "PSA",
                    BrokenCauses = brokenCauseList,
                    IsFinishFlag = true
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo.
                Setup(s => s.Query)
                .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsDate: Tanggal Tidak Boleh Melebihi Tanggal Sebelumnya", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateFinishDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONCOMPLETE, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP,
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

            var brokenCauseList = new List<WarpingBrokenThreadsCausesCommand>();
            WarpingBrokenThreadsCausesCommand brokenCause = new WarpingBrokenThreadsCausesCommand
            {
                WarpingBrokenCauseId = Guid.NewGuid(),
                TotalBroken = 3
            };
            brokenCauseList.Add(brokenCause);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var finishDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var finishTime = new TimeSpan(7);
            var finishShift = new ShiftId(Guid.NewGuid());
            var finishOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            CompletedDailyOperationWarpingCommand request =
                new CompletedDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = finishDate,
                    ProduceBeamsTime = finishTime,
                    ProduceBeamsShift = finishShift,
                    ProduceBeamsOperator = finishOperator,
                    WarpingBeamLengthPerOperator = 100,
                    WarpingBeamLengthUomId = 195,
                    Tention = 33,
                    MachineSpeed = 3500,
                    PressRoll = 44,
                    PressRollUom = "PSA",
                    BrokenCauses = brokenCauseList,
                    IsFinishFlag = true
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo.
                Setup(s => s.Query)
                .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsTime: Waktu Tidak Boleh Melebihi Waktu Sebelumnya", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidationPassedIsFinishFlagTrue_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateFinishDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONCOMPLETE, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP,
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

            var brokenCauseList = new List<WarpingBrokenThreadsCausesCommand>();
            WarpingBrokenThreadsCausesCommand brokenCause = new WarpingBrokenThreadsCausesCommand
            {
                WarpingBrokenCauseId = Guid.NewGuid(),
                TotalBroken = 3
            };
            brokenCauseList.Add(brokenCause);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var finishDate = DateTimeOffset.UtcNow.AddDays(1);
            var finishTime = new TimeSpan(7);
            var finishShift = new ShiftId(Guid.NewGuid());
            var finishOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            CompletedDailyOperationWarpingCommand request =
                new CompletedDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = finishDate,
                    ProduceBeamsTime = finishTime,
                    ProduceBeamsShift = finishShift,
                    ProduceBeamsOperator = finishOperator,
                    WarpingBeamLengthPerOperator = 100,
                    WarpingBeamLengthUomId = 195,
                    Tention = 33,
                    MachineSpeed = 3500,
                    PressRoll = 44,
                    PressRollUom = "PSA",
                    BrokenCauses = brokenCauseList,
                    IsFinishFlag = true
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo.
                Setup(s => s.Query)
                .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBrokenCauseRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBrokenCause>())).Returns(Task.CompletedTask);
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
        public async Task Handle_ValidationPassedIsFinishFlagFalse_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateFinishDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONCOMPLETE, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP,
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

            var brokenCauseList = new List<WarpingBrokenThreadsCausesCommand>();
            WarpingBrokenThreadsCausesCommand brokenCause = new WarpingBrokenThreadsCausesCommand
            {
                WarpingBrokenCauseId = Guid.NewGuid(),
                TotalBroken = 3
            };
            brokenCauseList.Add(brokenCause);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var finishDate = DateTimeOffset.UtcNow.AddDays(1);
            var finishTime = new TimeSpan(7);
            var finishShift = new ShiftId(Guid.NewGuid());
            var finishOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            CompletedDailyOperationWarpingCommand request =
                new CompletedDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = finishDate,
                    ProduceBeamsTime = finishTime,
                    ProduceBeamsShift = finishShift,
                    ProduceBeamsOperator = finishOperator,
                    WarpingBeamLengthPerOperator = 100,
                    WarpingBeamLengthUomId = 195,
                    Tention = 33,
                    MachineSpeed = 3500,
                    PressRoll = 44,
                    PressRollUom = "PSA",
                    BrokenCauses = brokenCauseList,
                    IsFinishFlag = false
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo.
                Setup(s => s.Query)
                .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBrokenCauseRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBrokenCause>())).Returns(Task.CompletedTask);
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
        public async Task Handle_MachineStatusOnEntry_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateFinishDailyOperationWarpingCommandHandler();

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
                                                                         MachineStatus.ONCOMPLETE, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP,
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

            var brokenCauseList = new List<WarpingBrokenThreadsCausesCommand>();
            WarpingBrokenThreadsCausesCommand brokenCause = new WarpingBrokenThreadsCausesCommand
            {
                WarpingBrokenCauseId = Guid.NewGuid(),
                TotalBroken = 3
            };
            brokenCauseList.Add(brokenCause);

            //Instantiate Incoming Object
            var warpingDocumentTestId = Guid.NewGuid();
            var finishDate = DateTimeOffset.UtcNow.AddDays(1);
            var finishTime = new TimeSpan(7);
            var finishShift = new ShiftId(Guid.NewGuid());
            var finishOperator = new OperatorId(Guid.NewGuid());

            //Create Update Start Object
            CompletedDailyOperationWarpingCommand request =
                new CompletedDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    ProduceBeamsDate = finishDate,
                    ProduceBeamsTime = finishTime,
                    ProduceBeamsShift = finishShift,
                    ProduceBeamsOperator = finishOperator,
                    WarpingBeamLengthPerOperator = 100,
                    WarpingBeamLengthUomId = 195,
                    Tention = 33,
                    MachineSpeed = 3500,
                    PressRoll = 44,
                    PressRollUom = "PSA",
                    BrokenCauses = brokenCauseList,
                    IsFinishFlag = false
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo.
                Setup(s => s.Query)
                .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Finish, latest status is not ONSTART or ONPROCESSBEAM", messageException.Message);
            }
        }
    }
}
