using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadBroker.Entities;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces;
using BadBroker.Interfaces.Repositories;
using BadBroker.Interfaces.Services;

namespace BadBroker.Services
{
    public class RatesService : IRatesService
    {
        private readonly IRatesRepository _ratesRepository;
        private readonly IExternalRatesService _externalRatesService;

        public RatesService(IExternalRatesService externalRatesService, IRatesRepository ratesRepository)
        {
            _externalRatesService = externalRatesService;
            _ratesRepository = ratesRepository;
        }

        public async Task<IEnumerable<Rate>> GetRateByFilterAsync(RateFilterModel filterModel)
        {
            //TODO: Add checking available rates
            var cachingRates = await _ratesRepository.GetRatesInfoAsync(filterModel);
            if (cachingRates.Count() == filterModel.ResultCurrencyList.Count() *
                filterModel.DateTo.Subtract(filterModel.DateFrom).Days)
            {
                return cachingRates;
            }
            else
            {
                var newRates = await GetMissingRates(filterModel, cachingRates);
                await _ratesRepository.AddRangeRatesInfoAsync(newRates);
                var result = cachingRates.ToList(); 
                result.AddRange(newRates);
                return result.GroupBy(x => new {x.ResultCurrencyId, x.RateDate}).Select(x => x.First());
            }
            
        }

        private IEnumerable<string> GetPartiallyMissingResultCurrencyCodes(IEnumerable<IGrouping<int, Rate>> groupingCachingRates, 
            DateTime dateFrom, DateTime dateTo)
        {
            var result = new List<string>();
            foreach (var group in groupingCachingRates)
            {
                if (group.Count() == dateFrom.Subtract(dateTo).Days)
                {
                    result.Add(group.First().ResultCurrency.Code);
                }
            }

            return result;
        }

        private Tuple<DateTime, DateTime> GetMinMaxMissingIntervalByCachingItems(IEnumerable<Rate> cachingRates)
        {
            var orderedListDate = cachingRates.Select(x => x.RateDate).OrderBy(x => x);
            DateTime minDateTime = DateTime.Today;
            DateTime maxDateTime = DateTime.Today;
            var tempDateTime = orderedListDate.First();
            foreach (var date in orderedListDate)
            {
                if (date.Subtract(tempDateTime).Days > 1)
                {
                    minDateTime = date;
                    break;
                }
                tempDateTime = date;
            }
            var orderedListDateDesc = cachingRates.Select(x => x.RateDate).OrderByDescending(x => x);
            tempDateTime = orderedListDateDesc.First();
            foreach (var date in orderedListDate)
            {
                if (tempDateTime.Subtract(date).Days > 1)
                {
                    maxDateTime = date;
                    break;
                }
                tempDateTime = date;
            }
            return new Tuple<DateTime, DateTime>(minDateTime, maxDateTime);
        }

        private IEnumerable<string> GetFullMissingResultCurrencyCodes(IEnumerable<string> existResultCurrency,
            IEnumerable<string> queryResultCurrency)
        {
            return queryResultCurrency.Where(x => existResultCurrency.All(y => y != x));
        }

        private async Task<IEnumerable<Rate>> GetMissingRates(RateFilterModel filterModel,
            IEnumerable<Rate> cachingRates)
        {
            var response = new RatesResponse(); 
            if (!cachingRates.Any())
            {
                response = await _externalRatesService.GetRatesAsync(filterModel.DateFrom,
                    filterModel.DateTo,
                    filterModel.BaseCurrency.Code,
                    filterModel.ResultCurrencyList.Select(x => x.Code));
            }
            else
            {
                var groupingCachingRates = cachingRates.GroupBy(x => x.BaseCurrencyId);
                var fullMissingResultCurrency =
                    GetFullMissingResultCurrencyCodes(groupingCachingRates.Select(x => x.First().ResultCurrency.Code), 
                        filterModel.ResultCurrencyList.Select(x => x.Code));
                var partiallyMissingResultCurrency =
                    GetPartiallyMissingResultCurrencyCodes(groupingCachingRates, filterModel.DateFrom, filterModel.DateTo);
                var minMaxUpdatingInterval = new Tuple<DateTime, DateTime>(DateTime.Today, DateTime.Today);
                if (!partiallyMissingResultCurrency.Any() && !fullMissingResultCurrency.Any())
                {
                    return new List<Rate>();
                }
            
                if (fullMissingResultCurrency.Any())
                {
                    minMaxUpdatingInterval = new Tuple<DateTime, DateTime>(filterModel.DateFrom, filterModel.DateTo);
                }
                else
                {
                    if (partiallyMissingResultCurrency.Any())
                    {
                        minMaxUpdatingInterval = GetMinMaxMissingIntervalByCachingItems(cachingRates);
                    }
                }
            
                var missingCurrencyCodes = new List<string>(fullMissingResultCurrency);
                missingCurrencyCodes.AddRange(partiallyMissingResultCurrency);
                response = await _externalRatesService.GetRatesAsync(minMaxUpdatingInterval.Item1, minMaxUpdatingInterval.Item2, 
                    filterModel.BaseCurrency.Code,
                    missingCurrencyCodes);
            }
            return await ConvertResponseToList(response);
        }

        private async Task<IEnumerable<Rate>> ConvertResponseToList(RatesResponse response)
        {
            var currenciesMap = (await _ratesRepository.GetAllCurrencyAsync()).ToDictionary(key => key.Code, value => value.Id);
            var results = response.RatesDictionary.Select(x => x.Value.Select(y => new Rate
            {
                BaseCurrencyId = currenciesMap[response.BaseCurrencyCode],
                ResultCurrencyId = currenciesMap[y.Key],
                RateDate = DateTime.Parse(x.Key),
                RateValue = y.Value
            }));
            var resultingRates = new List<Rate>();
            foreach (var result in results)
            {
                resultingRates.AddRange(result);
            }
            return resultingRates;
        }
    }
}