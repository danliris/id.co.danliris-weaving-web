using Infrastructure;
using Moonlay.Domain;
using System;

namespace Weaving.Domain.ReadModels
{
    public class ManufactureOrderReadModel : DanLirisReadModel
    {
        public ManufactureOrderReadModel(Guid identity) : base(identity)
        {
        }

        public int UnitDepartmentId { get; set; }

        public int MachineId { get; set; }

        public string YarnCodesJson { get;  set; }

        public ManufactureOrder.Status State { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string BlendedJson { get; set; }
    }
}
