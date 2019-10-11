//using ExtCore.Data.Abstractions;
//using FluentAssertions;
//using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
//using Manufactures.Application.Helpers;
//using Manufactures.Domain.DailyOperations.Sizing;
//using Manufactures.Domain.DailyOperations.Sizing.Commands;
//using Manufactures.Domain.DailyOperations.Sizing.Entities;
//using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
//using Manufactures.Domain.DailyOperations.Sizing.Repositories;
//using Manufactures.Domain.Shared.ValueObjects;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
//{
//    public class UpdateResumeDailyOperationSizingCommandHandlerTests : IDisposable
//    {
//        private readonly MockRepository mockRepository;
//        private readonly Mock<IStorage> mockStorage;
//        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;

//        public UpdateResumeDailyOperationSizingCommandHandlerTests()
//        {
//            this.mockRepository = new MockRepository(MockBehavior.Default);

//            this.mockStorage = mockRepository.Create<IStorage>();

//            this.mockDailyOperationSizingRepo = mockRepository.Create<IDailyOperationSizingRepository>();
//            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
//                .Returns(mockDailyOperationSizingRepo.Object);
//        }

//        public void Dispose()
//        {
//            this.mockRepository.VerifyAll();
//        }

//        private UpdateResumeDailyOperationSizingCommandHandler CreateUpdateResumeDailyOperationSizingCommandHandler()
//        {
//            return new UpdateResumeDailyOperationSizingCommandHandler(
//                this.mockStorage.Object);
//        }

//        /**
//         * Test for Update Resume on Daily Operation Sizing
//         * **/
//        [Fact]
//        public async Task Handle_BeamStatusOnProcess_ThrowError()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONPROCESS;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow.AddDays(-1),
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                "");
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow.AddDays(-1),
//                MachineStatus.ONENTRY,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow,
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            try
//            {
//                // Act
//                var result = await unitUnderTest.Handle(request, cancellationToken);
//            }
//            catch (Exception messageException)
//            {
//                // Assert
//                Assert.Equal("Validation failed: \r\n -- BeamStatus: Can't Resume. There isn't ONPROCESS Sizing Beam on this Operation", messageException.Message);
//            }
//        }

//        [Fact]
//        public async Task Handle_OperationStatusOnFinish_ThrowError()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONFINISH;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow.AddDays(-1),
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                BeamStatus.ONPROCESS);
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow.AddDays(-1),
//                MachineStatus.ONSTART,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow,
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            try
//            {
//                // Act
//                var result = await unitUnderTest.Handle(request, cancellationToken);
//            }
//            catch (Exception messageException)
//            {
//                // Assert
//                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Resume. This operation's status already FINISHED", messageException.Message);
//            }
//        }
//        [Fact]
//        public async Task Handle_SizingResumeDateLessThanLatestDate_ThrowError()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONPROCESS;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow,
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                BeamStatus.ONPROCESS);
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow,
//                MachineStatus.ONSTART,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow.AddDays(-1),
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            try
//            {
//                // Act
//                var result = await unitUnderTest.Handle(request, cancellationToken);
//            }
//            catch (Exception messageException)
//            {
//                // Assert
//                Assert.Equal("Validation failed: \r\n -- ResumeDate: Resume date cannot less than latest date log", messageException.Message);
//            }
//        }

//        [Fact]
//        public async Task Handle_SizingResumeTimeLessThanLatestTime_ThrowError()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONPROCESS;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow,
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                BeamStatus.ONPROCESS);
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow,
//                MachineStatus.ONSTART,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow.AddHours(-1),
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            try
//            {
//                // Act
//                var result = await unitUnderTest.Handle(request, cancellationToken);
//            }
//            catch (Exception messageException)
//            {
//                // Assert
//                Assert.Equal("Validation failed: \r\n -- ResumeTime: Resume time cannot less than or equal latest operation", messageException.Message);
//            }
//        }

//        [Fact]
//        public async Task Handle_MachineStatusOnStop_DataUpdated()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONPROCESS;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow,
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                BeamStatus.ONPROCESS);
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow,
//                MachineStatus.ONSTOP,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
//            mockDailyOperationSizingRepo
//                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
//                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
//            //mockBeamRepo
//            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
//            //    .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });
//            this.mockStorage.Setup(x => x.Save());

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow.AddDays(1),
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };
//            request.SetId(sizingDocumentTestId);

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            //---ACT---//
//            //Instantiate Command Handler
//            var result = await unitUnderTest.Handle(request, cancellationToken);

//            //---ASSERT---//
//            result.Identity.Should().NotBeEmpty();
//            result.Should().NotBeNull();
//        }

//        [Fact]
//        public async Task Handle_MachineStatusNotOnStop_DataUpdated()
//        {
//            // Arrange
//            //Instantiate Properties
//            //Add Existing Data
//            var sizingDocumentTestId = Guid.NewGuid();
//            var unitUnderTest = this.CreateUpdateResumeDailyOperationSizingCommandHandler();
//            var machineDocumentId = new MachineId(Guid.NewGuid());
//            var orderId = new OrderId(Guid.NewGuid());
//            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
//            var emptyWeight = 63;
//            var yarnStrands = 63;
//            var recipeCode = "PCA 133R";
//            var neReal = 2;
//            var machineSpeed = 0;
//            var texSQ = "0";
//            var visco = "0";
//            var operationStatus = OperationStatus.ONPROCESS;

//            //Assign Property to DailyOperationReachingTyingDocument
//            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

//            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
//            var sizingBeamDocument = new DailyOperationSizingBeamProduct(
//                Guid.NewGuid(),
//                DateTimeOffset.UtcNow,
//                new DailyOperationSizingCounterValueObject(0, 0),
//                new DailyOperationSizingWeightValueObject(0, 0, 0),
//                0,
//                0,
//                BeamStatus.ONPROCESS);
//            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

//            //Instantiate and Assign Property to DailyOperationSizingDetail
//            var sizingDetail = new DailyOperationSizingHistory(
//                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
//                new OperatorId(Guid.NewGuid()),
//                DateTimeOffset.UtcNow,
//                MachineStatus.ONSTART,
//                "",
//                new DailyOperationSizingCauseValueObject("0", "0"),
//                "TS122");
//            resultModel.AddDailyOperationSizingDetail(sizingDetail);

//            //Mocking Repository
//            mockDailyOperationSizingRepo
//                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
//                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

//            //Instantiate Incoming Object
//            //Sizing Detail Command (Incoming Object)
//            var sizingDetailCommand = new UpdateResumeDailyOperationSizingDetailCommand
//            {
//                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
//                ShiftId = new ShiftId(Guid.NewGuid()),
//                ResumeDate = DateTimeOffset.UtcNow.AddDays(1),
//                ResumeTime = new TimeSpan(7)
//            };

//            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
//            var sizingDocument = new UpdateResumeDailyOperationSizingCommand
//            {
//                Id = sizingDocumentTestId,
//                Details = sizingDetailCommand
//            };

//            //Update Incoming Object
//            UpdateResumeDailyOperationSizingCommand request = new UpdateResumeDailyOperationSizingCommand
//            {
//                Details = sizingDocument.Details
//            };
//            request.SetId(sizingDocumentTestId);

//            //Set Cancellation Token
//            CancellationToken cancellationToken = CancellationToken.None;

//            try
//            {
//                // Act
//                var result = await unitUnderTest.Handle(request, cancellationToken);
//            }
//            catch (Exception messageException)
//            {
//                // Assert
//                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Resume. This current Operation status isn't ONSTOP", messageException.Message);
//            }
//        }
//    }
//}
