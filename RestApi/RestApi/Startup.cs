using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestApi.HubConfig;
using RestApi.Infrastructure.Mapper;
using RestApi.Persistence;

namespace RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });
            });

            ConfigureDbContext(services);

            services.AddSignalR();

            services.AddControllers()
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSingleton<IMappingCoordinator, MappingCoordinator>();

            services.AddScoped<IMatchRepository, SqlMatchRepository>();
        }

        public void ConfigureDbContext(IServiceCollection services)
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AppHub>("basketball");
            });
        }
    }
}