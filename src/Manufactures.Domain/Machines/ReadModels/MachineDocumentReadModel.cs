using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Machines.ReadModels
{
    public class MachineDocumentReadModel : ReadModelBase
    {
        public MachineDocumentReadModel(Guid identity) : base(identity) { }
        
        public string MachineNumber { get; internal set; }
        public int? WeavingUnitId { get; internal set; }
        public Guid? MachineTypeId { get; internal set; }
        public string Location { get; internal set; }
        public int? Cutmark{ get; internal set; }
        public string CutmarkUom { get; internal set; }
        public string Process { get; internal set; }
        public string Area { get; internal set; }
        public int Block { get; internal set; }
    }
}
