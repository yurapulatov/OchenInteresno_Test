using System;
using System.Collections.Generic;

namespace BadBroker.Entities.DTO
{
    public class RateFilterModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Currency BaseCurrency { get; set; }
        public decimal InputValueMoney { get; set; }
        public IEnumerable<Currency> ResultCurrencyList { get; set; }
    }
}