using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Defects.FabricDefect.ReadModels
{
    public class FabricDefectReadModel : ReadModelBase
    {
        public FabricDefectReadModel(Guid identity) : base(identity)
        {
        }
        public string DefectCode { get; internal set; }
        public string DefectType { get; internal set; }
        public string DefectCategory { get; internal set; }
    }
}
