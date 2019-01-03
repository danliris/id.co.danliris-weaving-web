using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Orders
{
    public class WeavingOrderDocument : AggregateRoot<WeavingOrderDocument, WeavingOrderDocumentReadModel>
    {
        public WeavingOrderDocument(Guid id, string orderNumber,
                                    FabricSpecificationId fabricSpecificationId,
                                    DateTimeOffset dateOrdered,
                                    Period period,
                                    Composition composition,
                                    string warpOrigin,
                                    string weftOrigin,
                                    int wholeGrade,
                                    string yarnType,
                                    WeavingUnit weavingUnit,
                                    string userId
                                    ) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => orderNumber);
            Validator.ThrowIfNull(() => fabricSpecificationId);
            Validator.ThrowIfNull(() => dateOrdered);
            Validator.ThrowIfNull(() => period);
            Validator.ThrowIfNull(() => composition);
            Validator.ThrowIfNullOrEmpty(() => warpOrigin);
            Validator.ThrowIfNullOrEmpty(() => weftOrigin);
            Validator.ThrowIfNull(() => wholeGrade);
            Validator.ThrowIfNullOrEmpty(() => yarnType);
            Validator.ThrowIfNull(() => weavingUnit);
            Validator.ThrowIfNullOrEmpty(() => userId);

            this.MarkTransient();

            // Set Initial Value
            Identity = id;
            OrderNumber = orderNumber;
            FabricSpecificationId = fabricSpecificationId;
            DateOrdered = dateOrdered;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            WholeGrade = wholeGrade;
            YarnType = yarnType;
            Period = period;
            Composition = composition;
            WeavingUnit = weavingUnit;
            UserId = userId;

            ReadModel = new WeavingOrderDocumentReadModel(Identity)
            {
                OrderNumber = this.OrderNumber,
                DateOrdered = this.DateOrdered,
                WarpOrigin = this.WarpOrigin,
                WeftOrigin = this.WeftOrigin,
                WholeGrade = this.WholeGrade,
                YarnType = this.YarnType,
                Period = this.Period.Serialize(),
                Composition = this.Composition.Serialize(),
                WeavingUnit = this.Composition.Serialize(),
                UserId = this.UserId
            };

            if(this.FabricSpecificationId != null)
            {
                ReadModel.FabricSpecificationId = FabricSpecificationId.Value;
            }

            ReadModel.AddDomainEvent(new OnWeavingOrderPlaced(this.Identity));
        }

        public WeavingOrderDocument(WeavingOrderDocumentReadModel readModel) : base(readModel)
        {
            this.OrderNumber = ReadModel.OrderNumber;
            this.FabricSpecificationId = ReadModel.FabricSpecificationId.HasValue ? new FabricSpecificationId(ReadModel.FabricSpecificationId.Value) : null;
            this.DateOrdered = ReadModel.DateOrdered;
            this.WarpOrigin = ReadModel.WarpOrigin;
            this.WeftOrigin = ReadModel.WeftOrigin;
            this.WholeGrade = ReadModel.WholeGrade;
            this.YarnType = ReadModel.YarnType;
            this.Period = ReadModel.Period.Deserialize<Period>();
            this.Composition = ReadModel.Composition.Deserialize<Composition>();
            this.WeavingUnit = ReadModel.WeavingUnit.Deserialize<WeavingUnit>();
            this.UserId = ReadModel.UserId;
        }

        public string OrderNumber { get; private set; }
        public FabricSpecificationId FabricSpecificationId { get; private set; }
        public DateTimeOffset DateOrdered { get; private set; }
        public string WarpOrigin { get; private set; }
        public string WeftOrigin { get; private set; }
        public int WholeGrade { get; private set; }
        public string YarnType { get; private set; }
        public Period Period { get; private set; }
        public Composition Composition { get; private set; }
        public WeavingUnit WeavingUnit { get; private set; }
        public string UserId { get; private set; }
        
        public void SetWarpOrigin(string warpOrigin)
        {
            Validator.ThrowIfNull(() => warpOrigin);

            if (warpOrigin != WarpOrigin)
            {
                WarpOrigin = warpOrigin;
                ReadModel.WarpOrigin = WarpOrigin;

                MarkModified();
            }
        }

        public void SetWeftOrigin(string weftOrigin)
        {
            Validator.ThrowIfNull(() => weftOrigin);

            if(weftOrigin != WeftOrigin)
            {
                WeftOrigin = weftOrigin;
                ReadModel.WeftOrigin = WeftOrigin;

                MarkModified();
            }
        }

        public void SetWholeGrade(int wholeGrade)
        {
            Validator.ThrowIfNull(() => wholeGrade);

            if (wholeGrade != WholeGrade)
            {
                WholeGrade = wholeGrade;
                ReadModel.WholeGrade = WholeGrade;

                MarkModified();
            }
        }

        public void SetYarnType(string yarnType)
        {
            Validator.ThrowIfNull(() => yarnType);

            if (yarnType != YarnType)
            {
                YarnType = yarnType;
                ReadModel.YarnType = YarnType;

                MarkModified();
            }
        }

        public void SetFabricSpecificationId(FabricSpecificationId fabricSpecificationId)
        {
            Validator.ThrowIfNull(() => fabricSpecificationId);

            if(fabricSpecificationId != FabricSpecificationId)
            {
                FabricSpecificationId = fabricSpecificationId;
                ReadModel.FabricSpecificationId = FabricSpecificationId.Value;

                MarkModified();
            }
        }

        public void SetPeriod(Period period)
        {
            Validator.ThrowIfNull(() => period);

            if (period != Period)
            {
                Period = period;
                ReadModel.Period = Period.Serialize();

                MarkModified();
            }
        }

        public void SetComposition(Composition composition)
        {
            Validator.ThrowIfNull(() => composition);

            if (composition != Composition)
            {
                Composition = composition;
                ReadModel.Composition = Composition.Serialize();

                MarkModified();
            }
        }

        public void SetWeavingUnit(WeavingUnit weavingUnit)
        {
            Validator.ThrowIfNull(() => weavingUnit);

            if(weavingUnit != WeavingUnit)
            {
                WeavingUnit = weavingUnit;
                ReadModel.WeavingUnit = WeavingUnit.Serialize();

                MarkModified();
            }
        }

        public void SetUserId(string userId)
        {
            Validator.ThrowIfNull(() => userId);

            if(userId != UserId)
            {
                UserId = userId;
                ReadModel.UserId = UserId;

                MarkModified();
            }
        }

        protected override WeavingOrderDocument GetEntity()
        {
            throw new NotImplementedException();
        }
    }
}
