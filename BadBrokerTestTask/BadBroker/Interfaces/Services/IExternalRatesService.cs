using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadBroker.Entities.DTO;

namespace BadBroker.Interfaces.Services
{
    public interface IExternalRatesService
    {
        Task<RatesResponse> GetRatesAsync(DateTime dateFrom, DateTime dateTo, string baseCurrencyCode,
            IEnumerable<string> listResultCurrencyCodes);
    }
}