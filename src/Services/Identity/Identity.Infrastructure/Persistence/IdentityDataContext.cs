﻿using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedCommon.Domain;
using System.Reflection;

namespace Identity.Infrastructure.Persistence;
public class IdentityDataContext : DbContext
{
    #region DbSet

    public DbSet<UserAccount> UserAccounts { get; set; } = null!;
    public DbSet<VerifiedUrl> VerifiedUrls { get; set; } = null!;

    #endregion

    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(
        (builder) =>
        {
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                    .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Warning)
                    .AddConsole();
        });

    public IdentityDataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTableName(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase<ulong>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;

                default:
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private static void ConfigureTableName(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tblName = entityType.GetTableName();
            if (tblName is not null && tblName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tblName[6..]);
            }
        }
    }
}
