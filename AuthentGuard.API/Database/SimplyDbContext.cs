using AuthentGuard.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AuthentGuard.API.Database
{
    public class SimplyDbContext : DbContext
    {
        public SimplyDbContext(DbContextOptions<SimplyDbContext> options) : base(options)
        {
        }
        public DbSet<Register> RegisterModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the MySQL-specific configurations here if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}
