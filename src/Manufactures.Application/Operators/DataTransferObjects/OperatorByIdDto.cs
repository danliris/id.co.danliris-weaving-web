using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Operators.DataTransferObjects
{
    public class OperatorByIdDto : OperatorListDto
    {
        [JsonProperty(PropertyName = "CoreAccount")]
        public CoreAccountDto CoreAccount { get; }

        public OperatorByIdDto(OperatorDocument document, string unitName) : base(document, unitName)
        {
            CoreAccount = new CoreAccountDto(document.CoreAccount);
        }
    }
}
