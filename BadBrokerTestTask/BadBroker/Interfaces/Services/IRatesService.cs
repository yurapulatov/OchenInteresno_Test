using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadBroker.Entities;
using BadBroker.Entities.DTO;

namespace BadBroker.Interfaces.Services
{
    public interface IRatesService
    {
        Task<IEnumerable<Rate>> GetRateByFilterAsync(RateFilterModel filterModel);
    }
}