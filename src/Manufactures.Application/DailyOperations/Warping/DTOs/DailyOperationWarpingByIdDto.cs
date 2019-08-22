using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.Operators;
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

        [JsonProperty(PropertyName = "DailyOperationBeamProducts")]
        public List<DailyOperationBeamProduct> DailyOperationBeamProducts { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationLoomHistories")]
        public List<DailyOperationLoomHistoryDto> DailyOperationLoomHistories { get; private set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            AmountOfCones = document.AmountOfCones;
            ColourOfCone = document.ColourOfCone;
            DailyOperationBeamProducts = new List<DailyOperationBeamProduct>();
            DailyOperationLoomHistories = new List<DailyOperationLoomHistoryDto>();
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

        public void AddDailyOperationLoomHistories(DailyOperationLoomHistoryDto value)
        {
            DailyOperationLoomHistories.Add(value);
        }
    }
}
