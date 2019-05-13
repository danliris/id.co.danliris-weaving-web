using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Shifts.ReadModels
{
    public class ShiftReadModel : ReadModelBase
    {
        public ShiftReadModel(Guid identity) : base(identity) { }

        public string Name { get; internal set; }
        public TimeSpan StartTime { get; internal set; }
        public TimeSpan EndTime { get; internal set; }
    }
}
