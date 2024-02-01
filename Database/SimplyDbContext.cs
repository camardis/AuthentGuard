using AuthentGuard.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthentGuard.Database
{
    public class SimplyDbContext : DbContext
    {

        public SimplyDbContext(DbContextOptions<SimplyDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
        }
    }
}
