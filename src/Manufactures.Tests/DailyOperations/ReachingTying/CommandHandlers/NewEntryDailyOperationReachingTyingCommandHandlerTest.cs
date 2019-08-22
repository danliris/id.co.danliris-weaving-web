using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.ReachingTying.CommandHandlers
{
    public class NewEntryDailyOperationReachingTyingCommandHandlerTest : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingTyingRepository>
            mockDailyOperationReachingTyingRepo;

        public NewEntryDailyOperationReachingTyingCommandHandlerTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();
            mockStorage.Setup(x => x.Save());

            mockDailyOperationReachingTyingRepo = mockRepository.Create<IDailyOperationReachingTyingRepository>();
            mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingTyingRepository>())
                .Returns(mockDailyOperationReachingTyingRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private NewEntryDailyOperationReachingTyingCommandHandler NewEntryDailyOperationReachingCommandHandler()
        {
            return
                new NewEntryDailyOperationReachingTyingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Create New Entry on Daily Operation Reaching
         * **/
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            //---ARRANGE---//
            // Set New Entry Command Handler Object
            var createAddNewDailyOperationReachingCommandHandler = this.NewEntryDailyOperationReachingCommandHandler();

            //Instantiate New Object
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(new int());
            var constructionDocumentId = new ConstructionId(Guid.NewGuid());
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var pisPieces = 20;
            var operatorId = new OperatorId(Guid.NewGuid());
            DateTimeOffset entryDate = DateTimeOffset.UtcNow;
            var entryTime = new TimeSpan(7);
            var shiftId = new ShiftId(Guid.NewGuid());

            //Create New Entry Object
            NewEntryDailyOperationReachingTyingCommand request = new NewEntryDailyOperationReachingTyingCommand
            {
                MachineDocumentId = machineDocumentId,
                WeavingUnitId = weavingUnitId,
                ConstructionDocumentId = constructionDocumentId,
                SizingBeamId = sizingBeamId,
                OperatorDocumentId = operatorId,
                PISPieces = pisPieces,
                EntryDate = entryDate,
                EntryTime = entryTime,
                ShiftDocumentId = shiftId
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            // Instantiate Command Handler
            var result =
                await createAddNewDailyOperationReachingCommandHandler
                    .Handle(request, cancellationToken);

            //---ASSERT---//
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
