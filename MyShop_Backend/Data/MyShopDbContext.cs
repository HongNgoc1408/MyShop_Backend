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
		public DbSet<ProductModel>? Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<Image> Images { get; set; }
		#endregion
	}

}
