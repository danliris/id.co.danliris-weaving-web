using Manufactures.Domain.Defects.YarnDefect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Defects.YarnDefect.DataTransferObjects
{
    public class YarnDefectDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "DefectCode")]
        public string DefectCode { get; }

        [JsonProperty(propertyName: "DefectType")]
        public string DefectType { get; }

        [JsonProperty(propertyName: "DefectCategory")]
        public string DefectCategory { get; }

        public YarnDefectDto(YarnDefectDocument document)
        {
            Id = document.Identity;
            DefectCode = document.DefectCode;
            DefectType = document.DefectType;
            DefectCategory = document.DefectCategory;
        }
    }
}
