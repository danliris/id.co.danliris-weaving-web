﻿using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Application.Operators.DataTransferObjects
{
    public class OperatorListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Username")]
        public string Username { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; }

        [JsonProperty(PropertyName = "Group")]
        public string Group { get; }

        [JsonProperty(PropertyName = "Assignment")]
        public string Assignment { get; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; }

        public OperatorListDto(OperatorDocument document, string unitName)
        {
            Id = document.Identity;
            Username = document.CoreAccount.Name;
            UnitId = document.UnitId;
            UnitName = unitName;
            Group = document.Group;
            Assignment = document.Assignment;
            Type = document.Type;
        }
    }
}
