using Infrastructure.Domain;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Orders
{
    public class OrderDocument : AggregateRoot<OrderDocument, OrderReadModel>
    {
        public string OrderNumber { get; private set; }
        public DateTime Period { get; private set; }
        public ConstructionId ConstructionDocumentId { get; private set; }
        public string YarnType { get; private set; }
        public SupplierId WarpOriginIdOne { get; private set; }
        //public SupplierId WarpOriginIdTwo { get; private set; }
        public double WarpCompositionPoly { get; private set; }
        public double WarpCompositionCotton { get; private set; }
        public double WarpCompositionOthers { get; private set; }
        public SupplierId WeftOriginIdOne { get; private set; }
        public SupplierId WeftOriginIdTwo { get; private set; }
        public double WeftCompositionPoly { get; private set; }
        public double WeftCompositionCotton { get; private set; }
        public double WeftCompositionOthers { get; private set; }
        public double AllGrade { get; private set; }
        public UnitId UnitId { get; private set; }
        public string OrderStatus { get; private set; }

        public OrderDocument(Guid identity,
                             string orderNumber, 
                             DateTime period, 
                             ConstructionId constructionDocumentId, 
                             string yarnType,
                             SupplierId warpOriginIdOne,
                             SupplierId weftOriginIdOne,
                             double allGrade, 
                             UnitId unitId, 
                             string orderStatus) : base(identity)
        {
            Identity = identity;
            OrderNumber = orderNumber;
            Period = period;
            ConstructionDocumentId = constructionDocumentId;
            YarnType = yarnType;
            WarpOriginIdOne = warpOriginIdOne;
            //WarpCompositionPoly = warpCompositionPoly;
            //WarpCompositionCotton = warpCompositionCotton;
            //WarpCompositionOthers = warpCompositionOthers;
            WeftOriginIdOne = weftOriginIdOne;
            //WeftCompositionPoly = weftCompositionPoly;
            //WeftCompositionCotton = weftCompositionCotton;
            //WeftCompositionOthers = weftCompositionOthers;
            AllGrade = allGrade;
            UnitId = unitId;
            OrderStatus = orderStatus;

            MarkTransient();

            ReadModel = new OrderReadModel(Identity)
            {
                OrderNumber = OrderNumber,
                Period = Period,
                ConstructionDocumentId = ConstructionDocumentId.Value,
                YarnType = YarnType,
                WarpOriginIdOne = WarpOriginIdOne.Value,
                //WarpOriginIdTwo = WarpOriginId.Value,
                WarpCompositionPoly = WarpCompositionPoly,
                WarpCompositionCotton = WarpCompositionCotton,
                WarpCompositionOthers = WarpCompositionOthers,
                WeftOriginIdOne = WeftOriginIdOne.Value,
                WeftOriginIdTwo = WeftOriginIdTwo == null ? Guid.Empty : WeftOriginIdTwo.Value,
                WeftCompositionPoly = WeftCompositionPoly,
                WeftCompositionCotton = WeftCompositionCotton,
                WeftCompositionOthers = WeftCompositionOthers,
                AllGrade = AllGrade,
                UnitId = UnitId.Value,
                OrderStatus = OrderStatus
            };
        }

        public OrderDocument(OrderReadModel readModel) : base(readModel)
        {
            OrderNumber = readModel.OrderNumber;
            Period = readModel.Period;
            ConstructionDocumentId = new ConstructionId(readModel.ConstructionDocumentId);
            YarnType = readModel.YarnType;
            WarpOriginIdOne = new SupplierId(readModel.WarpOriginIdOne);
            //WarpOriginIdTwo = new SupplierId(readModel.WarpOriginIdTwo.HasValue ? readModel.WarpOriginIdTwo.Value : Guid.Empty);
            WarpCompositionPoly = readModel.WarpCompositionPoly;
            WarpCompositionCotton = readModel.WarpCompositionCotton;
            WarpCompositionOthers = readModel.WarpCompositionOthers;
            WeftOriginIdOne = new SupplierId(readModel.WeftOriginIdOne);
            WeftOriginIdTwo = new SupplierId(readModel.WeftOriginIdTwo ?? Guid.Empty);
            WeftCompositionPoly = readModel.WeftCompositionPoly;
            WeftCompositionCotton = readModel.WeftCompositionCotton;
            WeftCompositionOthers = readModel.WeftCompositionOthers;
            AllGrade = readModel.AllGrade;
            UnitId = new UnitId(readModel.UnitId);
            OrderStatus = readModel.OrderStatus;
        }

        public void SetOrderNumber(string orderNumber)
        {
            Validator.ThrowIfNull(() => orderNumber);

            if (orderNumber != OrderNumber)
            {

                OrderNumber = orderNumber;
                ReadModel.OrderNumber = OrderNumber;

                MarkModified();
            }
        }

        public void SetPeriod(DateTime period)
        {

            if (period != Period)
            {

                Period = period;
                ReadModel.Period = Period;

                MarkModified();
            }
        }

        public void SetConstructionDocumentId(ConstructionId constructionDocumentId)
        {
            Validator.ThrowIfNull(() => constructionDocumentId);

            if (constructionDocumentId != ConstructionDocumentId)
            {

                ConstructionDocumentId = constructionDocumentId;
                ReadModel.ConstructionDocumentId = ConstructionDocumentId.Value;

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

        public void SetWarpOriginOne(SupplierId warpOriginOne)
        {
            Validator.ThrowIfNull(() => warpOriginOne);

            if (warpOriginOne != WarpOriginIdOne)
            {

                WarpOriginIdOne = warpOriginOne;
                ReadModel.WarpOriginIdOne = WarpOriginIdOne.Value;

                MarkModified();
            }
        }

        //public void SetWarpOriginTwo(SupplierId warpOriginTwo)
        //{
        //    Validator.ThrowIfNull(() => warpOriginTwo);

        //    if (warpOriginTwo != WarpOriginIdTwo)
        //    {

        //        WarpOriginIdTwo = warpOriginTwo;
        //        ReadModel.WarpOriginIdOne = WarpOriginIdTwo.Value;

        //        MarkModified();
        //    }
        //}

        public void SetWarpCompositionPoly(double warpCompositionPoly)
        {

            if (warpCompositionPoly != WarpCompositionPoly)
            {

                WarpCompositionPoly = warpCompositionPoly;
                ReadModel.WarpCompositionPoly = WarpCompositionPoly;

                MarkModified();
            }
        }

        public void SetWarpCompositionCotton(double warpCompositionCotton)
        {

            if (warpCompositionCotton != WarpCompositionCotton)
            {

                WarpCompositionCotton = warpCompositionCotton;
                ReadModel.WarpCompositionCotton = WarpCompositionCotton;

                MarkModified();
            }
        }

        public void SetWarpCompositionOthers(double warpCompositionOthers)
        {

            if (warpCompositionOthers != WarpCompositionOthers)
            {

                WarpCompositionOthers = warpCompositionOthers;
                ReadModel.WarpCompositionOthers = WarpCompositionOthers;

                MarkModified();
            }
        }

        public void SetWeftOriginOne(SupplierId weftOriginOne)
        {
            Validator.ThrowIfNull(() => weftOriginOne);

            if (weftOriginOne != WeftOriginIdOne)
            {

                WeftOriginIdOne = weftOriginOne;
                ReadModel.WeftOriginIdOne = WeftOriginIdOne.Value;

                MarkModified();
            }
        }

        public void SetWeftOriginTwo(SupplierId weftOriginTwo)
        {
            Validator.ThrowIfNull(() => weftOriginTwo);

            if (weftOriginTwo != WeftOriginIdTwo)
            {

                WeftOriginIdTwo = weftOriginTwo;
                ReadModel.WeftOriginIdTwo = WeftOriginIdTwo.Value;

                MarkModified();
            }
        }

        public void SetWeftCompositionPoly(double weftCompositionPoly)
        {

            if (weftCompositionPoly != WeftCompositionPoly)
            {

                WeftCompositionPoly = weftCompositionPoly;
                ReadModel.WeftCompositionPoly = WeftCompositionPoly;

                MarkModified();
            }
        }

        public void SetWeftCompositionCotton(double weftCompositionCotton)
        {

            if (weftCompositionCotton != WeftCompositionCotton)
            {

                WeftCompositionCotton = weftCompositionCotton;
                ReadModel.WeftCompositionCotton = WeftCompositionCotton;

                MarkModified();
            }
        }

        public void SetWeftCompositionOthers(double weftCompositionOthers)
        {

            if (weftCompositionOthers != WeftCompositionOthers)
            {

                WeftCompositionOthers = weftCompositionOthers;
                ReadModel.WeftCompositionOthers = WeftCompositionOthers;

                MarkModified();
            }
        }

        public void SetAllGrade(double allGrade)
        {

            if (allGrade!= AllGrade)
            {

                AllGrade = allGrade;
                ReadModel.AllGrade = AllGrade;

                MarkModified();
            }
        }

        public void SetUnit(UnitId unit)
        {
            Validator.ThrowIfNull(() => unit);

            if (unit != UnitId)
            {

                UnitId = unit;
                ReadModel.UnitId = UnitId.Value;

                MarkModified();
            }
        }

        public void SetOrderStatus(string orderStatus)
        {
            Validator.ThrowIfNull(() => orderStatus);

            if (orderStatus != OrderStatus)
            {

                OrderStatus = orderStatus;
                ReadModel.OrderStatus = OrderStatus;

                MarkModified();
            }
        }

        //public void SetWarpOrigin(string warpOrigin)
        //{

        //    if (warpOrigin != WarpOrigin)
        //    {

        //        WarpOrigin = warpOrigin;
        //        ReadModel.WarpOrigin = WarpOrigin;

        //        MarkModified();
        //    }
        //}

        //public void SetWeftOrigin(string weftOrigin)
        //{

        //    if (weftOrigin != WeftOrigin)
        //    {

        //        WeftOrigin = weftOrigin;
        //        ReadModel.WeftOrigin = WeftOrigin;

        //        MarkModified();
        //    }
        //}

        //public void SetWholeGrade(int wholeGrade)
        //{

        //    if (wholeGrade != WholeGrade)
        //    {
        //        WholeGrade = wholeGrade;
        //        ReadModel.WholeGrade = WholeGrade;

        //        MarkModified();
        //    }
        //}

        //public void SetYarnType(string yarnType)
        //{

        //    if (yarnType != YarnType)
        //    {
        //        YarnType = yarnType;
        //        ReadModel.YarnType = YarnType;

        //        MarkModified();
        //    }
        //}

        //public void SetFabricConstructionDocument(ConstructionId constructionId)
        //{

        //    if (constructionId != ConstructionId)
        //    {
        //        ConstructionId = constructionId;
        //        ReadModel.ConstructionId = ConstructionId.Value;

        //        MarkModified();
        //    }
        //}

        //public void SetPeriod(Period period)
        //{

        //    if (period != Period)
        //    {

        //        Period = period;
        //        ReadModel.Period = Period.Serialize();

        //        MarkModified();
        //    }
        //}

        //public void SetWarpComposition(Composition composition)
        //{

        //    if (composition != WarpComposition)
        //    {

        //        WarpComposition = composition;
        //        ReadModel.WarpComposition = WarpComposition.Serialize();

        //        MarkModified();
        //    }
        //}

        //public void SetWeftComposition(Composition composition)
        //{

        //    if (composition != WeftComposition)
        //    {

        //        WeftComposition = composition;
        //        ReadModel.WeftComposition = WeftComposition.Serialize();

        //        MarkModified();
        //    }
        //}

        //public void SetWeavingUnit(UnitId value)
        //{

        //    if (UnitId != value)
        //    {

        //        UnitId = value;
        //        ReadModel.UnitId = value.Value;

        //        MarkModified();
        //    }
        //}
        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override OrderDocument GetEntity()
        {
            return this;
        }
    }
}
