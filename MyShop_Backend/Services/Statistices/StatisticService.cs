using Microsoft.AspNetCore.Identity;
using MyShop_Backend.Models;
using MyShop_Backend.Repositories.ImportRepositories;
using MyShop_Backend.Repositories.OrderRepositories;
using MyShop_Backend.Repositories.UserRepositories;
using MyShop_Backend.Response;
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

		public async Task<RevenueResponse> GetRevenueByYear(int year, int? month)
		{
			var spending = await _importRepository.GetTotalSpendingByYear(year, month);
			var sales = await _orderRepository.GetTotalSoldByYear(year, month);

			var SpendingStatistic = new StatisticResponse
			{
				StatisticDTO = spending,
				Total = spending.Sum(x => x.Statistic)
			};

			var SalesStatistic = new StatisticResponse
			{
				StatisticDTO = sales,
				Total = sales.Sum(x => x.Statistic)
			};

			return new RevenueResponse
			{
				Spending = SpendingStatistic,
				Sales = SalesStatistic,
				Total = SalesStatistic.Total - SpendingStatistic.Total
			};
		}
		public async Task<RevenueDateResponse> GetRevenue(DateTime dateFrom, DateTime dateTo)
		{
			var spending = await _importRepository.GetTotalSpending(dateFrom, dateTo);
			var sales = await _orderRepository.GetTotalSold(dateFrom, dateTo);

			var spendingData = new StatisticDateResponse
			{
				StatisticDateDTO = spending,
				Total = spending.Sum(e => e.Statistic)
			};

			var salesData = new StatisticDateResponse
			{
				StatisticDateDTO = sales,
				Total = sales.Sum(e => e.Statistic)
			};

			return new RevenueDateResponse
			{
				Spending = spendingData,
				Sales = salesData,
				Total = salesData.Total - spendingData.Total
			};
		}
	
		public async Task<RevenueResponse> GetProductRevenueByYear(long productId, int year, int? month)
		{
			var productSpending = await _importRepository.GetTotalProductSpendingByYear(productId, year, month);
			var productSales = await _orderRepository.GetTotalProductSalesByYear(productId, year, month);

			var spendingData = new StatisticResponse
			{
				StatisticDTO = productSpending,
				Total = productSpending.Sum(x => x.Statistic)
			};

			var salesData = new StatisticResponse
			{
				StatisticDTO = productSales,
				Total = productSales.Sum(x => x.Statistic)
			};

			return new RevenueResponse
			{
				Spending = spendingData,
				Sales = salesData,
				Total = salesData.Total - spendingData.Total
			};
		}

		public async Task<RevenueDateResponse> GetProductRevenue(long productId, DateTime dateFrom, DateTime dateTo)
		{
			var productSpending = await _importRepository.GetTotalProductSpending(productId, dateFrom, dateTo);
			var productSales = await _orderRepository.GetTotalProductSales(productId, dateFrom, dateTo);

			var spendingData = new StatisticDateResponse
			{
				StatisticDateDTO = productSpending,
				Total = productSpending.Sum(e => e.Statistic)
			};

			var salesData = new StatisticDateResponse
			{
				StatisticDateDTO = productSales,
				Total = productSales.Sum(e => e.Statistic)
			};

			return new RevenueDateResponse
			{
				Spending = spendingData,
				Sales = salesData,
				Total = salesData.Total - spendingData.Total
			};
		}
	}
}
