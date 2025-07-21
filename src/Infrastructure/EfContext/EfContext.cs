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
    public DbSet<Reply> Reply { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Note
        modelBuilder.Entity<Note>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<Note>().Property(x => x.Title).HasMaxLength(60);
        
        modelBuilder.Entity<Note>().Property(x => x.Author).IsRequired();
        modelBuilder.Entity<Note>().Property(x => x.Author).HasMaxLength(16);
        
        modelBuilder.Entity<Note>().Property(x => x.Text).IsRequired();
        modelBuilder.Entity<Note>().Property(x => x.Text).HasMaxLength(500);
        // Comment
        modelBuilder.Entity<Comment>().Property(x => x.Author).IsRequired();
        modelBuilder.Entity<Comment>().Property(x => x.Author).HasMaxLength(16);
        
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).IsRequired();
        modelBuilder.Entity<Comment>().Property(x => x.CommentText).HasMaxLength(500);
        // Replies
        modelBuilder.Entity<Reply>().Property(x => x.Author).IsRequired();
        modelBuilder.Entity<Reply>().Property(x => x.Author).HasMaxLength(16);
        
        modelBuilder.Entity<Reply>().Property(x => x.CommentText).IsRequired();
        modelBuilder.Entity<Reply>().Property(x => x.CommentText).HasMaxLength(500);
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