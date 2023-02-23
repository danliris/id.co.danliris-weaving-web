using Infrastructure.Domain;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard.ReadModels;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.StockCard
{
    public class StockCardDocument : AggregateRoot<StockCardDocument, StockCardReadModel>
    {
        public string StockNumber { get; private set; }
        public DailyOperationId DailyOperationId { get; private set; }
        public BeamId BeamId { get; private set; }
        public BeamDocumentValueObject BeamDocument { get; private set; }
        public string StockStatus { get; private set; }
        public bool Expired { get; internal set; }

        public StockCardDocument(Guid id,
                                 string stockNumber,
                                 DailyOperationId dailyOperationId,
                                 BeamId beamId,
                                 BeamDocumentValueObject beamDocument,
                                 string stockStatus) : base(id)
        {
            Identity = id;
            StockNumber = stockNumber;
            DailyOperationId = dailyOperationId;
            BeamId = beamId;
            BeamDocument = beamDocument;
            StockStatus = stockStatus;

            MarkTransient();

            ReadModel = new StockCardReadModel(Identity)
            {
                StockNumber = StockNumber,
                DailyOperationId = DailyOperationId.Value,
                BeamId = BeamId.Value,
                BeamDocument = JsonConvert.SerializeObject(BeamDocument),
                StockStatus = StockStatus,
                Expired = Expired
            };
        }

        public StockCardDocument(StockCardReadModel readModel) : base(readModel)
        {
            Identity = readModel.Identity;
            StockNumber = readModel.StockNumber;
            BeamId = new BeamId(readModel.BeamId);
            BeamDocument = 
                JsonConvert
                    .DeserializeObject<BeamDocumentValueObject>(readModel.BeamDocument);
            Expired = readModel.Expired;
            StockStatus = readModel.StockStatus;
        }

        public void UpdateExpired(bool value)
        {
            if (Expired != value)
            {
                Expired = value;
                ReadModel.Expired = Expired;

                MarkModified();
            }
        }

        protected override StockCardDocument GetEntity()
        {
            return this;
        }
    }
}
