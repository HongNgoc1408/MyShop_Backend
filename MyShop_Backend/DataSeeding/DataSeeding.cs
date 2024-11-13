using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Enumerations;
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
				catch (Exception)
				{
					throw;
				}
			}
		}

		private static async Task InitialRoles(IServiceProvider serviceProvider, MyShopDbContext context)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

			foreach (string role in Enum.GetNames(typeof(RolesEnum)))
			{
				if (!context.Roles.Any(r => r.Name == role))
				{
					await roleManager.CreateAsync(new Role
					{
						Name = role,
						NormalizedName = role.ToUpper(),
					});
				}
			}
			await InitialUsers(serviceProvider, context);
		}

		private static async Task InitialUsers(IServiceProvider serviceProvider, MyShopDbContext context)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
			var adminEmail = "ngocb2005766@student.ctu.edu.vn";
			var admin = new User
			{
				FullName = "Hong Ngoc",
				Email = adminEmail,
				NormalizedEmail = adminEmail.ToUpper(),
				UserName = adminEmail,
				NormalizedUserName = adminEmail.ToUpper(),
				PhoneNumber = "0946633248",
				PhoneNumberConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var staffEmail = "lengoc14082002@gmail.com";
			var staff = new User
			{
				FullName = "Le Ngoc",
				Email = staffEmail,
				NormalizedEmail = staffEmail.ToUpper(),
				UserName = staffEmail,
				NormalizedUserName = staffEmail.ToUpper(),
				PhoneNumber = "0901089182",
				PhoneNumberConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var inventorierEmail = "minhnhat012340@gmail.com";
			var inventorier = new User
			{
				FullName = "Minh Nhật",
				Email = inventorierEmail,
				NormalizedEmail = inventorierEmail.ToUpper(),
				UserName = inventorierEmail,
				NormalizedUserName = inventorierEmail.ToUpper(),
				PhoneNumber = "0358103707",
				PhoneNumberConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			if (!context.Users.Any(u => u.UserName == admin.UserName))
			{
				var result = await userManager.CreateAsync(admin, "Ngoc123@");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, RolesEnum.Admin.ToString());
				}
			}
			if (!context.Users.Any(u => u.UserName == staff.UserName))
			{
				var result = await userManager.CreateAsync(staff, "Ngoc123@");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(staff, RolesEnum.Staff.ToString());
				}
			}
			if (!context.Users.Any(u => u.UserName == inventorier.UserName))
			{
				var result = await userManager.CreateAsync(inventorier, "Ngoc123@");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(inventorier, RolesEnum.Inventorier.ToString());
				}
			}

		}
	}
}
