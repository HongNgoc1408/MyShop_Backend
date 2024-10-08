using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.CommonRepositories;

namespace MyShop_Backend.Repositories.SizeRepositories
{
	public class SizeRepository(MyShopDbContext context) : CommonRepository<Size>(context), ISizeRepository
	{
	}
}
