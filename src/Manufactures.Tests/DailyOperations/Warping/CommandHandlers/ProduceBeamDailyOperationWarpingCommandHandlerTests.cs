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
    public class ProduceBeamDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;
        private readonly Mock<IDailyOperationWarpingHistoryRepository>
            mockDailyOperationWarpingHistoryRepo;
        private readonly Mock<IDailyOperationWarpingBeamProductRepository>
            mockDailyOperationWarpingBeamProductRepo;

        public ProduceBeamDailyOperationWarpingCommandHandlerTests()
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
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY,
                                                                         currentWarpingDocument.Identity);
            currentWarpingHistory.SetWarpingBeamId(new BeamId(Guid.NewGuid()));
            currentWarpingHistory.SetOperatorDocumentId(new OperatorId(Guid.NewGuid()));
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          new UomId(195),
                                                                          "Meter",
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP, 
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

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
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsDate: Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya", messageException.Message);
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
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY,
                                                                         currentWarpingDocument.Identity);
            currentWarpingHistory.SetWarpingBeamId(new BeamId(Guid.NewGuid()));
            currentWarpingHistory.SetOperatorDocumentId(new OperatorId(Guid.NewGuid()));
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          new UomId(195),
                                                                          "Meter",
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP, 
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

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
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamsTime: Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya", messageException.Message);
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
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONSTART,
                                                                         currentWarpingDocument.Identity);
            currentWarpingHistory.SetWarpingBeamId(new BeamId(Guid.NewGuid()));
            currentWarpingHistory.SetOperatorDocumentId(new OperatorId(Guid.NewGuid()));
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          new UomId(195),
                                                                          "Meter",
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP, 
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

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
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
            this.mockStorage.Setup(x => x.Save());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

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
                                                                         DateTimeOffset.UtcNow,
                                                                         MachineStatus.ONENTRY, 
                                                                         currentWarpingDocument.Identity);
            currentWarpingHistory.SetWarpingBeamId(new BeamId(Guid.NewGuid()));
            currentWarpingHistory.SetOperatorDocumentId(new OperatorId(Guid.NewGuid()));
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                          new BeamId(Guid.NewGuid()),
                                                                          new UomId(195),
                                                                          "Meter",
                                                                          DateTimeOffset.UtcNow,
                                                                          BeamStatus.ROLLEDUP, 
                                                                          currentWarpingDocument.Identity);
            currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

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
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Tidak Dapat Produksi Beam, Status Mesin Harus ONSTART", messageException.Message);
            }
        }
    }
}
