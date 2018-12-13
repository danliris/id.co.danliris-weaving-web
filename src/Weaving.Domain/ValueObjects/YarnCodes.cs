using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weaving.Domain.ValueObjects
{
    public class YarnCodes : List<string>
    {
        public YarnCodes(IEnumerable<string> items)
        {
            this.AddRange(items);
        }
    }
}
