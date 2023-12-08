using Microsoft.EntityFrameworkCore;
using World.Api.Models;

namespace World.Api.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Country> Countries { get; set; }
    }
}
