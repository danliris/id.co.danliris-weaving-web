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
    public class UpdateStartDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;
        private readonly Mock<IDailyOperationWarpingHistoryRepository>
            mockDailyOperationWarpingHistoryRepo;
        private readonly Mock<IDailyOperationWarpingBeamProductRepository>
            mockDailyOperationWarpingBeamProductRepo;

        public UpdateStartDailyOperationWarpingCommandHandlerTests()
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

        private UpdateStartDailyOperationWarpingCommandHandler CreateUpdateStartDailyOperationWarpingCommandHandler()
        {
            return new UpdateStartDailyOperationWarpingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        DateTimeOffset.UtcNow,
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
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
            var startDate = DateTimeOffset.UtcNow;
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            var beamUomId = new UomId(195);
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
            //mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
            //mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
            //this.mockStorage.Setup(x => x.Save());

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Tidak Dapat Memulai. Operasi Sudah Selesai", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
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
            var startDate = DateTimeOffset.UtcNow.AddDays(-1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            var beamUomId = new UomId(195);
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                    //WarpingBeamNumber = warpingBeamNumber
                };
            
            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

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
                Assert.Equal("Validation failed: \r\n -- StartDate: Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_WarpingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
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
            var startDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            var beamUomId = new UomId(195);
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

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
                Assert.Equal("Validation failed: \r\n -- StartTime: Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya", messageException.Message);
            }
        }

        //[Fact]
        //public async Task Handle_MachineStatusOnEntry_DataUpdated()
        //{
        //    // Arrange
        //    // Set Update Start Command Handler Object
        //    var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

        //    //Instantiate Current Object
        //    //Assign Property to DailyOperationWarpingDocument
        //    var currentWarpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
        //                                                                   new OrderId(Guid.NewGuid()),
        //                                                                   40,
        //                                                                   1,
        //                                                                   DateTimeOffset.UtcNow,
        //                                                                   OperationStatus.ONPROCESS);

        //    var currentWarpingHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
        //                                                                 new ShiftId(Guid.NewGuid()),
        //                                                                 new OperatorId(Guid.NewGuid()),
        //                                                                 DateTimeOffset.UtcNow,
        //                                                                 MachineStatus.ONENTRY, currentWarpingDocument.Identity);
        //    currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

        //    var currentBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
        //                                                                  new BeamId(Guid.NewGuid()),
        //                                                                  new UomId(195),
        //                                                                  DateTimeOffset.UtcNow,
        //                                                                  BeamStatus.ROLLEDUP,
        //                                                                  currentWarpingDocument.Identity);
        //    currentWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct };

        //    //Instantiate Incoming Object
        //    var warpingDocumentTestId = Guid.NewGuid();
        //    var startDate = DateTimeOffset.UtcNow.AddDays(1);
        //    var startTime = new TimeSpan(7);
        //    var startShift = new ShiftId(Guid.NewGuid());
        //    var startOperator = new OperatorId(Guid.NewGuid());
        //    var warpingBeamId = new BeamId(Guid.NewGuid());
        //    //var warpingBeamNumber = "TS123";

        //    //Create Update Start Object
        //    UpdateStartDailyOperationWarpingCommand request =
        //        new UpdateStartDailyOperationWarpingCommand
        //        {
        //            Id = warpingDocumentTestId,
        //            StartDate = startDate,
        //            StartTime = startTime,
        //            StartShift = startShift,
        //            StartOperator = startOperator,
        //            WarpingBeamId = warpingBeamId,
        //            //WarpingBeamNumber = warpingBeamNumber
        //        };

        //    //Setup Mock Object for Warping Repo
        //    mockDailyOperationWarpingRepo
        //         .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
        //         .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

        //    mockDailyOperationWarpingHistoryRepo
        //         .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
        //         .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

        //    mockDailyOperationWarpingBeamProductRepo
        //         .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
        //         .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

        //    //mockDailyOperationWarpingHistoryRepo.
        //    //    Setup(s => s.Query)
        //    //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

        //    //mockDailyOperationWarpingBeamProductRepo.
        //    //    Setup(s => s.Query)
        //    //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

        //    mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
        //    mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
        //    mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
        //    this.mockStorage.Setup(x => x.Save());

        //    //mockDailyOperationWarpingRepo
        //    //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
        //    //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });
        //    //this.mockStorage.Setup(x => x.Save());

        //    //Set Cancellation Token
        //    CancellationToken cancellationToken = CancellationToken.None;

        //    // Act
        //    var result = await unitUnderTest.Handle(request, cancellationToken);

        //    // Assert
        //    result.Identity.Should().NotBeEmpty();
        //    result.Should().NotBeNull();
        //}

        [Fact]
        public async Task Handle_MachineStatusOnCompleteDifferentWarpingBeam_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
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
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            var beamUomId = new UomId(195);
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

            //mockDailyOperationWarpingHistoryRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingHistoryReadModel>() { currentWarpingHistory.GetReadModel() }.AsQueryable());

            //mockDailyOperationWarpingBeamProductRepo.
            //    Setup(s => s.Query)
            //    .Returns(new List<DailyOperationWarpingBeamProductReadModel>() { currentBeamProduct.GetReadModel() }.AsQueryable());

            mockDailyOperationWarpingRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingDocument>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingHistoryRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingHistory>())).Returns(Task.CompletedTask);
            mockDailyOperationWarpingBeamProductRepo.Setup(o => o.Update(It.IsAny<DailyOperationWarpingBeamProduct>())).Returns(Task.CompletedTask);
            this.mockStorage.Setup(x => x.Save());

            //mockDailyOperationWarpingRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });
            //this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnCompleteSameWarpingBeam_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
            currentWarpingHistory.SetOperatorDocumentId(new OperatorId(Guid.NewGuid()));
            currentWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { currentWarpingHistory };

            var warpingBeamId = new BeamId(Guid.NewGuid());
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
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var beamUomId = new UomId(195);

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

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
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Beam yang dipilih telah selesai diproses, harus Input Beam yang beda", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnEntryAndNotOnComplete_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationWarpingCommandHandler();

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
                                                                        MachineStatus.ONENTRY, currentWarpingDocument.Identity);
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
            var startDate = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = new TimeSpan(7);
            var startShift = new ShiftId(Guid.NewGuid());
            var startOperator = new OperatorId(Guid.NewGuid());
            var warpingBeamId = new BeamId(Guid.NewGuid());
            var beamUomId = new UomId(195);
            //var warpingBeamNumber = "TS123";

            //Create Update Start Object
            UpdateStartDailyOperationWarpingCommand request =
                new UpdateStartDailyOperationWarpingCommand
                {
                    Id = warpingDocumentTestId,
                    StartDate = startDate,
                    StartTime = startTime,
                    StartShift = startShift,
                    StartOperator = startOperator,
                    WarpingBeamId = warpingBeamId,
                    WarpingBeamLengthUomId = beamUomId
                    //WarpingBeamNumber = warpingBeamNumber
                };

            //Setup Mock Object for Warping Repo
            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { currentWarpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { currentWarpingHistory });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { currentBeamProduct });

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Tidak Dapat Memulai, Status Mesin Harus ONENTRY atau ONCOMPLETE", messageException.Message);
            }
        }
    }
}
