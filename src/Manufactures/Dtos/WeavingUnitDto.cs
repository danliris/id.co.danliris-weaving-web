using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
    public class WeavingUnitDto
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
