using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingWeightDto
    {
        [JsonProperty(PropertyName = "Weight")]
        public double Netto { get; }

        [JsonProperty(PropertyName = "Bruto")]
        public double Bruto { get; }

        [JsonProperty(PropertyName = "Theoritical")]
        public double Theoritical { get; set; }

        public DailyOperationSizingWeightDto(double netto, double bruto, double theoritical)
        {
            Netto = netto;
            Bruto = bruto;
            Theoritical = theoritical;
        }
    }
}
