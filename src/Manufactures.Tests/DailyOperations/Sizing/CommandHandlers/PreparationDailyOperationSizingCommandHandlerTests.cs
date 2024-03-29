using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class PreparationDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingDocumentRepository>
            mockSizingOperationRepo;
        private readonly Mock<IDailyOperationSizingHistoryRepository>
            mockSizingHistoryRepo;
        private readonly Mock<IDailyOperationSizingBeamsWarpingRepository>
            mockSizingBeamsWarpingRepo;

        public PreparationDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockSizingOperationRepo =
                this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>())
                .Returns(mockSizingOperationRepo.Object);

            this.mockSizingHistoryRepo =
                this.mockRepository.Create<IDailyOperationSizingHistoryRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingHistoryRepository>())
                .Returns(mockSizingHistoryRepo.Object);

            this.mockSizingBeamsWarpingRepo =
                this.mockRepository.Create<IDailyOperationSizingBeamsWarpingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationSizingBeamsWarpingRepository>())
                .Returns(mockSizingBeamsWarpingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PreparationDailyOperationSizingCommandHandler CreatePreparationDailyOperationSizingCommandHandler()
        {
            return new PreparationDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_NoSameOrderUsed_DataCreated()
        {
            // Arrange
            // Set Preparation Command Handler Object
            var unitUnderTest = this.CreatePreparationDailyOperationSizingCommandHandler();

            //Instantiate Object for New Preparation Object (Commands)
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var recipeCode = "PCA 133R";
            var neReal = 40;
            var preparationOperator = new OperatorId(Guid.NewGuid());
            var preparationDate = DateTimeOffset.UtcNow;
            var preparationTime = TimeSpan.Parse("01:00");
            var preparationShift = new ShiftId(Guid.NewGuid());
            var yarnStrands = 400;
            var emptyWeight = 13;
            List<PreparationDailyOperationSizingBeamsWarpingCommand> beamsWarping = new List<PreparationDailyOperationSizingBeamsWarpingCommand>();
            var beamWarping = new PreparationDailyOperationSizingBeamsWarpingCommand()
            {
                BeamDocumentId = new BeamId(Guid.NewGuid()),
                YarnStrands = 50,
                EmptyWeight = 2
            };
            beamsWarping.Add(beamWarping);

            //Create New Preparation Object (Commands)
            PreparationDailyOperationSizingCommand request =
                new PreparationDailyOperationSizingCommand
                {
                    MachineDocumentId = machineDocumentId,
                    OrderDocumentId = orderDocumentId,
                    RecipeCode = recipeCode,
                    NeReal = neReal,
                    PreparationOperator = preparationOperator,
                    PreparationDate = preparationDate,
                    PreparationTime = preparationTime,
                    PreparationShift = preparationShift,
                    BeamProductResult = 1,
                    YarnStrands = yarnStrands,
                    EmptyWeight = emptyWeight,
                    BeamsWarping = beamsWarping
                };

            //Setup Mock Object for Sizing Repository
            mockSizingOperationRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
                .Returns(new List<DailyOperationSizingDocument>());
            this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            //Check if object not null
            result.Should().NotBeNull();

            //Check if has identity
            result.Identity.Should().NotBeEmpty();

            //check if has history
            //result.SizingHistories.Should().NotBeEmpty();
        }

        //[Fact]
        //public async Task Handle_SameOrderUsed_ThrowError()
        //{
        //    // Arrange
        //    // Set Preparation Command Handler Object
        //    var unitUnderTest = this.CreatePreparationDailyOperationSizingCommandHandler();

        //    //Instantiate Object for New Preparation Object (Commands)
        //    var machineDocumentId = new MachineId(Guid.NewGuid());
        //    var orderDocumentId = new OrderId(Guid.NewGuid());
        //    var recipeCode = "PCA 133R";
        //    var neReal = 40;
        //    var preparationOperator = new OperatorId(Guid.NewGuid());
        //    var preparationDate = DateTimeOffset.UtcNow;
        //    var preparationTime = TimeSpan.Parse("01:00");
        //    var preparationShift = new ShiftId(Guid.NewGuid());
        //    var yarnStrands = 400;
        //    var emptyWeight = 13;
        //    List<BeamId> beamsWarping = new List<BeamId>();
        //    var warpingBeamId = new BeamId(Guid.NewGuid());
        //    beamsWarping.Add(warpingBeamId);

        //    //Add Existing Sizing Document
        //    var sizingId = Guid.NewGuid();
        //    var sizingDocument = 
        //        new DailyOperationSizingDocument 
        //        (
        //            Guid.NewGuid(),
        //            new MachineId(Guid.NewGuid()),
        //            new OrderId(Guid.NewGuid()),
        //            46,
        //            400,
        //            "PCA 133R",
        //            2,
        //            DateTimeOffset.UtcNow,
        //            2,
        //            OperationStatus.ONPROCESS
        //         );

        //    //Create New Preparation Object (Commands)
        //    PreparationDailyOperationSizingCommand request =
        //        new PreparationDailyOperationSizingCommand
        //        {
        //            MachineDocumentId = machineDocumentId,
        //            OrderDocumentId = orderDocumentId,
        //            RecipeCode = recipeCode,
        //            NeReal = neReal,
        //            PreparationOperator = preparationOperator,
        //            PreparationDate = preparationDate,
        //            PreparationTime = preparationTime,
        //            PreparationShift = preparationShift,
        //            YarnStrands = yarnStrands,
        //            EmptyWeight = emptyWeight
        //        };

        //    //Setup Mock Object for Sizing Repository
        //    mockSizingOperationRepo
        //        .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingDocumentReadModel, bool>>>()))
        //        .Returns(new List<DailyOperationSizingDocument>() { sizingDocument });

        //    //Set Cancellation Token
        //    CancellationToken cancellationToken = CancellationToken.None;

        //    try
        //    {
        //        // Act
        //        var result = await unitUnderTest.Handle(request, cancellationToken);
        //    }
        //    catch (Exception messageException)
        //    {
        //        // Assert
        //        Assert.Equal("Validation failed: \r\n -- OrderDocument: No. Produksi Sudah Digunakan", messageException.Message);
        //    }
        //}
    }
}
