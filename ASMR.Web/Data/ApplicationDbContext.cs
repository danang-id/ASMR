//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:11 AM
//
// ApplicationDbContext.cs
//

using Microsoft.EntityFrameworkCore;
using ASMR.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ASMR.Web.Data;

public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>, IDataProtectionKeyContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Bean> Beans { get; set; }
	public DbSet<BeanInventory> BeanInventories { get; set; }
	public DbSet<BusinessAnalytic> BusinessAnalytics { get; set; }
	public DbSet<Configuration> Configurations { get; set; }
	public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
	public DbSet<IncomingGreenBean> IncomingGreenBeans { get; set; }
	public DbSet<MediaFile> MediaFiles { get; set; }
	public DbSet<Packaging> Packagings { get; set; }
	public DbSet<PackagingResult> PackagingResults { get; set; }
	public DbSet<Payment> Payments { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Roasting> Roastings { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<TransactionItem> TransactionItems { get; set; }

	private void MarkEntities()
	{
		foreach (var entry in ChangeTracker.Entries())
		{
			var identifierProperty = entry.Metadata.FindProperty("Id");
			var createdAtProperty = entry.Metadata.FindProperty("CreatedAt");
			var lastUpdatedAtProperty = entry.Metadata.FindProperty("LastUpdatedAt");

			var hasIdentifierProperty = identifierProperty is not null &&
			                            identifierProperty.ClrType == typeof(string);
			var hasCreatedAtProperty = createdAtProperty is not null &&
			                           createdAtProperty.ClrType == typeof(DateTimeOffset);
			var hasLastUpdatedAtProperty = lastUpdatedAtProperty is not null &&
			                               lastUpdatedAtProperty.ClrType == typeof(DateTimeOffset?);

			switch (entry.State)
			{
				case EntityState.Added:

					if (hasIdentifierProperty)
					{
						if (entry.CurrentValues["Id"]?.ToString() == Guid.Empty.ToString())
						{
							entry.CurrentValues["Id"] = Guid.NewGuid().ToString();
						}
					}

					if (hasCreatedAtProperty)
					{
						entry.CurrentValues["CreatedAt"] = DateTimeOffset.Now;
					}

					if (hasLastUpdatedAtProperty)
					{
						entry.CurrentValues["LastUpdatedAt"] = null;
					}

					break;
				case EntityState.Modified:
					if (hasLastUpdatedAtProperty)
					{
						entry.CurrentValues["LastUpdatedAt"] = DateTimeOffset.Now;
					}

					break;
				case EntityState.Deleted:
					break;
				case EntityState.Detached:
					break;
				case EntityState.Unchanged:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public override int SaveChanges()
	{
		MarkEntities();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default(CancellationToken))
	{
		MarkEntities();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}
}