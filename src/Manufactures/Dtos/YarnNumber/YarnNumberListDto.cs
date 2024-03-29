﻿using Manufactures.Domain.YarnNumbers;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Manufactures.DataTransferObjects.YarnNumber
{
    public class YarnNumberListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; }

        [JsonProperty(PropertyName = "AdditionalNumber")]
        public string AdditionalNumber { get; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; }

        public YarnNumberListDto(YarnNumberDocument ringDocument)
        {
            Id = ringDocument.Identity;
            Code = ringDocument.Code;
            Number = ringDocument.Number;
            AdditionalNumber = ringDocument.AdditionalNumber;

            //if (ringDocument.AdditionalNumber != 0)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(ringDocument.Number.ToString());
            //    sb.Append("/");
            //    sb.Append(ringDocument.AdditionalNumber.ToString());

            //    Number =  sb.ToString();
            //} else
            //{
            //    Number = ringDocument.Number.ToString();
            //}

            RingType = ringDocument.RingType;
        }
    }
}
