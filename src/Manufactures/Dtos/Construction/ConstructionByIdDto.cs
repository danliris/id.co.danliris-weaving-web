using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Dtos.Construction
{
    public class ConstructionByIdDto
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

        [JsonProperty(PropertyName = "ItemsWarp")]
        public IReadOnlyCollection<Warp> ItemsWarp { get; private set; }

        [JsonProperty(PropertyName = "ItemsWeft")]
        public IReadOnlyCollection<Weft> ItemsWeft { get; private set; }


        public ConstructionByIdDto(ConstructionDocument document)
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
