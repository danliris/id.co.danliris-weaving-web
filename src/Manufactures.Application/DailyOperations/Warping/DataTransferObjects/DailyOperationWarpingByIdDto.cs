using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingByIdDto : DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; private set; }

        [JsonProperty(PropertyName = "ColourOfCone")]
        public string ColourOfCone { get; private set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; private set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; private set; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; private set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public UnitId WeavingUnitId { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingBeamProducts")]
        public List<DailyOperationWarpingBeamProductDto> DailyOperationWarpingBeamProducts { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingDetails")]
        public List<DailyOperationWarpingDetailDto> DailyOperationWarpingDetails { get; private set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            AmountOfCones = document.AmountOfCones;
            ColourOfCone = document.ColourOfCone;
            DailyOperationWarpingBeamProducts = new List<DailyOperationWarpingBeamProductDto>();
            DailyOperationWarpingDetails = new List<DailyOperationWarpingDetailDto>();
        }

        public void SetWeavingUnit(UnitId value)
        {
            WeavingUnitId = value;
        }

        public void SetOperator (OperatorDocument value)
        {
            OperatorName = value.CoreAccount.Name;
            OperatorGroup = value.Group;
        }

        public void SetMaterialName(string value)
        {
            MaterialType = value;
        }

        public void AddDailyOperationBeamProducts(DailyOperationWarpingBeamProductDto value)
        {
            if (!DailyOperationWarpingBeamProducts.Any(product => product.Id.Equals(value.Id)))
            {
                DailyOperationWarpingBeamProducts.Add(value);
            }
        }

        public void AddDailyOperationWarpingHistories(DailyOperationWarpingDetailDto value)
        {
            DailyOperationWarpingDetails.Add(value);
        }
    }
}
