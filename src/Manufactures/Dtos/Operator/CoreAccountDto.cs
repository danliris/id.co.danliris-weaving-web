using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.DataTransferObjects.Operator
{
    public class CoreAccountDto
    {
        [JsonProperty(PropertyName = "MongoId")]
        public string MongoId { get; }

        [JsonProperty(PropertyName = "Id")]
        public int Id { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        public CoreAccountDto(CoreAccount core)
        {
            MongoId = core.MongoId;
            Id = core.Id;
            Name = core.Name;
        }
    }
}
