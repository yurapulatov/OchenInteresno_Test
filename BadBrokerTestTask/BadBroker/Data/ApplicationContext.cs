using BadBroker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadBroker.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Currency> Currencies;
        public DbSet<Rate> Rates;
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>().HasIndex(x => x.RateDate);
        }
    }
}