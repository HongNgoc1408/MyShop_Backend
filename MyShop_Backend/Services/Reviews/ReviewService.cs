using MyShop_Backend.ErroMessage;
using MyShop_Backend.Repositories.ProductReviewRepositories;
using MyShop_Backend.Request;
using MyStore.Repository.ProductRepository;

namespace MyShop_Backend.Services.Reviews
{
	public class ReviewService : IReviewService
	{
		private readonly IProductReviewRepository _productReviewRepository;
		private readonly IProductRepository _productRepository;

		public ReviewService(IProductReviewRepository productReviewRepository, IProductRepository productRepository) {
			_productReviewRepository = productReviewRepository;
			_productRepository = productRepository;
		}

		public async Task DeleteReview(string reviewId)
		{
			var review = await _productReviewRepository.FindAsync(reviewId);
			if (review != null)
			{
				var product = await _productRepository.FindAsync(review.ProductId);
				if (product != null)
				{
					//var currentStart = product.Rating * product.RatingCount;
					//product.Rating = (currentStart - review.Star) / (product.RatingCount - 1);
					//product.RatingCount -= 1;

					var currentStart = product.Rating * product.RatingCount;
					if (product.RatingCount > 1)
					{
						product.Rating = (currentStart - review.Star) / (product.RatingCount - 1);
					}
					else
					{
						product.Rating = 0;  
					}
					product.RatingCount -= 1;

					await _productRepository.UpdateAsync(product);
					//await _productRepository.UpdateAsync(product);
				}
				await _productReviewRepository.DeleteAsync(review);
			}
			else throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
		}

		public async Task<bool> UpdateEnable(string reviewId, UpdateEnableRequest request)
		{
			var review = await _productReviewRepository.FindAsync(reviewId);
			if (review != null)
			{	
				var product = await _productRepository.FindAsync(review.ProductId);
				if (product != null)
				{
					//var currentStart = product.Rating * product.RatingCount;
					//product.Rating = (currentStart - review.Star) / (product.RatingCount - 1);
					//product.RatingCount -= 1;


					//await _productRepository.UpdateAsync(product);
					var currentStart = product.Rating * product.RatingCount;
					if (product.RatingCount > 1)
					{
						product.Rating = (currentStart - review.Star) / (product.RatingCount - 1);
					}
					else
					{
						product.Rating = 0;
					}
					product.RatingCount -= 1;

					await _productRepository.UpdateAsync(product);

				}
				review.Enable = request.Enable;
				await _productReviewRepository.UpdateAsync(review);
				return review.Enable;
			}
			else throw new InvalidOperationException(ErrorMessage.NOT_FOUND);
			
		}
	}
}
