using Manufactures.Domain.DailyOperations.Productions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Application.DailyOperations.Production.DataTransferObjects
{
    public class ViewDailyOperationMachineSizingByIdDto : DailyOperationMachineSizingListDto
    {
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<ViewDailyOperationMachineSizingDetailDto> EstimatedDetails { get; }

        public ViewDailyOperationMachineSizingByIdDto(DailyOperationMachineSizingDocument estimatedDocument, string unitName) : base(estimatedDocument)
        {
            UnitName = unitName;
            EstimatedDetails = new List<ViewDailyOperationMachineSizingDetailDto>();
        }
    }
}
