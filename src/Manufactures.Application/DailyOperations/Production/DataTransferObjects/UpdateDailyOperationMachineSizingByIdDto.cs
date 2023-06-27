using Manufactures.Domain.DailyOperations.Productions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Production.DataTransferObjects
{
    public class UpdateDailyOperationMachineSizingByIdDto : DailyOperationMachineSizingListDto
    {
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<UpdateDailyOperationMachineSizingDetailDto> EstimatedDetails { get; }

        public UpdateDailyOperationMachineSizingByIdDto(DailyOperationMachineSizingDocument estimatedDocument, string unitName) : base(estimatedDocument)
        {
            UnitName = unitName;
            EstimatedDetails = new List<UpdateDailyOperationMachineSizingDetailDto>();
        }
    }
}
