using System.Collections.Generic;
using Newtonsoft.Json;


namespace BadBroker.Entities.DTO
{
    public class RatesResponse
    {
        [JsonProperty (PropertyName = "rates")]
        public Dictionary<string, Dictionary<string, decimal>> RatesDictionary { get; set; }
        
        [JsonProperty (PropertyName = "base")]
        public string BaseCurrencyCode { get; set; }
        
        [JsonProperty (PropertyName = "start_at")]
        public string StartDate { get; set; }
        
        [JsonProperty (PropertyName = "end_at")]
        public string EndDate { get; set; }
    }
}