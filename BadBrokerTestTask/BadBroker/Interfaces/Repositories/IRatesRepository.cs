using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadBroker.Entities;
using BadBroker.Entities.DTO;

namespace BadBroker.Interfaces.Repositories
{
    public interface IRatesRepository
    {
        /// <summary>
        /// Get caching rates in DB.
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns>Filtering rates</returns>
        Task<IEnumerable<Rate>> GetRatesInfoAsync(RateFilterModel filterModel);

        Task<IEnumerable<Currency>> GetAllCurrencyAsync();

        /// <summary>
        /// Adding to DB new rates
        /// </summary>
        /// <param name="newCachingRates"></param>
        /// <returns></returns>
        Task AddRangeRatesInfoAsync(IEnumerable<Rate> newCachingRates);
    }
}