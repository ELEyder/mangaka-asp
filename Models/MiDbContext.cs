using Mangaka.Models;
using Microsoft.EntityFrameworkCore;
public class MiDbContext : DbContext
{
    public MiDbContext(DbContextOptions<MiDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding data
        modelBuilder.Entity<User>().HasData(
            new User { ID = 1, Name = "Eyder", Email = "eyder@gmail.com", Password = "1234", DateCreate = DateTime.Now, Status = true },
            new User { ID = 2, Name = "Juan", Email = "juan@gmail.com", Password = "5678", DateCreate = DateTime.Now, Status = true }
        );

        base.OnModelCreating(modelBuilder);
    }
}