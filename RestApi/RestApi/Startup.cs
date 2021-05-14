using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestApi.Infrastructure.Mapper;
using RestApi.Persistence;
using System.Text.Json.Serialization;

namespace RestApi
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
            services.AddDbContext<RestApiDBContext>(opt =>
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                string dbPath = Configuration.GetValue<string>("DbPath");
                if (dbPath.Contains(":"))
                {
                    path = dbPath;
                }
                else
                {
                    path += "\\" + dbPath;
                }
                var connectionString = $"Data Source={path};";
                opt.UseSqlite(connectionString);
            });

            services.AddControllers()
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSingleton<IMappingCoordinator, MappingCoordinator>();

            services.AddScoped<IMatchRepository, SqlMatchRepository>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}