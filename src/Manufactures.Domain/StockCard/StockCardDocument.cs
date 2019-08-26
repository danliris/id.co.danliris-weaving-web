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

        public DateTimeOffset DateTimeOperation { get; private set; }

        public BeamDocumentValueObject BeamDocument { get; private set; }

        public bool IsEmpty { get; private set; }

        public bool MoveIn { get; private set; }

        public bool MoveOut { get; private set; }

        public bool IsReaching { get; internal set; }

        public bool IsTying { get; internal set; }

        public string StockType { get; private set; }

        public string StockStatus { get; private set; }

        public bool Expired { get; internal set; }

        public StockCardDocument(Guid id,
                                 string stockNumber,
                                 DailyOperationId dailyOperationId,
                                 DateTimeOffset dateTimeOperation,
                                 BeamDocumentValueObject beamDocument,
                                 bool isMoveIn,
                                 bool isMoveOut,
                                 string stockType,
                                 string stockStatus) : base(id)
        {
            Identity = id;
            StockNumber = stockNumber;
            DailyOperationId = dailyOperationId;
            DateTimeOperation = dateTimeOperation;
            BeamDocument = beamDocument;
            MoveIn = isMoveIn;
            MoveOut = isMoveOut;
            StockType = stockType;
            StockStatus = stockStatus;

            //Default Value is false, to make beam stock generic
            IsReaching = false;
            IsTying = false;
            Expired = false;

            //Update status of stock when move in
            if (MoveIn)
            {
                IsEmpty = false;
            }

            //Update status of stock when move out
            if (MoveOut)
            {
                IsEmpty = true;
            }

            MarkTransient();

            ReadModel = new StockCardReadModel(Identity)
            {
                StockNumber = StockNumber,
                DailyOperationId = DailyOperationId.Value,
                BeamDocument = JsonConvert.SerializeObject(BeamDocument),
                DateTimeOperation = DateTimeOperation,
                IsAvailable = IsEmpty,
                StockType = StockType,
                IsReaching = IsReaching,
                IsTying = IsTying,
                StockStatus = StockStatus,
                Expired = Expired
            };
        }

        public StockCardDocument(StockCardReadModel readModel) : base(readModel)
        {
            Identity = readModel.Identity;
            StockNumber = readModel.StockNumber;
            DateTimeOperation = readModel.DateTimeOperation;
            BeamDocument = JsonConvert.DeserializeObject<BeamDocumentValueObject>(readModel.BeamDocument);
            StockType = readModel.StockType;
            Expired = readModel.Expired;
            StockStatus = readModel.StockStatus;

            //Cek Status of available of stock
            if (readModel.IsAvailable)
            {
                MoveIn = true;
                MoveOut = false;
                IsEmpty = false;
            }
            else
            {
                MoveIn = false;
                MoveOut = true;
                IsEmpty = true;
            }
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

        public void UpdateIsReaching(bool value)
        {
            if (IsReaching != value)
            {
                IsReaching = value;
                ReadModel.IsReaching = IsReaching;

                MarkModified();
            }
        }

        public void UpdateIsTying(bool value)
        {
            if (IsTying != value)
            {
                IsTying = value;
                ReadModel.IsTying = IsTying;

                MarkModified();
            }
        }

        protected override StockCardDocument GetEntity()
        {
            return this;
        }
    }
}
