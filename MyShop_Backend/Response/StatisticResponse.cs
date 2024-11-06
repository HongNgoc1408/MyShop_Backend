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
		public IEnumerable<StatisticDTO> Spending { get; set; }
		public IEnumerable<StatisticDTO> Sales { get; set; }
		public double Total { get; set; }
	}
}
