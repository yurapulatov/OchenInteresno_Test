using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadBroker.Entities;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BadBroker.Data.Repositories
{
    public class RatesRepository : IRatesRepository
    {
        private readonly ApplicationContext _appContext;

        public RatesRepository(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<IEnumerable<Rate>> GetRatesInfoAsync(RateFilterModel filterModel)
        {
            var resultCurrencyIds = filterModel.ResultCurrencyList.Select(x => x.Id);
            return await _appContext.Rates.Where(x => x.BaseCurrencyId == filterModel.BaseCurrency.Id
                                                      && resultCurrencyIds.Contains(x.ResultCurrencyId)
                                                      && x.RateDate >= filterModel.DateFrom
                                                      && x.RateDate <= filterModel.DateTo).ToListAsync();
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencyAsync()
        {
            return await _appContext.Currencies.ToListAsync();
        }

        public async Task AddRangeRatesInfoAsync(IEnumerable<Rate> newCachingRates)
        {
            await _appContext.Rates.AddRangeAsync(newCachingRates);
            await _appContext.SaveChangesAsync();
        }

        public async Task<bool> CheckAvailableBaseCurrencyAsync(int currencyId)
        {
            return await _appContext.Currencies.AnyAsync(x => x.Id == currencyId && x.AccessBase);
        }

        public async Task<bool> CheckAvailableResultCurrenciesAsync(IEnumerable<int> currencyIds)
        {
            var count = await _appContext.Currencies.CountAsync(x => currencyIds.Contains(x.Id) && x.AccessResult);
            return count == currencyIds.Count();
        }
    }
}