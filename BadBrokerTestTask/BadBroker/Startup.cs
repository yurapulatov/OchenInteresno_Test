using BadBroker.Data;
using BadBroker.Data.Repositories;
using BadBroker.Entities.DTO;
using BadBroker.Interfaces;
using BadBroker.Interfaces.Repositories;
using BadBroker.Interfaces.Services;
using BadBroker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BadBroker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IExternalRatesService, ExternalRatesService>();
            services.AddScoped<IRatesRepository, RatesRepository>();
            services.AddScoped<IRatesService, RatesService>();
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("Application")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}