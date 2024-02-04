using AuthentGuard.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthentGuard.Database
{
    public class SimplyDbContext : DbContext
    {

        public SimplyDbContext(DbContextOptions<SimplyDbContext> options) : base(options)
        {
        }

        public DbSet<RegisterModel> RegisterModel { get; set; }

    }
}
