using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Persistence.EfCore.Internal;

namespace Identity.Infrastructure.Persistence;

public class IdentityDataContext : DbContext
{
	private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(
		builder =>
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

		optionsBuilder.UseLoggerFactory(loggerFactory)
			.EnableSensitiveDataLogging();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		ConfigureTableName(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default)
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

	#region DbSet

	public DbSet<UserAccount> UserAccounts { get; set; } = null!;
	public DbSet<VerifiedUrl> VerifiedUrls { get; set; } = null!;

	#endregion
}
