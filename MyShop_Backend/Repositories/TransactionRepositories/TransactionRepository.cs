using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;

namespace MyShop_Backend.Repositories.TransactionRepositories
{
	public class TransactionRepository(MyShopDbContext dbcontext) : ITransactionRepository
	{
		private readonly MyShopDbContext _dbContext = dbcontext;

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return await _dbContext.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await _dbContext.Database.CommitTransactionAsync();
		}

		public async Task RollbackTransactionAsync()
		{
			await _dbContext.Database.RollbackTransactionAsync();
		}
	}
}
