using MyShop_Backend.DTO;
using MyShop_Backend.Request;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Reviews
{
	public interface IReviewService
	{
		Task<bool> UpdateEnable(string reviewId, UpdateEnableRequest request);
		Task DeleteReview(string reviewId);
	}
}
