using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Entities;
using System;

namespace Manufactures.Domain.ReadModels
{
    public class ManufactureOrderReadModel : ReadModelBase
    {
        public ManufactureOrderReadModel(Guid identity) : base(identity)
        {
        }

        public int UnitDepartmentId { get; internal set; }

        public int MachineId { get; internal set; }

        public string YarnCodesJson { get; internal set; }

        public ManufactureOrder.Status State { get; internal set; }

        public DateTimeOffset OrderDate { get; internal set; }

        public string BlendedJson { get; internal set; }

        public string UserId { get; internal set; }

        public virtual GoodsComposition Composition { get; internal set; }

        public Guid? CompositionId { get; internal set; }
    }
}