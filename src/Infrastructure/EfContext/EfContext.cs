using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PetProject.Domain.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace PetProject.Infrastructure.EfContext;

public class EfContext : DbContext
{
    public EfContext()
    {
    }

    public EfContext(DbContextOptions<EfContext> options) : base(options)
    {
    }
    
    public DbSet<Note> Notes { get; set; }
    public DbSet<Comment> Comment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EfContext>
{
    public EfContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
        
        optionsBuilder.UseNpgsql("Host=localhost;Port=5430;Database=postgres_db;Username=postgres_user;Password=postgres_password");
        
        return new EfContext(optionsBuilder.Options);
    }
}