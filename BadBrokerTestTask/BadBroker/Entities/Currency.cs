using System.ComponentModel.DataAnnotations.Schema;

namespace BadBroker.Entities
{
    [Table("currency")]
    public class Currency
    {
        [Column ("id")]
        public int Id { get; set; }
        [Column ("code")]
        public string Code { get; set; }
        
        [Column ("name")]
        public string Name { get; set; }
        
        [Column("access_base")]
        public bool AccessBase { get; set; }
        
        [Column("access_result")]
        public bool AccessResult { get; set; }
    }
}