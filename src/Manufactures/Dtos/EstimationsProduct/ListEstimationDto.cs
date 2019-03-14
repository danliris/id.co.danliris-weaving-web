using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.EstimationsProduct
{
    public class ListEstimationDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "EstimationNumber")]
        public string EstimationNumber { get; }

        [JsonProperty(PropertyName = "DateEstimated")]
        public string DateEstimated { get; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; }

        public ListEstimationDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimationNumber = document.EstimatedNumber;
            DateEstimated = document.Date.ToString("MMMM dd, yyyy");
            Period = document.Period;
        }
    }
}
