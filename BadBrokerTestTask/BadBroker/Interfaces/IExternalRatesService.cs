using System;
using System.Threading.Tasks;
using BadBroker.Entities.DTO;

namespace BadBroker.Interfaces
{
    public interface IExternalRatesService
    {
        Task<RatesResponse> GetRatesAsync(DateTime dateFrom, DateTime dateTo);
    }
}