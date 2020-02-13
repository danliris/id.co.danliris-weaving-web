using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
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
    public class FinishDoffDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public FinishDoffDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
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

        private FinishDoffDailyOperationSizingCommandHandler CreateFinishDoffDailyOperationSizingCommandHandler()
        {
            return new FinishDoffDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OnProcessBeam_ThrowError()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

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
                                                                MachineStatus.ONRESUME,
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
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Can't Finish. There's ONPROCESS Sizing Beam on this Operation", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnComplete_ThrowError()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

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
                                                                MachineStatus.ONSTART,
                                                                sizingDocument.Identity);

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Finish. This Machine's Operation is not ONCOMPLETE", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

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

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Finish. This Operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingFinishDoffDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

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

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands
            var request =
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- FinishDoffDate: Finish date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingFinishDoffTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

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

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- FinishDoffTime: Finish time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_PassedAllValidation_DataUpdated()
        {
            // Arrange
            // Set Finish Doff Command Handler Object
            var unitUnderTest = this.CreateFinishDoffDailyOperationSizingCommandHandler();

            //Instantiate Existing Object
            //Assign Property to to DailyOperationSizingDocument
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

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                        new BeamId(Guid.NewGuid()),
                                                                        0,
                                                                        BeamStatus.ROLLEDUP,
                                                                        DateTimeOffset.UtcNow,
                                                                        sizingDocument.Identity);

            var sizingBeamsWarping = new DailyOperationSizingBeamsWarping(Guid.NewGuid(),
                                                                          sizingBeamProduct.SizingBeamId,
                                                                          120,
                                                                          32,
                                                                          sizingDocument.Identity);

            //Instantiate Object for New Update Pause Object (Commands)
            var request =
                new FinishDoffDailyOperationSizingCommand
                {
                    Id = sizingDocument.Identity,
                    MachineSpeed = 1200,
                    TexSQ = 1160,
                    Visco = 40,
                    ProduceBeamDate = DateTimeOffset.UtcNow,
                    ProduceBeamTime = TimeSpan.Parse("01:00"),
                    ProduceBeamShift = new ShiftId(Guid.NewGuid()),
                    ProduceBeamOperator = new OperatorId(Guid.NewGuid())
                };

            //Setup Mock Object for Sizing Repo
            mockSizingOperationRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });
            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }
    }
}
