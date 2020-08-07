using System;
using System.Threading.Tasks;
using BadBroker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : Controller
    {
        private readonly IExternalRatesService _ratesService;

        public RatesController(IExternalRatesService ratesService)
        {
            _ratesService = ratesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var result = await _ratesService.GetRatesAsync(dateFrom, dateTo, null, null);
            return Ok(result);
        }
    }
}