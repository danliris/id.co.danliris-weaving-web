using Manufactures.Domain.DailyOperations.Productions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Production.DataTransferObjects
{
    public class DailyOperationMachineSizingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "EstimatedNumber")]
        public string EstimatedNumber { get; }

        [JsonProperty(PropertyName = "DateEstimated")]
        public DateTime DateEstimated { get; }

        public DailyOperationMachineSizingListDto(DailyOperationMachineSizingDocument document)
        {
            Id = document.Identity;
            EstimatedNumber = document.EstimatedNumber;
            DateEstimated = document.Period;
        }
    }
}
