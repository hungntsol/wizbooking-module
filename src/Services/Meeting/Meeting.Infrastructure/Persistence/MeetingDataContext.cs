using Meeting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedCommon.Domain;
using System.Reflection;

namespace Meeting.Infrastructure.Persistence;
public class MeetingDataContext : DbContext
{
    #region dbset

    public DbSet<AppUser> AppUsers { get; set; } = null!;
    public DbSet<AppUserService> AppUserServices { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<ProvidedUrl> ProviderUrls { get; set; } = null!;

    #endregion

    private static readonly ILoggerFactory _loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        (builder) =>
        {
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                    .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Warning)
                    .AddConsole();
        });

    public MeetingDataContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLoggerFactory(_loggerFactory);
        //optionsBuilder.UseSqlServer(connection => connection.MigrationsAssembly("Meeting.Infrastructure"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTableName(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
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

    public DbContext GetContext()
    {
        return this;
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
