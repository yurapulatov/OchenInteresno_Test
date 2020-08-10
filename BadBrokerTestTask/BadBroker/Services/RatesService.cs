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

        public async Task<IEnumerable<CalculatedRatesModel>> GetCalculatedRatesByFilterAsync(RateFilterModel filterModel)
        {
            var rates = await GetRateByFilterAsync(filterModel);
            var grouping = rates.GroupBy(x => x.ResultCurrencyId);
            var result = new List<CalculatedRatesModel>();
            foreach (var item in grouping)
            {
                var orderedRates = item.OrderBy(x => x.RateDate).ToList();
                decimal bestValue = 0;
                var bestStartDate = DateTime.Today;
                var bestEndDate = DateTime.Today;
                //Find optimal start/stop date for buy 
                for (var i = 0; i < orderedRates.Count() - 1; i++)
                {

                    for (var j = 1; j < orderedRates.Count(); j++)
                    {
                        var value =
                            (orderedRates[i].RateValue * filterModel.InputValueMoney / orderedRates[j].RateValue)
                            - (orderedRates[j].RateDate.Subtract(orderedRates[i].RateDate).Days) * 1/*TODO: Config Broker Price*/;
                        if (value > bestValue)
                        {
                            bestValue = value;
                            bestStartDate = orderedRates[i].RateDate;
                            bestEndDate = orderedRates[j].RateDate;
                        }
                    }
                }
                result.Add(new CalculatedRatesModel
                {
                    ResultCurrency = orderedRates.First().ResultCurrency,
                    BaseCurrency = orderedRates.First().BaseCurrency,
                    RatesInfoList = orderedRates.Select(x => new CalculatedRatesInfoModel
                    {
                        Date = x.RateDate,
                        Value = x.RateValue
                    }),
                    BestStartDate = bestStartDate,
                    BestEndingDate = bestEndDate,
                    MaxMoneyValue = bestValue
                });
            }

            return result;
        }

        public async Task<IEnumerable<Rate>> GetRateByFilterAsync(RateFilterModel filterModel)
        {
            var baseIsAvailable = await _ratesRepository.CheckAvailableBaseCurrencyAsync(filterModel.BaseCurrency.Id);
            var resultsIsAvailable =
                await _ratesRepository.CheckAvailableResultCurrenciesAsync(
                    filterModel.ResultCurrencyList.Select(x => x.Id));
            if (!baseIsAvailable || !resultsIsAvailable)
            {
                throw new ArgumentException("Invalid filter model. Check your currency accessing.");
            }
            var cachingRates = await _ratesRepository.GetRatesInfoAsync(filterModel);
            if (cachingRates.Count() == filterModel.ResultCurrencyList.Count() *
                filterModel.DateFrom.GetBusinessDaysCount(filterModel.DateTo))
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

        public async Task<IEnumerable<Currency>> GetCurrenciesList(string type)
        {
            return await _ratesRepository.GetCurrencyByTypeAsync(type);
        }

        private IEnumerable<string> GetPartiallyMissingResultCurrencyCodes(IEnumerable<IGrouping<int, Rate>> groupingCachingRates, 
            DateTime dateFrom, DateTime dateTo)
        {
            var result = new List<string>();
            foreach (var group in groupingCachingRates)
            {
                if (group.Count() != dateFrom.GetBusinessDaysCount(dateTo))
                {
                    result.Add(group.First().ResultCurrency.Code);
                }
            }

            return result;
        }

        private Tuple<DateTime, DateTime> GetMinMaxMissingIntervalByCachingItems(IEnumerable<Rate> cachingRates, DateTime startDate, DateTime endDate)
        {
            var ordered = cachingRates.OrderBy(x => x.RateDate);
            DateTime? minDateTime = null;
            DateTime? maxDateTime = null;
            var intervalDayCount = endDate.Subtract(startDate).Days;
            for (var day = 0; day <= intervalDayCount; day++)
            {

                var searchDayByBegin = startDate.AddDays(day);
                var searchDayByEnding = endDate.AddDays(day * -1);
                if ((minDateTime != null && maxDateTime != null))
                {
                    break;
                }
                if (minDateTime == null && searchDayByBegin.DayOfWeek != DayOfWeek.Saturday && searchDayByBegin.DayOfWeek != DayOfWeek.Sunday && !cachingRates.Any(x => x.RateDate.Date == searchDayByBegin))
                {
                    minDateTime = searchDayByBegin;
                }
                if (maxDateTime == null && searchDayByEnding.DayOfWeek != DayOfWeek.Saturday && searchDayByEnding.DayOfWeek != DayOfWeek.Sunday && !cachingRates.Any(x => x.RateDate.Date == searchDayByEnding))
                {
                    maxDateTime = searchDayByEnding;
                }
            }

            if (!minDateTime.HasValue || !maxDateTime.HasValue)
            {
                throw new ArgumentException("Nothing to search. Min/Max interval not found");
            }
            return new Tuple<DateTime, DateTime>(minDateTime.Value, maxDateTime.Value);
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
                var groupingCachingRates = cachingRates.GroupBy(x => x.ResultCurrencyId);
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
                        minMaxUpdatingInterval = GetMinMaxMissingIntervalByCachingItems(cachingRates, filterModel.DateFrom, filterModel.DateTo);
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
            var currenciesMap = (await _ratesRepository.GetAllCurrencyAsync()).ToDictionary(key => key.Code, value => value);
            var results = response.RatesDictionary.Select(x => x.Value.Select(y => new Rate
            {
                BaseCurrencyId = currenciesMap[response.BaseCurrencyCode].Id,
                BaseCurrency = currenciesMap[response.BaseCurrencyCode],
                ResultCurrencyId = currenciesMap[y.Key].Id,
                ResultCurrency = currenciesMap[y.Key],
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