using Manufactures.Domain.Estimations.Productions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Estimations.Productions.DataTransferObjects
{
    public class UpdateEstimatedProductionByIdDto : EstimatedProductionListDto
    {
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<UpdateEstimatedProductionDetailDto> EstimatedDetails { get; }

        public UpdateEstimatedProductionByIdDto(EstimatedProductionDocument estimatedDocument, string unitName) : base(estimatedDocument)
        {
            UnitName = unitName;
            EstimatedDetails = new List<UpdateEstimatedProductionDetailDto>();
        }
    }
}
