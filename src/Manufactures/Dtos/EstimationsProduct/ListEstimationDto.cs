using Manufactures.Domain.Estimations.Productions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.EstimationsProduct
{
    public class ListEstimationDto
    {
        public Guid Id { get; }
        public string EstimationNumber { get; }
        public string DateEstimated { get; }

        public ListEstimationDto(EstimatedProductionDocument document)
        {
            Id = document.Identity;
            EstimationNumber = document.EstimatedNumber;
            DateEstimated = document.Date.ToString("MMMM dd, yyyy");
        }
    }
}
