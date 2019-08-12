using Infrastructure.Domain;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard.ReadModels;
using System;

namespace Manufactures.Domain.StockCard
{
    public class StockCardDocument : AggregateRoot<StockCardDocument, StockCardReadModel>
    {
        public string StockNumber { get; private set; }

        public DailyOperationId DailyOperationId { get; private set; }

        public DateTimeOffset DateTimeOperation { get; private set; }

        public BeamId BeamId { get; private set; }

        public double Length { get; private set; }

        public double YarnStrands { get; private set; }

        public bool IsAvailable { get; private set; }

        public bool MoveIn { get; private set; }

        public bool MoveOut { get; private set; }

        public StockCardDocument(Guid id,
                                 string stockNumber,
                                 DailyOperationId dailyOperationId,
                                 DateTimeOffset dateTimeOperation,
                                 BeamId beamId,
                                 double length,
                                 double yarnStrands,
                                 bool isMoveIn,
                                 bool isMoveOut) : base(id)
        {
            Identity = id;
            StockNumber = stockNumber;
            DailyOperationId = dailyOperationId;
            DateTimeOperation = dateTimeOperation;
            BeamId = beamId;
            Length = length;
            YarnStrands = yarnStrands;
            MoveIn = isMoveIn;
            MoveOut = isMoveOut;

            //Cek Status of available of stock
            if (MoveIn)
            {
                IsAvailable = true;
            }
            else if (MoveOut)
            {
                IsAvailable = false;
            }

            MarkTransient();

            ReadModel = new StockCardReadModel(Identity)
            {
                StockNumber = StockNumber,
                DailyOperationId = DailyOperationId.Value,
                BeamId = BeamId.Value,
                DateTimeOperation = DateTimeOperation,
                Length = Length,
                YarnStrands = YarnStrands,
                IsAvailable = IsAvailable
            };
        }

        public StockCardDocument(StockCardReadModel readModel) : base(readModel)
        {
            Identity = readModel.Identity;
            StockNumber = readModel.StockNumber;
            DateTimeOperation = readModel.DateTimeOperation;
            BeamId = new BeamId(readModel.BeamId);
            Length = readModel.Length ?? 0;
            YarnStrands = readModel.YarnStrands ?? 0;

            //Cek Status of available of stock
            if (readModel.IsAvailable)
            {
                MoveIn = true;
                MoveOut = false;
            }
            else
            {
                MoveIn = false;
                MoveOut = true;
            }
        }

        public void UpdateAvailability(bool moveIn, bool moveOut)
        {
            //Cek Status of available of stock
            if (MoveIn)
            {
                IsAvailable = true;
            }
            else if (MoveOut)
            {
                IsAvailable = false;
            }

            ReadModel.IsAvailable = IsAvailable;

            MarkModified();
        }

        protected override StockCardDocument GetEntity()
        {
            return this;
        }
    }
}
