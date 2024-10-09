using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyShop_Backend.Models;

namespace MyShop_Backend.Data
{
	public class MyShopDbContext : IdentityDbContext<User>
	{
		public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
		{

		}

		#region DbSet
		public DbSet<User> Users { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
		
		public DbSet<Image> Images { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<Product>? Products { get; set; }
		public virtual DbSet<ProductColor> ProductColors { get; set; }
		public virtual DbSet<ProductSize> ProductSizes { get; set; }
		#endregion

		private void UpdateTimestamps()
		{
			var entries = ChangeTracker.Entries()
				.Where(e => e.Entity is IBaseEntity
				&& (e.State == EntityState.Added || e.State == EntityState.Modified));
			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					((IBaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
				}
				if (entry.State == EntityState.Modified)
				{
					((IBaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
				}
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			UpdateTimestamps();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			UpdateTimestamps();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
	
}
