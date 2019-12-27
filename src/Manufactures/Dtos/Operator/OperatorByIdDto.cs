using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.DataTransferObjects.Operator
{
    public class OperatorByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "CoreAccount")]
        public CoreAccountDto CoreAccount { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "Group")]
        public string Group { get; }

        [JsonProperty(PropertyName = "Assignment")]
        public string Assignment { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        public OperatorByIdDto(OperatorDocument document)
        {
            Id = document.Identity;
            CoreAccount = new CoreAccountDto(document.CoreAccount);
            UnitId = document.UnitId;
            Group = document.Group;
            Assignment = document.Assignment;
            Type = document.Type;
        }
    }
}
