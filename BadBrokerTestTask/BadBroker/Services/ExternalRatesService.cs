using System;
using System.Net.Http;
using System.Threading.Tasks;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces;
using Newtonsoft.Json;

namespace BadBroker.Services
{
    public class ExternalRatesService : IExternalRatesService
    {
        public async Task<RatesResponse> GetRatesAsync(DateTime dateFrom, DateTime dateTo)
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.exchangeratesapi.io")
            };
            var response = await httpClient.GetStringAsync($"/history?start_at={dateFrom:yyyy-MM-dd}&end_at={dateTo:yyyy-MM-dd}&base=USD&symbols=ILS,JPY");
            return JsonConvert.DeserializeObject<RatesResponse>(response);
        }
    }
}