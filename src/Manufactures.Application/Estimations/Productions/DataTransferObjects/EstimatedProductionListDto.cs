using Manufactures.Domain.Estimations.Productions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Estimations.Productions.DataTransferObjects
{
    public class EstimatedProductionListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "EstimatedNumber")]
        public string EstimatedNumber { get; }

        [JsonProperty(PropertyName = "DateEstimated")]
        public DateTime DateEstimated { get; }

        public EstimatedProductionListDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimatedNumber = document.EstimatedNumber;
            DateEstimated = document.Period;
        }
    }
}
