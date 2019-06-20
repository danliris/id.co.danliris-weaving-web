using Infrastructure.Domain;
using Manufactures.Domain.Movements.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Movements
{
    public class MovementDocument
        : AggregateRoot<MovementDocument, MovementReadModel>
    {
        public DailyOperationId DailyOperationId { get; private set; }
        public string MovementType { get; private set; }
        public bool IsActive { get; private set; }

        public MovementDocument(Guid id,
                                DailyOperationId dailyOperationId,
                                string movementType,
                                bool isActive) : base(id)
        {
            Identity = id;
            DailyOperationId = dailyOperationId;
            MovementType = movementType;
            IsActive = isActive;

            this.MarkTransient();

            ReadModel = new MovementReadModel(Identity)
            {
                DailyOperationId = this.DailyOperationId.Value,
                MovementType = this.MovementType,
                IsActive = this.IsActive
            };
        }

        public MovementDocument(MovementReadModel readModel) : base(readModel)
        {
            this.DailyOperationId = new DailyOperationId(readModel.DailyOperationId);
            this.MovementType = readModel.MovementType;
            this.IsActive = readModel.IsActive;
        }

        public void UpdateActiveMovement(bool isActive)
        {
            if (!IsActive.Equals(isActive))
            {
                IsActive = isActive;
                ReadModel.IsActive = IsActive;
                this.MarkModified();
            }
        }

        protected override MovementDocument GetEntity()
        {
            return this;
        }
    }
}
