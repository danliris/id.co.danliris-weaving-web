using Newtonsoft.Json;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport
{
    public class WarpingProductionReportHeaderDto
    {
        [JsonProperty(PropertyName = "Group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        public WarpingProductionReportHeaderDto(string group, string name)
        {
            Group = group;
            Name = name;
        }
    }
}