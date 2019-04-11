using Infrastructure.Domain;
using Manufactures.Domain.Shifts.ReadModels;
using System;

namespace Manufactures.Domain.Shifts
{
    public class ShiftDocument : AggregateRoot<ShiftDocument, ShiftReadModel>
    {
        public string Name { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }

        public ShiftDocument(Guid identity, 
                             string name, 
                             string startTime, 
                             string endTime) : base(identity)
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
            if(Name != value)
            {
                Name = value;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetStartTime(string value)
        {
            var startTime = DateTimeOffset.Parse(StartTime);
            var valueTime = DateTimeOffset.Parse(value);

            if(startTime.Hour != valueTime.Hour)
            {
                StartTime = value;
                ReadModel.StartTime = StartTime;

                MarkModified();
            }
        }

        public void SetEndTime(string value)
        {
            var endTime = DateTimeOffset.Parse(EndTime);
            var valueTime = DateTimeOffset.Parse(value);

            if (endTime.Hour != valueTime.Hour)
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
