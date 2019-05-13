using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Operator
{
    public class CoreAccountDto
    {
        [JsonProperty(PropertyName = "MongoId")]
        public string MongoId { get; }

        [JsonProperty(PropertyName = "Id")]
        public int Id { get; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; }

        public CoreAccountDto(OperatorDocument document)
        {
            MongoId = document.CoreAccount.MongoId;
            Id = document.CoreAccount.Id;
            Name = document.CoreAccount.Name;
        }
    }
}
