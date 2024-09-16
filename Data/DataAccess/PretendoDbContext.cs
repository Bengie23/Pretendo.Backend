using Microsoft.EntityFrameworkCore;
using Pretendo.Backend.Data.Entities;

namespace Pretendo.Backend.Data.DataAccess
{
    public class PretendoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "PretendoDb");
        }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Entities.Pretendo> Pretendos { get; set; }
    }
}
