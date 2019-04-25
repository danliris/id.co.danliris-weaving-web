using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Shifts.ReadModels
{
    public class ShiftReadModel : ReadModelBase
    {
        public ShiftReadModel(Guid identity) : base(identity) { }

        public string Name { get; internal set; }
        public string StartTime { get; internal set; }
        public string EndTime { get; internal set; }
    }
}
