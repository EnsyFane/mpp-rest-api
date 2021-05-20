using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Persistence;
using System.Linq;

namespace RestApi.Integration.Tests.Infrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RestApiDBContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<RestApiDBContext>(options =>
                    options.UseInMemoryDatabase("TestDB"));
            });

            base.ConfigureWebHost(builder);
        }
    }
}