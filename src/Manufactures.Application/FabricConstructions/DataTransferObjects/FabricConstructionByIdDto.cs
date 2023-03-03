using Manufactures.Domain.FabricConstructions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Application.FabricConstructions.DataTransferObjects
{
    public class FabricConstructionByIdDto : FabricConstructionListDto
    {
        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; }

        [JsonProperty(PropertyName = "WovenType")]
        public string WovenType { get; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public double AmountOfWarp { get; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public double AmountOfWeft { get; }

        [JsonProperty(PropertyName = "Width")]
        public double Width { get; }

        [JsonProperty(PropertyName = "WarpType")]
        public string WarpType { get; }

        [JsonProperty(PropertyName = "WeftType")]
        public string WeftType { get; }

        [JsonProperty(PropertyName = "ReedSpace")]
        public double ReedSpace { get; }

        [JsonProperty(PropertyName = "YarnStrandsAmount")]
        public double YarnStrandsAmount { get; }

        [JsonProperty(PropertyName = "ConstructionWarpsDetail")]
        public List<ConstructionYarnDetailDto> ConstructionWarpsDetail { get; }

        [JsonProperty(PropertyName = "ConstructionWeftsDetail")]
        public List<ConstructionYarnDetailDto> ConstructionWeftsDetail { get; }

        public FabricConstructionByIdDto(FabricConstructionDocument constructionDocument) : base(constructionDocument)
        {
            MaterialType = constructionDocument.MaterialType;
            WovenType = constructionDocument.WovenType;
            AmountOfWarp = constructionDocument.AmountOfWarp;
            AmountOfWeft = constructionDocument.AmountOfWeft;
            Width = constructionDocument.Width;
            WarpType = constructionDocument.WarpType;
            WeftType = constructionDocument.WeftType;
            ReedSpace = constructionDocument.ReedSpace;
            YarnStrandsAmount = constructionDocument.YarnStrandsAmount;
            ConstructionWarpsDetail = new List<ConstructionYarnDetailDto>();
            ConstructionWeftsDetail = new List<ConstructionYarnDetailDto>();
        }

        public void AddConstructionWarpsDetail(ConstructionYarnDetailDto detail)
        {
            ConstructionWarpsDetail.Add(detail);
        }

        public void AddConstructionWeftsDetail(ConstructionYarnDetailDto detail)
        {
            ConstructionWeftsDetail.Add(detail);
        }
    }
}
