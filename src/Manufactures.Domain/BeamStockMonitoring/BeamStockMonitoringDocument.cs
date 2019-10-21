using Infrastructure.Domain;
using Manufactures.Domain.BeamStockMonitoring.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BeamStockMonitoring
{
    public class BeamStockMonitoringDocument : AggregateRoot<BeamStockMonitoringDocument, BeamStockMonitoringReadModel>
    {
        public BeamId BeamDocumentId { get; private set; }
        public DateTimeOffset SizingEntryDate { get; private set; }
        public DateTimeOffset ReachingEntryDate { get; private set; }
        public DateTimeOffset LoomEntryDate { get; private set; }
        public DateTimeOffset EmptyEntryDate { get; private set; }
        public OrderId OrderDocumentId { get; private set; }
        public double SizingLengthStock { get; private set; }
        public double ReachingLengthStock { get; private set; }
        public double LoomLengthStock { get; private set; }
        public int LengthUOMId { get; private set; }
        public int Position { get; private set; }
        public bool LoomFinish { get; private set; }

        //Main Constructor
        public BeamStockMonitoringDocument(Guid id,
                                           BeamId beamDocumentId,
                                           DateTimeOffset sizingEntryDate,
                                           OrderId orderDocumentId,
                                           double sizingLengthStock,
                                           int lengthUOMId,
                                           int position,
                                           bool loomFinish) : base(id)
        {
            //Instantiate Properties from Parameter Variable
            Identity = id;
            BeamDocumentId = beamDocumentId;
            SizingEntryDate = sizingEntryDate;
            //ReachingEntryDate = reachingEntryDate;
            //LoomEntryDate = loomEntryDate;
            //EmptyEntryDate = emptyEntryDate;
            OrderDocumentId = orderDocumentId;
            SizingLengthStock = sizingLengthStock;
            //ReachingLengthStock = reachingLengthStock;
            //LoomLengthStock = loomLengthStock;
            LengthUOMId = lengthUOMId;
            Position = position;
            LoomFinish = loomFinish;

            this.MarkTransient();

            //Save Object to Database as New One
            ReadModel = new BeamStockMonitoringReadModel(Identity)
            {
                BeamDocumentId = this.BeamDocumentId.Value,
                SizingEntryDate = this.SizingEntryDate,
                ReachingEntryDate = this.ReachingEntryDate,
                LoomEntryDate = this.LoomEntryDate,
                EmptyEntryDate = this.EmptyEntryDate,
                OrderDocumentId = this.OrderDocumentId.Value,
                SizingLengthStock = this.SizingLengthStock,
                ReachingLengthStock = this.ReachingLengthStock,
                LoomLengthStock = this.LoomLengthStock,
                LengthUOMId = this.LengthUOMId,
                Position = this.Position,
                LoomFinish = this.LoomFinish
            };
        }

        //Constructor for Mapping Object from Database to Domain
        public BeamStockMonitoringDocument(BeamStockMonitoringReadModel readModel) : base(readModel)
        {
            //Instantiate Object from Database
            this.BeamDocumentId = new BeamId(readModel.BeamDocumentId);
            this.SizingEntryDate = readModel.SizingEntryDate;
            this.ReachingEntryDate = readModel.ReachingEntryDate;
            this.LoomEntryDate = readModel.LoomEntryDate;
            this.EmptyEntryDate = readModel.EmptyEntryDate;
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.SizingLengthStock = readModel.SizingLengthStock;
            this.ReachingLengthStock = readModel.ReachingLengthStock;
            this.LoomLengthStock = readModel.LoomLengthStock;
            this.LengthUOMId = readModel.LengthUOMId;
            this.Position = readModel.Position;
            this.LoomFinish = readModel.LoomFinish;
        }

        public void SetBeamDocumentId(BeamId value)
        {
            BeamDocumentId = value;
            ReadModel.BeamDocumentId = BeamDocumentId.Value;

            MarkModified();
        }

        public void SetSizingEntryDate(DateTimeOffset value)
        {
            SizingEntryDate = value;
            ReadModel.SizingEntryDate = SizingEntryDate;

            MarkModified();
        }

        public void SetReachingEntryDate(DateTimeOffset value)
        {
            ReachingEntryDate = value;
            ReadModel.ReachingEntryDate = ReachingEntryDate;

            MarkModified();
        }

        public void SetLoomEntryDate(DateTimeOffset value)
        {
            LoomEntryDate = value;
            ReadModel.LoomEntryDate = LoomEntryDate;

            MarkModified();
        }

        public void SetEmptyEntryDate(DateTimeOffset value)
        {
            EmptyEntryDate = value;
            ReadModel.EmptyEntryDate = EmptyEntryDate;

            MarkModified();
        }

        public void SetOrderDocumentId(OrderId value)
        {
            OrderDocumentId = value;
            ReadModel.OrderDocumentId = OrderDocumentId.Value;

            MarkModified();
        }

        public void SetSizingLengthStock(double value)
        {
            SizingLengthStock = value;
            ReadModel.SizingLengthStock = SizingLengthStock;

            MarkModified();
        }

        public void SetReachingLengthStock(double value)
        {
            ReachingLengthStock = value;
            ReadModel.ReachingLengthStock = ReachingLengthStock;

            MarkModified();
        }

        public void SetLoomLengthStock(double value)
        {
            LoomLengthStock = value;
            ReadModel.LoomLengthStock = LoomLengthStock;

            MarkModified();
        }

        public void SetLengthUOMId(int value)
        {
            LengthUOMId = value;
            ReadModel.LengthUOMId = LengthUOMId;

            MarkModified();
        }

        public void SetPosition(int value)
        {
            Position = value;
            ReadModel.Position = Position;

            MarkModified();
        }

        public void SetLoomFinish(bool value)
        {
            LoomFinish = value;
            ReadModel.LoomFinish = LoomFinish;

            MarkModified();
        }

        //Get Entity
        protected override BeamStockMonitoringDocument GetEntity()
        {
            return this;
        }
    }
}
