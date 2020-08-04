using System;

namespace BadBroker.Entities
{
    /// <summary>
    /// Caching rates class
    /// </summary>
    public class Rates
    {
        public long Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public Сurrency BaseCurrency { get; set; }
        public int ResultCurrencyId { get; set; }
        public Сurrency ResultCurrency { get; set; }
        public DateTime RateDate { get; set; }
        public decimal RateValue { get; set; }
    }
}