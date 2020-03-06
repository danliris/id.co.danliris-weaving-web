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
    public class UpdateStartDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;
        private readonly Mock<IDailyOperationSizingBeamProductRepository>
            mockSizingBeamProductRepo;
        private readonly Mock<IDailyOperationSizingHistoryRepository>
            mockSizingHistoryRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public UpdateStartDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
                .Returns(mockSizingOperationRepo.Object);

            this.mockSizingBeamProductRepo =
                this.mockRepository.Create<IDailyOperationSizingBeamProductRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingBeamProductRepository>())
                .Returns(mockSizingBeamProductRepo.Object);

            this.mockSizingHistoryRepo =
                this.mockRepository.Create<IDailyOperationSizingHistoryRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingHistoryRepository>())
                .Returns(mockSizingHistoryRepo.Object);

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

        private UpdateStartDailyOperationSizingCommandHandler CreateUpdateStartDailyOperationSizingCommandHandler()
        {
            return new UpdateStartDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_SameBeamAlreadyUsed_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = sizingBeamProduct.SizingBeamId,
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddDays(1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- SizingBeamId: No. Beam Sizing ini Sudah Digunakan dalam Operasi Harian Sizing Ini", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ThereIsOnProcessBeam_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ONPROCESS,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            var startOperator = new OperatorId(Guid.NewGuid());
            var startDate = DateTimeOffset.UtcNow;
            var startTime = TimeSpan.Parse("01:00");
            var startShift = new ShiftId(Guid.NewGuid());
            var newSizingBeamId = new BeamId(Guid.NewGuid());
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow,
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Tidak Dapat Memulai. Ada Beam yang Sedang Diproses", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONFINISH);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONCOMPLETE,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddDays(1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Tidak Dapat Memulai. Status Operasi Sudah Selesai", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow.AddDays(1),
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow.AddDays(1),
                                                                MachineStatus.ONCOMPLETE,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow.AddDays(1),
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow,
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

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
        public async Task Handle_SizingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to DailyOperationSizingDocument
            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONCOMPLETE,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddMinutes(-1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

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

        [Fact]
        public async Task Handle_MachineStatusOnEntry_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to BeamDocument
            var existingBeamDocument =
                new BeamDocument(Guid.NewGuid(),
                                 "S123",
                                 "Sizing",
                                 23);

            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONENTRY,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddDays(1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { existingBeamDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusOnComplete_DataUpdated()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to BeamDocument
            var existingBeamDocument =
                new BeamDocument(Guid.NewGuid(),
                                 "S123",
                                 "Sizing",
                                 23);

            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONCOMPLETE,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                SizingBeamNumber = "T12",
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddDays(1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { existingBeamDocument });
            //this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnCompleteOrNotOnEntry_ThrowError()
        {
            // Arrange
            // Set Update Start Command Handler Object
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to BeamDocument
            var existingBeamDocument =
                new BeamDocument(Guid.NewGuid(),
                                 "S123",
                                 "Sizing",
                                 23);

            var sizingDocument = new DailyOperationSizingDocument(Guid.NewGuid(),
                                                                  new MachineId(Guid.NewGuid()),
                                                                  new OrderId(Guid.NewGuid()),
                                                                  46,
                                                                  400,
                                                                  "PCA 133R",
                                                                  2,
                                                                  DateTimeOffset.UtcNow,
                                                                  2,
                                                                  OperationStatus.ONPROCESS);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ONPROCESS,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);
            sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);
            sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Start Object (Commands)
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                SizingBeamId = new BeamId(Guid.NewGuid()),
                CounterStart = 0,
                StartDate = DateTimeOffset.UtcNow.AddDays(1),
                StartTime = TimeSpan.Parse("01:00"),
                StartShift = new ShiftId(Guid.NewGuid()),
                StartOperator = new OperatorId(Guid.NewGuid())
            };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

            mockSizingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingBeamProduct>() { });

            //mockSizingOperationRepo
            //     .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { existingBeamDocument });
            //this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Tidak Dapat Memulai, Status Mesin Terakhir Harus ONENTRY or ONCOMPLETE", messageException.Message);
            }
        }
    }
}
