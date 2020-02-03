using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.FabricConstructions.ReadModels
{
    public class FabricConstructionReadModel : ReadModelBase
    {
        public string ConstructionNumber { get; internal set; }
        public string MaterialType { get; internal set; }
        public string WovenType { get; internal set; }
        public int AmountOfWarp { get; internal set; }
        public int AmountOfWeft { get; internal set; }
        public int Width { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType { get; internal set; }
        public double ReedSpace { get; internal set; }
        public double YarnStrandsAmount { get; internal set; }
        public double TotalYarn { get; internal set; }

        public FabricConstructionReadModel(Guid identity) : base(identity) { }
    }
}
