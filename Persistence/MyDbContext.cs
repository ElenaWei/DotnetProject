using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Models;

namespace MyDotnetProject.Persistence
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<Make> Makes { get; set; }
    }
}