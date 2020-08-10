using System;
using System.Collections.Generic;

namespace BadBroker.Entities.DTO
{
    public class CalculatedRatesModel
    {
        public Currency ResultCurrency { get; set; }
        public Currency BaseCurrency { get; set; }
        public IEnumerable<CalculatedRatesInfoModel> RatesInfoList { get; set; }
        public DateTime BestStartDate { get; set; }
        public DateTime BestEndingDate { get; set; }
        public decimal MaxMoneyValue { get; set; }
    }
}