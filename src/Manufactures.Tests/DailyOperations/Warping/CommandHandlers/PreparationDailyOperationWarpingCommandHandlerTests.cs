using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Warping.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.CommandHandlers
{
    public class PreparationDailyOperationWarpingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockWarpingOperationRepo;
        private readonly Mock<IDailyOperationWarpingHistoryRepository>
            mockWarpingHistoryOperationRepo;

        public PreparationDailyOperationWarpingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockWarpingOperationRepo =
                this.mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationWarpingRepository>())
                .Returns(mockWarpingOperationRepo.Object);

            this.mockWarpingHistoryOperationRepo =
                this.mockRepository.Create<IDailyOperationWarpingHistoryRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationWarpingHistoryRepository>())
                .Returns(mockWarpingHistoryOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PreparationDailyOperationWarpingCommandHandler
            CreateAddNewWarpingOperationCommandHandler()
        {
            return new PreparationDailyOperationWarpingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for create new preparation on daily
         * operation warping
         * **/
        [Fact]
        public async Task Handle_NoSameOrderUsed_DataCreated()
        {
            // Set preparation command handler object
            var unitUnderTest = this.CreateAddNewWarpingOperationCommandHandler();

            //Instantiate new Object
            //var operatorId = new OperatorId(Guid.NewGuid());
            var shiftId = new ShiftId(Guid.NewGuid());

            //Create new preparation object
            PreparationDailyOperationWarpingCommand request =
                new PreparationDailyOperationWarpingCommand
                {
                    AmountOfCones = 10,
                    PreparationDate = DateTimeOffset.UtcNow,
                    PreparationTime = TimeSpan.Parse("01:00"),
                    PreparationShift = shiftId,
                    PreparationOrder = new OrderId(Guid.NewGuid())
                };

            //Setup mock object result for beam repository
            //mockWarpingOperationRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingReadModel, bool>>>()))
            //    .Returns(new List<DailyOperationWarpingDocument>());
            this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Instantiate command handler
            var result =
                await unitUnderTest.Handle(request, cancellationToken);

            //Check if object not null
            result.Should().NotBeNull();

            //Check if has identity
            result.Identity.Should().NotBeEmpty();
        }
    }
}
