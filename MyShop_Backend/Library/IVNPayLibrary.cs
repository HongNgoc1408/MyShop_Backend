using MyShop_Backend.ModelView;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Library
{
	public interface IVNPayLibrary
	{
		string VERSION { get; }
		string CreateRequestUrl(VNPay vnPAY, string baseUrl, string vnp_HashSecret);
		bool ValidateSignature(VNPayRequest request, string vnp_SecureHash, string vnp_HashSecret);
		bool ValidateQueryDrSignature(VNPayQueryDrResponse response, string vnp_SecureHash, string vnp_HashSecret);
		string CreateSecureHashQueryDr(VNPayQueryDr queryDr, string vnp_HashSecret);
	}
}
