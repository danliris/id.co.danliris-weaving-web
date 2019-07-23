using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentsWeightDto
    {
        [JsonProperty(PropertyName = "Weight")]
        public double Netto { get; }

        [JsonProperty(PropertyName = "Bruto")]
        public double Bruto { get; }

        [JsonProperty(PropertyName = "Theoritical")]
        public double Theoritical { get; set; }

        public DailyOperationSizingBeamDocumentsWeightDto(double netto, double bruto, double theoritical)
        {
            Netto = netto;
            Bruto = bruto;
            Theoritical = theoritical;
        }
    }
}
