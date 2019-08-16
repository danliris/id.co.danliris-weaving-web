using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Warping.CommandHandlers;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.CommandHandlers
{
    public class PreparationWarpingOperationCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository> 
            mockWarpingOperationRepo;


        public PreparationWarpingOperationCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockWarpingOperationRepo =
                this.mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationWarpingRepository>())
                .Returns(mockWarpingOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PreparationWarpingOperationCommandHandler 
            CreateAddNewWarpingOperationCommandHandler()
        {
            return 
                new PreparationWarpingOperationCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for create new preparation on daily
         * operation warping
         * **/
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Set preparation command handler object
            var preparationWarpingOperationCommandHandler = 
                this.CreateAddNewWarpingOperationCommandHandler();

            //Instantiate new Object
            var constructionId = new ConstructionId(Guid.NewGuid());
            var materialTypeId = new MaterialTypeId(Guid.NewGuid());
            var operatorId = new OperatorId(Guid.NewGuid());
            var shiftId = new ShiftId(Guid.NewGuid());

            //Create new preparation object
            PreparationWarpingOperationCommand request = 
                new PreparationWarpingOperationCommand
            {
                ConstructionId = constructionId,
                MaterialTypeId = materialTypeId,
                AmountOfCones = 10,
                ColourOfCone = "Red",
                DateOperation = DateTimeOffset.UtcNow,
                TimeOperation = TimeSpan.Parse("01:00"),
                OperatorId = operatorId,
                ShiftId = shiftId
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Instantiate command handler
            var result = 
                await preparationWarpingOperationCommandHandler
                    .Handle(request, cancellationToken);

            //Check if object not null
            result.Should().NotBeNull();

            //Check if has identity
            result.Identity.Should().NotBeEmpty();

            //check if has history
            result.DailyOperationWarpingDetailHistory.Should().NotBeEmpty();
        }
    }
}
