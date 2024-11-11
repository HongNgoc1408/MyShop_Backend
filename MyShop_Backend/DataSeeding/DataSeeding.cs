using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;

namespace MyShop_Backend.DataSeeding
{
	public class DataSeeding
	{
		public static async Task Initialize(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateAsyncScope();
			var context = scope.ServiceProvider.GetRequiredService<MyShopDbContext>();

			if (context != null)
			{
				try
				{
					if (context.Database.GetPendingMigrations().Any())
					{
						context.Database.Migrate();
					}
					await InitialRoles(scope.ServiceProvider, context);
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
		private static async Task InitialRoles(IServiceProvider serviceProvider, MyShopDbContext context)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			string[] roles = { "Admin" };

			foreach (string role in roles)
			{
				if (!context.Roles.Any(r => r.Name == role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
			await InitialUsers(serviceProvider, context, roles);
		}

		private static async Task InitialUsers(IServiceProvider serviceProvider, MyShopDbContext context, string[] roles)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
			var user = new User
			{
				FullName = "Hồng Ngọc",
				Email = "lengoc14082002@gmail.com",
				NormalizedEmail = "lengoc14082002@gmail.com",
				UserName = "lengoc14082002@gmail.com",
				NormalizedUserName = "lengoc14082002@gmail.com",
				PhoneNumber = "0946633248",
				//PhoneNumberConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			if (!context.Users.Any(u => u.UserName == user.UserName))
			{
				var result = await userManager.CreateAsync(user, "Ngoc123@");
				if (result.Succeeded)
				{
					await userManager.AddToRolesAsync(user, roles);
				}
			}
		}
	}
}
