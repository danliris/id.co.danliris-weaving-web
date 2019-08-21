using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.StockCards.EventHandlers.DailyOperations.Sizing;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Sizing;
using Manufactures.Domain.StockCard.ReadModels;
using Manufactures.Domain.StockCard.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.StockCards.EventHandlers.DailyOperations.Sizing
{
    public class MoveInBeamStockSizingEventHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        private Mock<IStockCardRepository> _stockCardRepository;

        public MoveInBeamStockSizingEventHandlerTests()
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

        private MoveInBeamStockSizingEventHandler CreateMoveInBeamStockSizingEventHandler()
        {
            return new MoveInBeamStockSizingEventHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task MoveIn_Stock_Card_Sizing_Should_Success()
        {
            // Arrange
            var moveInBeamStockSizingEventHandler = this.CreateMoveInBeamStockSizingEventHandler();
            MoveInBeamStockSizingEvent notification = new MoveInBeamStockSizingEvent();
            notification.BeamId = new BeamId(Guid.NewGuid());
            notification.DailyOperationId = new DailyOperationId(Guid.NewGuid());
            notification.DateTimeOperation = DateTimeOffset.UtcNow;
            notification.StockNumber = "Testing-MovIn-Sizing";

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;
            
            // Act
            await moveInBeamStockSizingEventHandler.Handle(notification,cancellationToken);

            // Assert
            moveInBeamStockSizingEventHandler.ReturnResult().Should().Equals(true);
        }
    }
}
