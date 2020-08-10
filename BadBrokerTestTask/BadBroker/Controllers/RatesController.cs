using System;
using System.Threading.Tasks;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : Controller
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            _ratesService = ratesService;
        }

        [HttpPost]
        public async Task<IActionResult> GetRates([FromBody] RateFilterModel filterModel)
        {
            var result = await _ratesService.GetCalculatedRatesByFilterAsync(filterModel);
            return Ok(result);
        }

        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrency(string type)
        {
            var result = await _ratesService.GetCurrenciesList(type);
            return Ok(result);
        }
    }
}