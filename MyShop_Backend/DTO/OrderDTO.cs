using MyShop_Backend.Enumerations;
using MyShop_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.DTO
{
	public class OrderDTO
	{
		public long Id { get; set; }
		public double Total { get; set; }
		public double ShippingCost { get; set; }
		public DateTime OrderDate { get; set; }
		public double AmountPaid { get; set; }
		public string PaymentMethod { get; set; }
		public DeliveryStatusEnum OrderStatus { get; set; }
		//public string? PayBackUrl { get; set; }
	}
}
