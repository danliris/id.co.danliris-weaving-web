using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weaving.Domain.ValueObjects
{
    public class YarnCodes : ValueObject
    {
        public YarnCodes(List<string> codes)
        {
            Codes = codes;
        }

        public List<string> Codes { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Codes;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
