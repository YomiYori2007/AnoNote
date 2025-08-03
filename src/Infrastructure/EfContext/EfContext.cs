using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PetProject.Domain.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace PetProject.Infrastructure.EfContext;

public class EfContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly((typeof(EfContext).Assembly));
        
        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(p => p.Notes)
            .HasForeignKey(n => n.UserId);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.UserId);
        modelBuilder.Entity<Reply>()
            .HasOne(r => r.User)
            .WithMany(p => p.Replies)
            .HasForeignKey(r => r.UserId);
        
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