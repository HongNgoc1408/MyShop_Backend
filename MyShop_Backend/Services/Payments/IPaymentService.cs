using MyShop_Backend.DTO;
using MyShop_Backend.ModelView;
using MyShop_Backend.Request;

namespace MyShop_Backend.Services.Payments
{
	public interface IPaymentService 
	{
		Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethods();
		Task<string?> IsActivePaymentMethod(int id);
		Task<PaymentMethodDTO> UpdatePaymentMethod(int id, UpdatePaymentMethodRequest request);
		Task<PaymentMethodDTO> CreatePaymentMethod(CreatePaymentMethodRequest request);
		Task DeletePaymentMethod(int id);
		string GetVNPayURL(VNPayOrderInfo order, string ipAddress, string? locale = null);
		Task VNPayCallback(VNPayRequest request);
	}
}
