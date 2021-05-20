using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestApi.Persistence;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestApi.Integration.Tests.Infrastructure
{
    public class BaseIntegrationTest
    {
        protected readonly HttpClient _client;

        public BaseIntegrationTest()
        {
            var factory = new CustomWebApplicationFactory()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(dbc => dbc.ServiceType == typeof(RestApiDBContext));
                        services.Remove(descriptor);
                        services.AddDbContext<RestApiDBContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    });
                });
            _client = factory.CreateClient();
            var requestHeaders = _client.DefaultRequestHeaders;

            if (requestHeaders.Accept == null || !requestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}