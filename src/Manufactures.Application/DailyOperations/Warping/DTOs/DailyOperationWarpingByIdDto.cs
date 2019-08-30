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

        [JsonProperty(PropertyName = "MaterialName")]
        public string MaterialName { get; private set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; private set; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationBeamProducts")]
        public List<DailyOperationBeamProduct> DailyOperationBeamProducts { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingHistories")]
        public List<DailyOperationWarpingHistoryDto> DailyOperationWarpingHistories { get; private set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            AmountOfCones = document.AmountOfCones;
            ColourOfCone = document.ColourOfCone;
            DailyOperationBeamProducts = new List<DailyOperationBeamProduct>();
            DailyOperationWarpingHistories = new List<DailyOperationWarpingHistoryDto>();
        }

        public void SetWeavingUnit(UnitId value)
        {
            UnitId = value;
        }

        public void SetOperator (OperatorDocument value)
        {
            OperatorName = value.CoreAccount.Name;
            OperatorGroup = value.Group;
        }

        public void SetMaterialName(string value)
        {
            MaterialName = value;
        }

        public void AddDailyOperationBeamProducts(DailyOperationBeamProduct value)
        {
            if (!DailyOperationBeamProducts.Any(product => product.Id.Equals(value.Id)))
            {
                DailyOperationBeamProducts.Add(value);
            }
        }

        public void AddDailyOperationWarpingHistories(DailyOperationWarpingHistoryDto value)
        {
            DailyOperationWarpingHistories.Add(value);
        }
    }
}
