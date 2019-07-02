using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Dtos.FabricConstructions
{
    public class FabricConstructionByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "MaterialTypeName")]
        public string MaterialTypeName { get; }

        [JsonProperty(PropertyName = "WovenType")]
        public string WovenType { get; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; }

        [JsonProperty(PropertyName = "Width")]
        public int Width { get; }

        [JsonProperty(PropertyName = "WarpTypeForm")]
        public string WarpTypeForm { get; }

        [JsonProperty(PropertyName = "WeftTypeForm")]
        public string WeftTypeForm { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        [JsonProperty(PropertyName = "ReedSpace")]
        public int ReedSpace { get; private set; }

        [JsonProperty(PropertyName = "TotalEnds")]
        public int TotalEnds { get; private set; }

        [JsonProperty(PropertyName = "ItemsWarp")]
        public IReadOnlyCollection<Warp> ItemsWarp { get; private set; }

        [JsonProperty(PropertyName = "ItemsWeft")]
        public IReadOnlyCollection<Weft> ItemsWeft { get; private set; }


        public FabricConstructionByIdDto(FabricConstructionDocument document)
        {
            Id = document.Identity;
            ConstructionNumber = document.ConstructionNumber;
            AmountOfWarp = document.AmountOfWarp;
            AmountOfWeft = document.AmountOfWeft;
            Width = document.Width;
            WovenType = document.WovenType;
            WarpTypeForm = document.WarpType;
            WeftTypeForm = document.WeftType;
            TotalYarn = document.TotalYarn;
            MaterialTypeName = document.MaterialTypeName;
            ReedSpace = document.ReedSpace;
            TotalEnds = document.TotalEnds;
            
            ItemsWarp = new List<Warp>();
            ItemsWeft = new List<Weft>();
        }

        public void AddWarp(Warp value)
        {
            var list = ItemsWarp.ToList();
            list.Add(value);
            ItemsWarp = list;
        }

        public void AddWeft(Weft value)
        {
            var list = ItemsWeft.ToList();
            list.Add(value);
            ItemsWeft = list;
        }

    }
}
