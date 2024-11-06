using MyShop_Backend.Enumerations;
using MyShop_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop_Backend.DTO
{
	public class OrderDTO
	{
		public long Id { get; set; }
		public string Receiver { get; set; }
		public string DeliveryAddress { get; set; }
		public double Total { get; set; }
		public double ShippingCost { get; set; }
		public double AmountPaid { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime ReceivedDate { get; set; }
		public string PaymentMethodName { get; set; }
		public DeliveryStatusEnum OrderStatus { get; set; }

		public string? PayBackUrl { get; set; }
		public DateTime? Expected_delivery_time { get; set; }
		
		public string? ShippingCode { get; set; }

		public ProductDTO Product { get; set; }
		public bool Reviewed { get; set; }
	}
}
