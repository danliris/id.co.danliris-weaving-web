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
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;
        private readonly Mock<IDailyOperationSizingBeamProductRepository>
            mockSizingBeamProductRepo;
        private readonly Mock<IDailyOperationSizingHistoryRepository>
            mockSizingHistoryRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public ProduceBeamDailyOperationSizingCommandHandlerTests()
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
                                                                  OperationStatus.ONFINISH);

            var sizingHistory = new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                new ShiftId(Guid.NewGuid()),
                                                                new OperatorId(Guid.NewGuid()),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONCOMPLETE,
                                                                sizingDocument.Identity);
            sizingDocument.SizingHistories = new List<DailyOperationSizingHistory>() { sizingHistory };

            //var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
            //                                                            new BeamId(Guid.NewGuid()),
            //                                                            0,
            //                                                            BeamStatus.ROLLEDUP,
            //                                                            DateTimeOffset.UtcNow,
            //                                                            sizingDocument.Identity);
            //sizingDocument.SizingBeamProducts = new List<DailyOperationSizingBeamProduct>() { sizingBeamProduct };

            //var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
            //                                                              sizingBeamProduct.SizingBeamId,
            //                                                              120,
            //                                                              32,
            //                                                              sizingDocument.Identity);
            //sizingDocument.BeamsWarping = new List<DailyOperationSizingBeamsWarping>() { sizingBeamsWarping };

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00")
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Tidak Dapat Produksi Beam. Operasi Sudah Selesai", messageException.Message);
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

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00")
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Tidak Dapat Produksi Beam. Status Mesin Complete", messageException.Message);
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

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow.AddDays(-1),
                    ProduceBeamTime = TimeSpan.Parse("01:00")
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamDate: Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya", messageException.Message);
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

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow.AddMinutes(-1),
                    ProduceBeamTime = TimeSpan.Parse("01:00")
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            mockSizingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingHistory>() { sizingHistory });
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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamTime: Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStart_DataUpdated()
        {
            // Arrange
            // Set Produce Beams Command Handler Objectrt
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object//Assign Property to BeamDocument
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

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow.AddDays(1),
                    ProduceBeamTime = TimeSpan.Parse("01:00")
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
            //    .Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
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
        public async Task Handle_MachineStatusNotOnStart_ThrowError()
        {
            // Arrange
            // Set Produce Beams Command Handler Object
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to 
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
                                                                MachineStatus.ONSTOP,
                                                                sizingDocument.Identity);

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ONPROCESS,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new ProduceBeamDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    CounterFinish = 1200,
                    WeightNetto = 1160,
                    WeightBruto = 40,
                    WeightTheoritical = 1128,
                    PISMeter = 40,
                    SPU = 40,
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid()),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamDate = DateTimeOffset.UtcNow.AddDays(1),
                    ProduceBeamTime = TimeSpan.Parse("01:00")
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Tidak Dapat Produksi Beam. Status Mesin Bukan ONSTART atau ONRESUME", messageException.Message);
            }
        }
    }
}
