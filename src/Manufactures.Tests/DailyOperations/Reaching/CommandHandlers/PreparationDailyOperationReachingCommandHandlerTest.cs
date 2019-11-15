using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.CommandHandlers;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Reaching.CommandHandlers
{
    public class PreparationDailyOperationReachingCommandHandlerTest : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingRepository>
            mockDailyOperationReachingRepo;

        public PreparationDailyOperationReachingCommandHandlerTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();
            mockStorage.Setup(x => x.Save());

            mockDailyOperationReachingRepo = mockRepository.Create<IDailyOperationReachingRepository>();
            mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingRepository>())
                .Returns(mockDailyOperationReachingRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private PreparationDailyOperationReachingCommandHandler CreatePreparationDailyOperationReachingCommandHandler()
        {
            return
                new PreparationDailyOperationReachingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Create Preparation on Daily Operation Reaching
         * **/
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            //---ARRANGE---//
            // Set Preparation Command Handler Object
            var unitUnderTest = this.CreatePreparationDailyOperationReachingCommandHandler();

            //Instantiate New Object
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(new int());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var sizingBeamId = new BeamId(Guid.NewGuid());
            var operatorId = new OperatorId(Guid.NewGuid());
            DateTimeOffset entryDate = DateTimeOffset.UtcNow;
            var entryTime = new TimeSpan(7);
            var shiftId = new ShiftId(Guid.NewGuid());

            //Create New Entry Object
            PreparationDailyOperationReachingCommand request = new PreparationDailyOperationReachingCommand
            {
                MachineDocumentId = machineDocumentId,
                OrderDocumentId = orderDocumentId,
                SizingBeamId = sizingBeamId,
                OperatorDocumentId = operatorId,
                PreparationDate = entryDate,
                PreparationTime = entryTime,
                ShiftDocumentId = shiftId
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            // Instantiate Command Handler
            var result =
                await unitUnderTest
                    .Handle(request, cancellationToken);

            //---ASSERT---//
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
