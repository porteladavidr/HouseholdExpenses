using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Household.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Transactions)
            .WithOne(t => t.Person!)
            .HasForeignKey(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Person>().Property(p => p.Name).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Category>().Property(c => c.Description).HasMaxLength(400).IsRequired();
        modelBuilder.Entity<Transaction>().Property(t => t.Description).HasMaxLength(400).IsRequired();
        modelBuilder.Entity<Transaction>().Property(t => t.Value).HasPrecision(18, 2);
    }
}