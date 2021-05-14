using Microsoft.EntityFrameworkCore;
using RestApi.Models;

namespace RestApi.Persistence
{
    public class RestApiDBContext : DbContext
    {
        public RestApiDBContext(DbContextOptions<RestApiDBContext> opt) : base(opt)
        {
        }

        public DbSet<Match> Matches { get; set; }
    }
}