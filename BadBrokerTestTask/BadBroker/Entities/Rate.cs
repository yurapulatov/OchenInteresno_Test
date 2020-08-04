using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadBroker.Entities
{
    /// <summary>
    /// Caching rates class
    /// </summary>
    [Table("rate")]
    public class Rate
    {
        [Column ("id")]
        public long Id { get; set; }
        
        [Column ("base_currency_id")]
        [ForeignKey("BaseCurrency")]
        public int BaseCurrencyId { get; set; }
        
        
        public Currency BaseCurrency { get; set; }
        
        [Column ("result_currency_id")]
        [ForeignKey("ResultCurrency")]
        public int ResultCurrencyId { get; set; }
        
        public Currency ResultCurrency { get; set; }
        
        [Column ("rate_date")]
        public DateTime RateDate { get; set; }
        
        [Column ("rate_value")]
        public decimal RateValue { get; set; }
    }
}