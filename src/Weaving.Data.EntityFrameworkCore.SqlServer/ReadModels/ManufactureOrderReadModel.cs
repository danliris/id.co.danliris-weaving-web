using Moonlay.Domain;
using System;

namespace Weaving.Domain.ReadModels
{
    public class ManufactureOrderReadModel : ReadModelExtCore, IAuditTrail, ISoftDelete
    {
        public ManufactureOrderReadModel(Guid identity)
        {
            Identity = identity;
        }

        public int UnitDepartmentId { get; set; }

        public int MachineId { get; set; }

        public string YarnCodesJson { get;  set; }

        public ManufactureOrder.Status State { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string BlendedJson { get; set; }

        #region IAuditTrail ISoftDelete
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool? Deleted { get; set; }

        public DateTimeOffset? DeletedDate { get; set; }

        public string DeletedBy { get; set; }
        #endregion
    }
}
