using MyShop_Backend.DTO;

namespace MyShop_Backend.Response
{

	public class StatisticResponse
	{
		public IEnumerable<StatisticDTO> StatisticDTO { get; set; }
		public double Total { get; set; }
	}
	public class StatisticDateResponse
	{
		public IEnumerable<StatisticDateDTO> StatisticDateDTO { get; set; }
		public double Total { get; set; }
	}
	public class RevenueResponse
	{
		public StatisticResponse Spending { get; set; }
		public StatisticResponse Sales { get; set; }
		public double Total { get; set; }
	}
	
	public class RevenueDateResponse
	{
		public StatisticDateResponse Spending { get; set; }
		public StatisticDateResponse Sales { get; set; }
		public double Total { get; set; }
	}
	public class StatisticProductReponse
	{
		public IEnumerable<StatisticDTO> StatisticProduct { get; set; }
		public double Total { get; set; }
	}
}
