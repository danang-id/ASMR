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

namespace ASMR.Web.Data
{
    public class ApplicationDbContext : DbContext, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bean> Beans { get; set; }
        public DbSet<BeanInventory> BeanInventories { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<IncomingGreenBean> IncomingGreenBeans { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Packaging> Packagings { get; set; }
        public DbSet<PackagingResult> PackagingResults { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RoastedBeanProduction> RoastedBeanProductions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        private void MarkEntities()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entityType = entry.Entity.GetType();
                if (entityType == typeof(DataProtectionKey))
                {
                    continue;
                }
                
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.CurrentValues["Id"].ToString() == Guid.Empty.ToString())
                        {
                            entry.CurrentValues["Id"] = Guid.NewGuid().ToString();
                        }
                        entry.CurrentValues["CreatedAt"] = DateTimeOffset.Now;
                        entry.CurrentValues["LastUpdatedAt"] = null;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["LastUpdatedAt"] = DateTimeOffset.Now;
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
}
