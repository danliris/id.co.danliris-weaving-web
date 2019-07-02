using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.FabricConstructions.ReadModels
{
    public class FabricConstructionReadModel : ReadModelBase
    {
        public FabricConstructionReadModel(Guid identity) : base(identity) { }

        public string ConstructionNumber { get; internal set; }
        public int AmountOfWarp { get; internal set; }
        public int AmountOfWeft { get; internal set; }
        public int Width { get; internal set; }
        public string WovenType { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType { get; internal set; }
        public double TotalYarn { get; internal set; }
        public string MaterialTypeName { get; internal set; }
        public string ListOfWarp { get; set; }
        public string ListOfWeft { get; set; }
        public int? ReedSpace { get; internal set; }
        public int? TotalEnds { get; internal set; }
    }
}
