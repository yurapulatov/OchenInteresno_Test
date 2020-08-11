using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces.Services;
using Newtonsoft.Json;

namespace BadBroker.Services
{
    public class ExternalRatesService : IExternalRatesService
    {
        public async Task<RatesResponse> GetRatesAsync(DateTime dateFrom, DateTime dateTo, string baseCurrencyCode, IEnumerable<string> listResultCurrencyCodes)
        {
            if (string.IsNullOrEmpty(nameof(baseCurrencyCode)))
            {
                throw new ArgumentNullException(baseCurrencyCode);
            }

            if (!listResultCurrencyCodes.Any())
            {
                throw new ArgumentNullException(nameof(listResultCurrencyCodes));
            }

            var resultCurrencyCodesString = listResultCurrencyCodes.
                Aggregate(string.Empty, (current, currencyCode) => current + (currencyCode + ","));
            //delete last ","
            resultCurrencyCodesString = resultCurrencyCodesString.Remove(resultCurrencyCodesString.Length - 1);

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.exchangeratesapi.io")
            };
            var response = await httpClient.GetStringAsync(
                $"/history?start_at={dateFrom:yyyy-MM-dd}&end_at={dateTo:yyyy-MM-dd}&base={baseCurrencyCode}&symbols={resultCurrencyCodesString}");
            return JsonConvert.DeserializeObject<RatesResponse>(response);
        }
    }
}