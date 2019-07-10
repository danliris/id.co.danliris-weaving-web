using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.DailyOperations.Warping
{
    public class DailyOperationWarpingDocument
        : AggregateRoot<DailyOperationWarpingDocument, DailyOperationWarpingReadModel>
    {
        public ConstructionId ConstructionId { get; private set; }
        public MaterialTypeId MaterialTypeId { get; private set; }
        public int AmountOfCones { get; private set; }
        public string ColourOfCone { get; private set; }
        public DateTimeOffset DateTimeOperation { get; private set; }
        public OperatorId OperatorId { get; private set; }

        public DailyOperationWarpingDocument(Guid id, 
                                             ConstructionId constructionId,
                                             MaterialTypeId materialTypeId,
                                             int amountOfCones,
                                             string colourOfCone,
                                             DateTimeOffset datetimeOperation,
                                             OperatorId operatorId) : base(id)
        {
            Identity = id;
            ConstructionId = constructionId;
            MaterialTypeId = materialTypeId;
            AmountOfCones = amountOfCones;
            ColourOfCone = colourOfCone;
            DateTimeOperation = datetimeOperation;
            OperatorId = operatorId;

            this.MarkTransient();

            ReadModel = new DailyOperationWarpingReadModel(Identity)
            {
                ConstructionId = this.ConstructionId.Value,
                MaterialTypeId = this.MaterialTypeId.Value,
                AmountOfCones = this.AmountOfCones,
                ColourOfCone = this.ColourOfCone,
                DateTimeOperation = this.DateTimeOperation,
                OperatorId = this.OperatorId.Value
            };
        }

        public DailyOperationWarpingDocument(DailyOperationWarpingReadModel readModel) : base (readModel)
        {
            this.ConstructionId = new ConstructionId(readModel.ConstructionId);
            this.MaterialTypeId = new MaterialTypeId(readModel.MaterialTypeId);
            this.AmountOfCones = readModel.AmountOfCones;
            this.ColourOfCone = readModel.ColourOfCone;
            this.DateTimeOperation = readModel.DateTimeOperation;
            this.OperatorId = new OperatorId(readModel.OperatorId);
        }

        protected override DailyOperationWarpingDocument GetEntity()
        {
            return this;
        }
    }
}
