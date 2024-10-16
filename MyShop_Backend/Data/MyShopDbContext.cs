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
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Brand> Brands { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Image> Images { get; set; }
		public virtual DbSet<Size> Sizes { get; set; }
		public virtual DbSet<Product>? Products { get; set; }
		public virtual DbSet<ProductColor> ProductColors { get; set; }
		public virtual DbSet<ProductSize> ProductSizes { get; set; }
		public virtual DbSet<CartItem> CartItems { get; set; }
		public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<OrderDetail> OrderDetails { get; set; }
		public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
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
