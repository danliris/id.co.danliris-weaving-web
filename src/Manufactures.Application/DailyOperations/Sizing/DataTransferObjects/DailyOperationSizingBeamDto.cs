using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingBeamDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "SizingLengthCounter")]
        public double SizingLengthCounter { get; set; }

        public DailyOperationSizingBeamDto(Guid id, double sizingLengthCounter)
        {
            Id = id;
            SizingLengthCounter = sizingLengthCounter;
        }

        public DailyOperationSizingBeamDto(DailyOperationSizingBeamDto sizingBeamDto)
        {
            Id = sizingBeamDto.Id;
            SizingLengthCounter = sizingBeamDto.SizingLengthCounter;
        }
    }
}
