using Manufactures.Domain.Estimations.Productions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Estimations.Productions.DataTransferObjects
{
    public class EstimatedProductionByIdDto : EstimatedProductionListDto
    {
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<EstimatedProductionDetailDto> EstimatedDetails { get; }

        public EstimatedProductionByIdDto(EstimatedProductionDocument estimatedDocument, string unitName) : base(estimatedDocument)
        {
            UnitName = unitName;
            EstimatedDetails = new List<EstimatedProductionDetailDto>();
        }
    }
}
