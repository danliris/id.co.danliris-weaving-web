using Manufactures.Domain.Defects.FabricDefect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Defects.FabricDefect.DataTransferObjects
{
    public class FabricDefectDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "DefectCode")]
        public string DefectCode { get; }

        [JsonProperty(propertyName: "DefectType")]
        public string DefectType { get; }

        [JsonProperty(propertyName: "DefectCategory")]
        public string DefectCategory { get; }

        public FabricDefectDto(FabricDefectDocument document)
        {
            Id = document.Identity;
            DefectCode = document.DefectCode;
            DefectType = document.DefectType;
            DefectCategory = document.DefectCategory;
        }
    }
}
