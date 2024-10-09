using Microsoft.EntityFrameworkCore.Storage;

namespace MyShop_Backend.Repositories.TransactionRepositories
{
	public interface ITransactionRepository
	{
		Task<IDbContextTransaction> BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}
