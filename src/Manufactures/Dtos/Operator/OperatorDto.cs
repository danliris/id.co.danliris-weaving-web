using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Operator
{
    public class OperatorDto
    {
        [JsonProperty(PropertyName = "CoreAccount")]
        public CoreAccount CoreAccount { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "Group")]
        public string Group { get; }

        [JsonProperty(PropertyName = "Assignment")]
        public string Assignment { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        public OperatorDto(OperatorDocument document)
        {
            CoreAccount = document.CoreAccount;
            UnitId = document.UnitId;
            Group = document.Group;
            Assignment = document.Assignment;
            Type = document.Type;
        }
    }
}
