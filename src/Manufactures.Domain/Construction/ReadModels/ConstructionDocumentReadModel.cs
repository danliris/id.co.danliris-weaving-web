using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Construction.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ReadModels
{
    public class ConstructionDocumentReadModel : ReadModelBase
    {
        public ConstructionDocumentReadModel(Guid identity) : base (identity)
        {

        }

        public string ConstructionNumber { get; internal set; }
        public int AmountOfWarp { get; internal set; }
        public int AmountOfWeft { get; internal set; }
        public int Width { get; internal set; }
        public string WovenType { get; internal set; }
        public string WarpType { get; internal set; }
        public string WeftType { get; internal set; }
        public double TotalYarn { get; internal set; }
        public Guid MaterialType { get; internal set; }
        public List<ConstructionDetail> Warps { get; internal set; }
        public List<ConstructionDetail> Wefts { get; internal set; }
    }
}
