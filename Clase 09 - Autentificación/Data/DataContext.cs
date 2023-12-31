using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UserItem> Users { get; set; }
        public DbSet<SessionItem> Sessions { get; set; }
    }
}
