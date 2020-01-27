using Infrastructure.Domain;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Estimations.Productions.Entities
{
    public class EstimatedProductionDetail : AggregateRoot<EstimatedProductionDetail, EstimatedProductionDetailReadModel>
    {
        public OrderId OrderId { get; private set; }
        public ConstructionId ConstructionId { get; private set; }
        public double GradeA { get; private set; }
        public double GradeB { get; private set; }
        public double GradeC { get; private set; }
        public double GradeD { get; private set; }
        public Guid EstimatedProductionDocumentId { get; set; }

        public EstimatedProductionDetail(Guid identity,
                                         OrderId orderId, 
                                         ConstructionId constructionId, 
                                         double gradeA, 
                                         double gradeB, 
                                         double gradeC, 
                                         double gradeD, 
                                         Guid estimatedProductionDocumentId):base(identity)
        {
            Identity = identity;
            OrderId = orderId;
            ConstructionId = constructionId;
            GradeA = gradeA;
            GradeB = gradeB;
            GradeC = gradeC;
            GradeD = gradeD;
            EstimatedProductionDocumentId = estimatedProductionDocumentId;

            MarkTransient();

            ReadModel = new EstimatedProductionDetailReadModel(Identity)
            {
                OrderId = OrderId.Value,
                ConstructionId = ConstructionId.Value,
                GradeA = GradeA,
                GradeB = GradeB,
                GradeC = GradeC,
                GradeD = GradeD,
                EstimatedProductionDocumentId = EstimatedProductionDocumentId
            };
        }

        public EstimatedProductionDetail(EstimatedProductionDetailReadModel readModel) : base(readModel)
        {
            OrderId = new OrderId(readModel.OrderId);
            ConstructionId = new ConstructionId(readModel.ConstructionId);
            GradeA = readModel.GradeA;
            GradeB = readModel.GradeB;
            GradeC = readModel.GradeC;
            GradeD = readModel.GradeD;
        }

        //public EstimatedProductionDetail(Guid identity,
        //                         OrderDocumentValueObject orderDocument,
        //                         ProductGrade productGrade, 
        //                         double totalGramEstimation) : base(identity)
        //{
        //    Identity = identity;
        //    OrderDocument = orderDocument.Serialize();
        //    ProductGrade = productGrade.Serialize();
        //    TotalGramEstimation = totalGramEstimation;
        //}

        public void SetOrderId(OrderId orderId)
        {
            Validator.ThrowIfNull(() => orderId);

            if (orderId != OrderId)
            {
                OrderId = orderId;
                ReadModel.OrderId = OrderId.Value;

                MarkModified();
            }
        }

        public void SetConstructionId(ConstructionId constructionId)
        {
            Validator.ThrowIfNull(() => constructionId);

            if (constructionId != ConstructionId)
            {
                ConstructionId = constructionId;
                ReadModel.ConstructionId = ConstructionId.Value;

                MarkModified();
            }
        }

        public void SetGradeA(double gradeA)
        {
            Validator.ThrowIfNull(() => gradeA);

            if (gradeA != GradeA)
            {
                GradeA = gradeA;
                ReadModel.GradeA = GradeA;

                MarkModified();
            }
        }

        public void SetGradeB(double gradeB)
        {
            Validator.ThrowIfNull(() => gradeB);

            if (gradeB != GradeB)
            {
                GradeB = gradeB;
                ReadModel.GradeB = GradeB;

                MarkModified();
            }
        }

        public void SetGradeC(double gradeC)
        {
            Validator.ThrowIfNull(() => gradeC);

            if (gradeC != GradeC)
            {
                GradeC = gradeC;
                ReadModel.GradeC = GradeC;

                MarkModified();
            }
        }

        public void SetGradeD(double gradeD)
        {
            Validator.ThrowIfNull(() => gradeD);

            if (gradeD != GradeD)
            {
                GradeD = gradeD;
                ReadModel.GradeD = GradeD;

                MarkModified();
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        public void SetDeleted()
        {
            MarkRemoved();
        }

        protected override EstimatedProductionDetail GetEntity()
        {
            return this;
        }
    }
}
