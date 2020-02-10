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
    public class UpdateResumeDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;
        //private readonly Mock<IBeamRepository>
        //    mockBeamRepo;

        public UpdateResumeDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
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
                                                                "-",
                                                                1,
                                                                1,
                                                                "S123",
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

            //Instantiate Object for New Update Resume Object (Commands)
            var request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                ResumeDate = DateTimeOffset.UtcNow,
                ResumeTime = TimeSpan.Parse("01:00"),
                ResumeShift = new ShiftId(Guid.NewGuid()),
                ResumeOperator = new OperatorId(Guid.NewGuid())
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
                                                                "-",
                                                                1,
                                                                1,
                                                                "S123",
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(-1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            var request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                ResumeDate = DateTimeOffset.UtcNow.AddDays(-1),
                ResumeTime = TimeSpan.Parse("01:00"),
                ResumeShift = new ShiftId(Guid.NewGuid()),
                ResumeOperator = new OperatorId(Guid.NewGuid())
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
                                                                "-",
                                                                1,
                                                                1,
                                                                "S123",
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow;
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            var request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                ResumeDate = DateTimeOffset.UtcNow,
                ResumeTime = TimeSpan.Parse("01:00"),
                ResumeShift = new ShiftId(Guid.NewGuid()),
                ResumeOperator = new OperatorId(Guid.NewGuid())
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
                                                                "-",
                                                                1,
                                                                1,
                                                                "S123",
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            var request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                ResumeDate = DateTimeOffset.UtcNow.AddDays(1),
                ResumeTime = TimeSpan.Parse("01:00"),
                ResumeShift = new ShiftId(Guid.NewGuid()),
                ResumeOperator = new OperatorId(Guid.NewGuid())
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

        [Fact]
        public async Task Handle_MachineStatusNotOnStop_ThrowError()
        {
            // Arrange
            // Set Update Pause Command Handler Object
            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();

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
                                                                "-",
                                                                1,
                                                                1,
                                                                "S123",
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

            //Instantiate Object for New Update Resume Object (Commands)
            var resumeOperator = new OperatorId(Guid.NewGuid());
            var resumeDate = DateTimeOffset.UtcNow.AddDays(1);
            var resumeTime = TimeSpan.Parse("01:00");
            var resumeShift = new ShiftId(Guid.NewGuid());

            var request = new UpdateResumeDailyOperationSizingCommand
            {
                Id = sizingDocument.Identity,
                ResumeDate = DateTimeOffset.UtcNow.AddDays(1),
                ResumeTime = TimeSpan.Parse("01:00"),
                ResumeShift = new ShiftId(Guid.NewGuid()),
                ResumeOperator = new OperatorId(Guid.NewGuid())
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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Resume. This current Operation status isn't ONSTOP", messageException.Message);
            }
        }
    }
}
