using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Orders
{
    public class WeavingOrderDocument : AggregateRoot<WeavingOrderDocument, WeavingOrderDocumentReadModel>
    {
        public string OrderNumber { get; private set; }
        public ConstructionId ConstructionId { get; private set; }
        public DateTimeOffset DateOrdered { get; private set; }
        public string WarpOrigin { get; private set; }
        public string WeftOrigin { get; private set; }
        public int WholeGrade { get; private set; }
        public string YarnType { get; private set; }
        public Period Period { get; private set; }
        public Composition WarpComposition { get; private set; }
        public Composition WeftComposition { get; private set; }
        public UnitId UnitId { get; private set; }
        public string OrderStatus { get; private set; }

        public WeavingOrderDocument(Guid id, string orderNumber,
                                    ConstructionId constructionId,
                                    DateTimeOffset dateOrdered,
                                    Period period,
                                    Composition warpComposition,
                                    Composition weftComposition,
                                    string warpOrigin,
                                    string weftOrigin,
                                    int wholeGrade,
                                    string yarnType,
                                    UnitId unitId,
                                    string orderStatus) : base(id)
        {
            // Set Initial Value
            Identity = id;
            OrderNumber = orderNumber;
            ConstructionId = constructionId;
            DateOrdered = dateOrdered;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            WholeGrade = wholeGrade;
            YarnType = yarnType;
            Period = period;
            WarpComposition = warpComposition;
            WeftComposition = weftComposition;
            UnitId = unitId;
            OrderStatus = orderStatus;

            this.MarkTransient();

            ReadModel = new WeavingOrderDocumentReadModel(Identity)
            {
                OrderNumber = this.OrderNumber,
                DateOrdered = this.DateOrdered,
                ConstructionId = this.ConstructionId.Value,
                WarpOrigin = this.WarpOrigin,
                WeftOrigin = this.WeftOrigin,
                WholeGrade = this.WholeGrade,
                YarnType = this.YarnType,
                Period = this.Period.Serialize(),
                WarpComposition = this.WarpComposition.Serialize(),
                WeftComposition = this.WeftComposition.Serialize(),
                UnitId = this.UnitId.Value,
                OrderStatus = this.OrderStatus
            };

            ReadModel.AddDomainEvent(new OnWeavingOrderPlaced(this.Identity));
        }

        public WeavingOrderDocument(WeavingOrderDocumentReadModel readModel) : base(readModel)
        {
            this.OrderNumber = readModel.OrderNumber;
            this.ConstructionId =
                readModel.ConstructionId.HasValue ? new ConstructionId(readModel.ConstructionId.Value) : null;
            this.DateOrdered = readModel.DateOrdered;
            this.WarpOrigin = readModel.WarpOrigin;
            this.WeftOrigin = readModel.WeftOrigin;
            this.WholeGrade = readModel.WholeGrade;
            this.YarnType = readModel.YarnType;
            this.Period = readModel.Period.Deserialize<Period>();
            this.WarpComposition =
                readModel.WarpComposition.Deserialize<Composition>();
            this.WeftComposition =
                readModel.WeftComposition.Deserialize<Composition>();
            this.UnitId =
                readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.OrderStatus = readModel.OrderStatus;
        }

        public void SetOrderStatus(string orderStatus)
        {

            if (OrderStatus != orderStatus)
            {

                OrderStatus = orderStatus;
                ReadModel.OrderStatus = OrderStatus;

                MarkModified();
            }
        }

        public void SetWarpOrigin(string warpOrigin)
        {

            if (warpOrigin != WarpOrigin)
            {

                WarpOrigin = warpOrigin;
                ReadModel.WarpOrigin = WarpOrigin;

                MarkModified();
            }
        }

        public void SetWeftOrigin(string weftOrigin)
        {

            if (weftOrigin != WeftOrigin)
            {

                WeftOrigin = weftOrigin;
                ReadModel.WeftOrigin = WeftOrigin;

                MarkModified();
            }
        }

        public void SetWholeGrade(int wholeGrade)
        {

            if (wholeGrade != WholeGrade)
            {
                WholeGrade = wholeGrade;
                ReadModel.WholeGrade = WholeGrade;

                MarkModified();
            }
        }

        public void SetYarnType(string yarnType)
        {

            if (yarnType != YarnType)
            {
                YarnType = yarnType;
                ReadModel.YarnType = YarnType;

                MarkModified();
            }
        }

        public void SetFabricConstructionDocument(ConstructionId constructionId)
        {

            if (constructionId != ConstructionId)
            {
                ConstructionId = constructionId;
                ReadModel.ConstructionId = ConstructionId.Value;

                MarkModified();
            }
        }

        public void SetPeriod(Period period)
        {

            if (period != Period)
            {

                Period = period;
                ReadModel.Period = Period.Serialize();

                MarkModified();
            }
        }

        public void SetWarpComposition(Composition composition)
        {

            if (composition != WarpComposition)
            {

                WarpComposition = composition;
                ReadModel.WarpComposition = WarpComposition.Serialize();

                MarkModified();
            }
        }

        public void SetWeftComposition(Composition composition)
        {

            if (composition != WeftComposition)
            {

                WeftComposition = composition;
                ReadModel.WeftComposition = WeftComposition.Serialize();

                MarkModified();
            }
        }

        public void SetWeavingUnit(UnitId value)
        {

            if (UnitId != value)
            {

                UnitId = value;
                ReadModel.UnitId = value.Value;

                MarkModified();
            }
        }

        protected override WeavingOrderDocument GetEntity()
        {
            return this;
        }
    }
}
