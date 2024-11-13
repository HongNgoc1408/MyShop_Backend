using MyShop_Backend.DTO;
using MyShop_Backend.Response;

namespace MyShop_Backend.Services.Reviews
{
	public class ReviewService : IReviewService
	{
		public Task<PagedResponse<ReviewDTO>> GetAllReviewAsync(int page, int pageSize, string? keySearch)
		{
			throw new NotImplementedException();
		}
	}
}
