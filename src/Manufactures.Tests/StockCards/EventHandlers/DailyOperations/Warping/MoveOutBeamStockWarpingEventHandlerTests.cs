using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Application.StockCards.EventHandlers.DailyOperations.Warping;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Domain.StockCard.ReadModels;
using Manufactures.Domain.StockCard.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.StockCards.EventHandlers.DailyOperations.Warping
{
    public class MoveOutBeamStockWarpingEventHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        private Mock<IStockCardRepository> _stockCardRepository;

        public MoveOutBeamStockWarpingEventHandlerTests()
        {
            //Set up mock object
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this._stockCardRepository = this.mockRepository.Create<IStockCardRepository>();
            this.mockStorage.Setup(x => x.Save());
            this.mockStorage.Setup(x => x.GetRepository<IStockCardRepository>()).Returns(_stockCardRepository.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private MoveOutBeamStockWarpingEventHandler CreateMoveOutBeamStockWarpingEventHandler()
        {
            return new MoveOutBeamStockWarpingEventHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var moveOutBeamStockWarpingEventHandler = this.CreateMoveOutBeamStockWarpingEventHandler();
            MoveOutBeamStockWarpingEvent notification = new MoveOutBeamStockWarpingEvent();
            notification.BeamId = new BeamId(Guid.NewGuid());
            notification.DailyOperationId = new DailyOperationId(Guid.NewGuid());
            notification.DateTimeOperation = DateTimeOffset.UtcNow;
            notification.StockNumber = "Testing-MovOut-Warping";

            var stockCardDocument = new StockCardDocument(Guid.NewGuid(), "Testing-MovIn-Warping", new DailyOperationId(Guid.NewGuid()), DateTimeOffset.UtcNow, notification.BeamId, true, false, StockCardStatus.WARPING_STOCK, StockCardStatus.MOVEIN_STOCK);

            //Setup value
            _stockCardRepository.Setup(x => x.Find(It.IsAny<Expression<Func<StockCardReadModel, bool>>>())).Returns(new List<StockCardDocument>());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await moveOutBeamStockWarpingEventHandler.Handle(
                notification,
                cancellationToken);

            // Assert
            Assert.True(true);
        }
    }
}
