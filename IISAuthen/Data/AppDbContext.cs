using IISAuthen.Models;
using Microsoft.EntityFrameworkCore;

namespace IISAuthen.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }  // Define your DB sets here

    }
}