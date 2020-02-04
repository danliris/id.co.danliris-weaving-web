using Manufactures.Domain.Estimations.Productions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Application.Estimations.Productions.DataTransferObjects
{
    public class ViewEstimatedProductionByIdDto : EstimatedProductionListDto
    {
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<ViewEstimatedProductionDetailDto> EstimatedDetails { get; }

        public ViewEstimatedProductionByIdDto(EstimatedProductionDocument estimatedDocument, string unitName) : base(estimatedDocument)
        {
            UnitName = unitName;
            EstimatedDetails = new List<ViewEstimatedProductionDetailDto>();
        }
    }
}
