using BadBroker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadBroker.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Rate> Rates { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>(ConfigureRates);
            modelBuilder.Entity<Currency>(ConfigureCurrencies);
        }

        private void ConfigureRates(EntityTypeBuilder<Rate> modelBuilder)
        {
            modelBuilder.HasIndex(x => x.RateDate);
        }

        private void ConfigureCurrencies(EntityTypeBuilder<Currency> modelBuilder)
        {
            modelBuilder.HasAlternateKey(x => x.Code);
        }
    }
}