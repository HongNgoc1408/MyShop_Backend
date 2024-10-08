using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Models;

namespace MyShop_Backend.Data
{
	public class MyShopDbContext : IdentityDbContext
	{
		public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
		{

		}

		#region DbSet
		public DbSet<User> Users { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product>? Products { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Size> Size { get; set; }
		#endregion
	}

}
