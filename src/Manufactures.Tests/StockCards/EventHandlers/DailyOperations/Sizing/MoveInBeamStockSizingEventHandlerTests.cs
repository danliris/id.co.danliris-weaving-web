using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Helpers;
using Manufactures.Application.StockCards.EventHandlers.DailyOperations.Sizing;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
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
        private Mock<IStockCardRepository> 
            _stockCardRepository;
        private Mock<IBeamRepository>
            _beamRepository;

        public MoveInBeamStockSizingEventHandlerTests()
        {
            //Set up mock object
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this._stockCardRepository = this.mockRepository.Create<IStockCardRepository>();
            this._beamRepository = this.mockRepository.Create<IBeamRepository>();

            this.mockStorage.Setup(x => x.Save());
            this.mockStorage.Setup(x => x.GetRepository<IStockCardRepository>()).Returns(_stockCardRepository.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(_beamRepository.Object);
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
        public async void RunUnitTest()
        {
            await MoveIn_Stock_Card_Sizing_Should_Success_Without_Existing_MoveOut_StockCard();
            await MoveIn_Stock_Card_Sizing_Should_Success_With_Existing_MoveOut_StockCard();
        }

        private async Task MoveIn_Stock_Card_Sizing_Should_Success_Without_Existing_MoveOut_StockCard()
        {
            // Arrange
            var moveInBeamStockSizingEventHandler = this.CreateMoveInBeamStockSizingEventHandler();
            MoveInBeamStockSizingEvent notification = new MoveInBeamStockSizingEvent();
            notification.BeamId = new BeamId(Guid.NewGuid());
            notification.DailyOperationId = new DailyOperationId(Guid.NewGuid());
            notification.DateTimeOperation = DateTimeOffset.UtcNow;
            notification.StockNumber = "Testing-MovIn-Sizing";

            //Setup Beam object to mockup
            var beamDocument = new BeamDocument(notification.BeamId.Value, "Testing-Number", "Testing-Type", 20);
            beamDocument.SetLatestYarnLength(2000);
            beamDocument.SetLatestYarnStrands(2000);

            var beamValueObject = new BeamDocumentValueObject(beamDocument);

            //Setup mock object result for find query linq
            _stockCardRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<StockCardReadModel, bool>>>()))
                .Returns(new List<StockCardDocument>());

            //Setup mock object result for beam repository
            _beamRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;
            
            // Act
            await moveInBeamStockSizingEventHandler.Handle(notification,cancellationToken);

            // Assert
            moveInBeamStockSizingEventHandler.ReturnResult().Should().Equals(true);
        }

        private async Task MoveIn_Stock_Card_Sizing_Should_Success_With_Existing_MoveOut_StockCard()
        {
            // Arrange
            var moveInBeamStockSizingEventHandler = this.CreateMoveInBeamStockSizingEventHandler();
            MoveInBeamStockSizingEvent notification = new MoveInBeamStockSizingEvent();
            notification.BeamId = new BeamId(Guid.NewGuid());
            notification.DailyOperationId = new DailyOperationId(Guid.NewGuid());
            notification.DateTimeOperation = DateTimeOffset.UtcNow;
            notification.StockNumber = "Testing-MovIn-Sizing";

            //Setup Beam object to mockup
            var beamDocument = new BeamDocument(notification.BeamId.Value, "Testing-Number", "Testing-Type", 20);
            beamDocument.SetLatestYarnLength(2000);
            beamDocument.SetLatestYarnStrands(2000);

            var beamValueObject = new BeamDocumentValueObject(beamDocument);

            //Add dummy object to test with existing moveOut Stockcard
            var existingMoveOutStockCard =
                new StockCardDocument(Guid.NewGuid(),
                                      "Testing-moveout-warping",
                                      new DailyOperationId(Guid.NewGuid()),
                                      notification.BeamId,
                                      beamValueObject,
                                      StockCardStatus.MOVEOUT_STOCK);

            //Setup mock object result for find query linq
            _stockCardRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<StockCardReadModel, bool>>>())).Returns(new List<StockCardDocument>() { existingMoveOutStockCard });

            //Setup mock object result for beam repository
            _beamRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await moveInBeamStockSizingEventHandler.Handle(notification, cancellationToken);

            // Assert
            moveInBeamStockSizingEventHandler.ReturnResult().Should().Equals(true);
        }

    }
}
