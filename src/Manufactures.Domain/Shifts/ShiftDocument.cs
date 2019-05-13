using Infrastructure.Domain;
using Manufactures.Domain.Shifts.ReadModels;
using System;

namespace Manufactures.Domain.Shifts
{
    public class ShiftDocument : AggregateRoot<ShiftDocument, ShiftReadModel>
    {
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public ShiftDocument(Guid identity,
                             string name,
                             TimeSpan startTime,
                             TimeSpan endTime) : base(identity)
        {
            Identity = identity;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;

            MarkTransient();

            ReadModel = new ShiftReadModel(Identity)
            {
                Name = this.Name,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }

        public ShiftDocument(ShiftReadModel readModel) : base(readModel)
        {
            this.Name = readModel.Name;
            this.StartTime = readModel.StartTime;
            this.EndTime = readModel.EndTime;
        }

        public void SetName(string value)
        {
            if (Name != value)
            {
                Name = value;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetStartTime(TimeSpan value)
        {

            if (StartTime != value)
            {
                StartTime = value;
                ReadModel.StartTime = StartTime;

                MarkModified();
            }
        }

        public void SetEndTime(TimeSpan value)
        {
            if (EndTime != value)
            {
                EndTime = value;
                ReadModel.EndTime = EndTime;

                MarkModified();
            }
        }

        protected override ShiftDocument GetEntity()
        {
            return this;
        }
    }
}
