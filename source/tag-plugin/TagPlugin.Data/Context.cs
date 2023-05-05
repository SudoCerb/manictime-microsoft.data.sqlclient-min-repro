using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TagPlugin.Domain;

namespace TagPlugin.Data
{
    public class TagsPluginContext : DbContext
    {
        public DbSet<AppleCart> Carts { get; set; }

        public DbSet<Apple> Apples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                    "Data Source = localhost\\SQLEXPRESS; Initial Catalog = MT_test; Encrypt = yes; TrustServerCertificate = true; Integrated Security = true"
            );
        }
    }
}
