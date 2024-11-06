
using Microsoft.AspNetCore.Identity;
using MyShop_Backend.CommonRepository.ProductRepository;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.ImportRepositories;
using MyShop_Backend.Repositories.OrderRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Response;
using MyShop_Backend.Services.Imports;
using MyShop_Backend.Services.Orders;
using MyStore.Repository.ProductRepository;

namespace MyShop_Backend.Services.Statistices
{
	public class StatisticService : IStatisticService
	{
		private readonly UserManager<User> _userManager;
		private readonly IImportRepository _importRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;
		

		public StatisticService(IImportRepository importRepository, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, UserManager<User> userManager) 
		{
			_importRepository = importRepository;
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_userRepository = userRepository;
			_userManager = userManager;
		}
		public async Task<int> GetCountImport()
		{
			var total = await _importRepository.CountAsync();
			return total;
		}
		public async Task<int> GetCountOrder()
		{
			var total = await _orderRepository.CountAsync();
			return total;
		}

		public async Task<int> GetCountOrderDone()
		{
			var total = await _orderRepository.CountAsync(e => e.OrderStatus == Enumerations.DeliveryStatusEnum.Received);
			return total;
		}

		public async Task<int> GetCountProduct()
		{
			var total = await _productRepository.CountAsync(e=> e.Enable);
			return total;
		}

		public async Task<int> GetCountUser()
		{
			var total = await _userRepository.CountAsync();
			return total;
		}

		public async Task<RevenueResponse> GetRevenue(int year, int? month)
		{
			var spending = await _importRepository.GetTotalSpendingByYear(year, month);
			var sales = await _orderRepository.GetTotalSoldByYear(year, month);

			return new RevenueResponse
			{
				Spending = spending,
				Sales = sales,
				Total = 0
			};
		}

		public async Task<StatisticDateResponse> GetTotalSold(DateTime dateFrom, DateTime dateTo)
		{
			var res = await _orderRepository.GetTotalSold(dateFrom, dateTo);
			return new StatisticDateResponse
			{
				StatisticDateDTO = res,
				Total = res.Sum(x => x.Statistic)
			};
		}

		public async Task<StatisticResponse> GetTotalSoldByYear(int year, int? month)
		{
			var res = await _orderRepository.GetTotalSoldByYear(year, month);
			return new StatisticResponse
			{
				StatisticDTO = res,
				Total = res.Sum(e => e.Statistic)
			};
		}

		public async Task<StatisticDateResponse> GetTotalSpending(DateTime dateFrom, DateTime dateTo)
		{
			var res = await _importRepository.GetTotalSpending(dateFrom, dateTo);
			return new StatisticDateResponse
			{
				StatisticDateDTO = res,
				Total = res.Sum(x => x.Statistic)
			};
		}

		public async Task<StatisticResponse> GetTotalSpendingByYear(int year, int? month)
		{
			var res = await _importRepository.GetTotalSpendingByYear(year, month);
			return new StatisticResponse
			{
				StatisticDTO = res,
				Total = res.Sum(x => x.Statistic)
			};
		}
	}
}
