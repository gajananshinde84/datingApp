using datingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace datingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Value> Values { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Photo> Photos { get; set; }

    }
}