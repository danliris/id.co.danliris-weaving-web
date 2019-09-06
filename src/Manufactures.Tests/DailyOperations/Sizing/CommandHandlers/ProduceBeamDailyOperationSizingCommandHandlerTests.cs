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
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
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
    public class ProduceBeamDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public ProduceBeamDailyOperationSizingCommandHandlerTests()
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
            this.mockRepository.VerifyAll();
        }

        private ProduceBeamDailyOperationSizingCommandHandler CreateProduceBeamDailyOperationSizingCommandHandler()
        {
            return new ProduceBeamDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        /**
         * Test for Produce Beam on Daily Operation Sizing
         * **/
        [Fact]
        public async Task Handle_BeamStatusOnProcess_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                DateTimeOffset.UtcNow.AddDays(-1),
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

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- BeamStatus: Can't Produce Beam. There isn't ONPROCESS Sizing Beam on this Operation", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnComplete_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                DateTimeOffset.UtcNow.AddDays(-1),
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ONPROCESS);
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
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel
    });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam. This current Operation status already ONCOMPLETE", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var machineSpeed = 0;
            var texSQ = "0";
            var visco = "0";
            var operationStatus = OperationStatus.ONFINISH;

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, machineSpeed, texSQ, visco, operationStatus);

            //Instantiate and Assign Property to DailyOperationSizingBeamDocument
            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddDays(-1),
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ONPROCESS);
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
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel
    });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Produce Beam. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingProduceBeamDateLessThanLatestDate_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                BeamStatus.ONPROCESS);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                MachineStatus.ONSTART,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel
    });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow.AddDays(-1),
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamDate: Produce Beam date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SizingProduceBeamTimeLessThanLatestDate_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                BeamStatus.ONPROCESS);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                MachineStatus.ONSTART,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel
    });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow.AddHours(-1),
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- ProduceBeamTime: Produce Beam time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStart_DataUpdated()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                DateTimeOffset.UtcNow.AddDays(-1),
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ONPROCESS);
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

            //Assign Property to DailyOperationSizingDocument
            var firstBeamDocument = new BeamDocument(Guid.NewGuid(), "TS122", "Warping", 12);
            var secondBeamDocument = new BeamDocument(Guid.NewGuid(), "TS123", "Warping", 12);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
                .Returns(new List<BeamDocument>() { firstBeamDocument, secondBeamDocument });
            this.mockStorage.Setup(x => x.Save());

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
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
        public async Task Handle_MachineStatusOnResume_DataUpdated()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                DateTimeOffset.UtcNow.AddDays(-1),
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ONPROCESS);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONRESUME,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Assign Property to DailyOperationSizingDocument
            var firstBeamDocument = new BeamDocument(Guid.NewGuid(), "TS122", "Warping", 12);
            var secondBeamDocument = new BeamDocument(Guid.NewGuid(), "TS123", "Warping", 12);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });
            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
                .Returns(new List<BeamDocument>() { firstBeamDocument, secondBeamDocument });
            this.mockStorage.Setup(x => x.Save());

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
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
        public async Task Handle_MachineStatusNotOnStartOrNotOnResume_ThrowError()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateProduceBeamDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
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
                DateTimeOffset.UtcNow.AddDays(-1),
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                BeamStatus.ONPROCESS);
            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);

            //Instantiate and Assign Property to DailyOperationSizingDetail
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                MachineStatus.ONSTOP,
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel
    });

            //Instantiate Incoming Object
            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 240
            };

            var weightBeamDocument = new DailyOperationSizingWeightCommand
            {
                Netto = 75,
                Bruto = 99,
                Theoritical = 67.87
            };

            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new ProduceBeamBeamDocumentDailyOperationSizingCommand
            {
                FinishCounter = counterBeamDocument.Finish,
                Weight = weightBeamDocument,
                PISMeter = 240,
                SPU = 10.51
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new ProduceBeamDetailDailyOperationSizingCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                ProduceBeamDate = DateTimeOffset.UtcNow,
                ProduceBeamTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new ProduceBeamDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingDetails = sizingDetailCommand,
                SizingBeamDocuments = sizingBeamDocumentCommand
            };

            //Update Incoming Object
            ProduceBeamDailyOperationSizingCommand request = new ProduceBeamDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };

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
                Assert.Equal("Validation failed: \r\n -- MachineStatus: Can't Produce Beam, latest status is not ONSTART or ONRESUME", messageException.Message);
            }
        }
    }
}
