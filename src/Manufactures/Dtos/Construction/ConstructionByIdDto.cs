using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos.Construction
{
    public class ConstructionByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; }

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
        public IReadOnlyCollection<Warp> ItemsWarp { get; }

        [JsonProperty(PropertyName = "ItemsWeft")]
        public IReadOnlyCollection<Weft> ItemsWeft { get; }


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
            MaterialTypeDocument = document.MaterialType;


            var warps = new List<Warp>();
            var wefts = new List<Weft>();

            foreach (var constructionDetail in document.ConstructionDetails)
            {
                if (constructionDetail.Detail.Contains(Constants.WARP))
                {
                    var warpObj = new Warp(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);
                    warps.Add(warpObj);

                }
                else if (constructionDetail.Detail.Contains(Constants.WEFT))
                {
                    var weftObj = new Weft(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);

                    wefts.Add(weftObj);
                }
            }

            this.ItemsWarp = warps;
            this.ItemsWeft = wefts;
        }


    }
}
