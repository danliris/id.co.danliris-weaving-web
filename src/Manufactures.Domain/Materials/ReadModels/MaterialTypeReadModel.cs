using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Materials.ReadModels
{
    public class MaterialTypeReadModel : ReadModelBase
    {
        public MaterialTypeReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }
}
