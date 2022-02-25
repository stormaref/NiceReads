using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NiceReads.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace NiceReads.Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany()
                .HasForeignKey(a => a.AuthorId);

            builder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId);

            base.OnModelCreating(builder);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
    }

    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("InMemory");
            return new DatabaseContext(builder.Options);
        }
    }
}

