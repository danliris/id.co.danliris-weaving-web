using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
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
        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public UpdateStartDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = mockRepository.Create<IStorage>();

            this.mockDailyOperationSizingRepo = mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockDailyOperationSizingRepo.Object);

            mockBeamRepo = mockRepository.Create<IBeamRepository>();
            mockStorage.Setup(x => x.GetRepository<IBeamRepository>())
                 .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private UpdateStartDailyOperationSizingCommandHandler CreateUpdateStartDailyOperationSizingCommandHandler()
        {
            return new UpdateStartDailyOperationSizingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Update Start on Daily Operation Sizing
         * **/
        [Fact]
        public async Task Handle_SizingStartDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationSizingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "");
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                MachineStatus.ONENTRY,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            //mockDailyOperationSizingRepo
            //    .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
            //    .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow.AddDays(-1),
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            //request.SetId(sizingDocumentTestId);

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
                Assert.Equal("Validation failed: \r\n -- StartDate: Start date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingStartTimeLessThanLatestTime_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "");
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                MachineStatus.ONENTRY,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            //mockDailyOperationSizingRepo
            //    .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
            //    .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow.AddHours(-1),
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            //request.SetId(sizingDocumentTestId);

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
                Assert.Equal("Validation failed: \r\n -- StartTime: Start time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusEntry_DataUpdated()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "");
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONENTRY,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            request.SetId(sizingDocumentTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            //Instantiate Command Handler
            var result = await unitUnderTest.Handle(request, cancellationToken);

            //---ASSERT---//
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusCompleteBeamStatusRolledUp_DataUpdated()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ROLLEDUP);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONCOMPLETE,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });
            this.mockStorage.Setup(x => x.Save());

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            request.SetId(sizingDocumentTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            //Instantiate Command Handler
            var result = await unitUnderTest.Handle(request, cancellationToken);

            //---ASSERT---//
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusCompleteBeamStatusNotRolledUp_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ROLLEDUP);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONCOMPLETE,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            //request.SetId(sizingDocumentTestId);

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
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Can't start, latest beam status must ROLLED-UP", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusNotEntryOrNotCompleted_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 630;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "");
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONSTART,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            //mockDailyOperationSizingRepo
            //    .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
            //    .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            //request.SetId(sizingDocumentTestId);

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't start, latest machine status must ONENTRY or ONCOMPLETE", messageException.Message);
            }
        }
    }
}
