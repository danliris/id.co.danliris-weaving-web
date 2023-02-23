using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Movements.ReadModels
{
    public class MovementReadModel : ReadModelBase
    {
        public MovementReadModel(Guid identity) : base(identity) { }
        
        public Guid DailyOperationId { get; internal set; }
        public string MovementType { get; internal set; }
        public bool IsActive { get; internal set; }
    }
}
