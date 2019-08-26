using ExtCore.Data.Abstractions;
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
    public class MoveOutBeamStockSizingEventHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private Mock<IStockCardRepository> 
            _stockCardRepository;
        private Mock<IBeamRepository>
           _beamRepository;

        public MoveOutBeamStockSizingEventHandlerTests()
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

        private MoveOutBeamStockSizingEventHandler CreateMoveOutBeamStockSizingEventHandler()
        {
            return new MoveOutBeamStockSizingEventHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task MoveOut_Stock_Card_Sizing_Should_Success()
        {
            // Arrange
            var moveOutBeamStockSizingEventHandler = this.CreateMoveOutBeamStockSizingEventHandler();
            MoveOutBeamStockSizingEvent notification = new MoveOutBeamStockSizingEvent();
            notification.BeamId = new BeamId(Guid.NewGuid());
            notification.DailyOperationId = new DailyOperationId(Guid.NewGuid());
            notification.DateTimeOperation = DateTimeOffset.UtcNow;
            notification.StockNumber = "Testing-MoveIn-Sizing";

            //Setup Beam object to mockup
            var beamDocument = new BeamDocument(notification.BeamId.Value, "Testing-Number", "Testing-Type", 20);
            beamDocument.SetLatestYarnLength(2000);
            beamDocument.SetLatestYarnStrands(2000);

            var beamValueObject = new BeamDocumentValueObject(beamDocument);


            var stockCardDocument = new StockCardDocument(Guid.NewGuid(), "Testing-MoveIn-Sizing", new DailyOperationId(Guid.NewGuid()), DateTimeOffset.UtcNow , beamValueObject, true, false, StockCardStatus.SIZING_STOCK, StockCardStatus.MOVEIN_STOCK);
            var stockCardDocumentTwo = new StockCardDocument(Guid.NewGuid(), "Testing-MoveIn-Sizing", new DailyOperationId(Guid.NewGuid()), DateTimeOffset.UtcNow, beamValueObject, true, false, StockCardStatus.WARPING_STOCK, StockCardStatus.MOVEIN_STOCK);

            //Setup value
            _stockCardRepository.Setup(x => x.Find(It.IsAny<Expression<Func<StockCardReadModel, bool>>>())).Returns(new List<StockCardDocument>() { stockCardDocument, stockCardDocumentTwo });

            //Setup mock object result for beam repository
            _beamRepository
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await moveOutBeamStockSizingEventHandler.Handle(
                notification,
                cancellationToken);

            // Assert
            Assert.True(true);
        }
    }
}
