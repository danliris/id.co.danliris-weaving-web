using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class NewEntryDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockDailyOperationSizingRepo;

        public NewEntryDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockDailyOperationSizingRepo = this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockDailyOperationSizingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private NewEntryDailyOperationSizingCommandHandler NewEntryDailyOperationSizingCommandHandler()
        {
            return
                new NewEntryDailyOperationSizingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for create new entry on daily operation sizing
         * **/
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Set new entry command handler object
            var createAddNewDailyOperationSizingCommandHandler = this.NewEntryDailyOperationSizingCommandHandler();

            //Instantiate new Object
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(new int());
            var constructionDocumentId = new ConstructionId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId>();
            var operatorId = new OperatorId(Guid.NewGuid());
            DateTimeOffset preparationDate = DateTimeOffset.UtcNow;
            var preparationTime = new TimeSpan(7);
            var shiftId = new ShiftId(Guid.NewGuid());
            var newEntryDetail = new NewEntryDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = operatorId,
                PreparationDate = preparationDate,
                PreparationTime = preparationTime,
                ShiftId = shiftId
            };

            //Create new entry object
            NewEntryDailyOperationSizingCommand request = new NewEntryDailyOperationSizingCommand
            {
                MachineDocumentId = machineDocumentId,
                WeavingUnitId = weavingUnitId,
                ConstructionDocumentId = constructionDocumentId,
                BeamsWarping = beamsWarping,
                Details = newEntryDetail,
                NeReal = 2,
                RecipeCode = "PCA 133R",
                YarnStrands = 63
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Instantiate command handler
            var result =
                await createAddNewDailyOperationSizingCommandHandler
                    .Handle(request, cancellationToken);

            //Check if object not null
            result.Should().NotBeNull();

            //Check if has identity
            result.Identity.Should().NotBeEmpty();

            //Check if has beam documents
            //result.SizingBeamDocuments.Should().NotBeEmpty();
        }
    }
}
