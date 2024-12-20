﻿
using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;
using MyShop_Backend.Repositories.CommonRepositories;
using MyShop_Backend.Services.PagedServices;
using System.Linq.Expressions;

namespace MyShop_Backend.CommonRepository.CommonRepository
{
	public class CommonRepository<T> : ICommonRepository<T> where T : class
	{
		private readonly MyShopDbContext _context;

		public CommonRepository(MyShopDbContext context) => _context = context;

		public virtual async Task AddAsync(T entity)
		{
			await _context.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task AddAsync(IEnumerable<T> entities)
		{
			await _context.AddRangeAsync(entities);
			await _context.SaveChangesAsync();
		}

		public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

		public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
		{
			return await _context.Set<T>().Where(expression).CountAsync();
		}

		public virtual async Task DeleteAsync(T entity)
		{
			_context.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(IEnumerable<T> entities)
		{
			_context.RemoveRange(entities);
			await _context.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(params object?[]? keyValues)
		{
			var entity = await _context.FindAsync<T>(keyValues);
			if (entity == null)
			{
				throw new ArgumentException($"Entity with specified keys not found.");
			}
			_context.Remove(entity);
			await _context.SaveChangesAsync();
		}
		public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
		{
			_context.RemoveRange(entities);
			await _context.SaveChangesAsync();
		}
		public async Task<T?> FindAsync(params object?[]? keyValues)
		{
			return await _context.FindAsync<T>(keyValues);
		}
		public async Task<T?> FindAsyncCart(Expression<Func<T, bool>> expression)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(expression);
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
		{
			return await _context.Set<T>().Where(expression).ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
		{
			return await _context.Set<T>().Paginate(page, pageSize).ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<T, bool>>? expression, Expression<Func<T, TKey>> orderBy)
		{
			return expression == null
				? await _context.Set<T>().OrderBy(orderBy).Paginate(page, pageSize).ToListAsync()
				: await _context.Set<T>().Where(expression).OrderBy(orderBy).Paginate(page, pageSize).ToListAsync();
		}

		//public virtual async Task<IEnumerable<T>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<T, bool>>? expression, Expression<Func<T, TKey>> orderByDesc)
		//{
		//	return expression == null
		//		? await _context.Set<T>().OrderByDescending(orderByDesc).Paginate(page, pageSize).ToArrayAsync()
		//		: await _context.Set<T>().Where(expression).OrderByDescending(orderByDesc).Paginate(page, pageSize).ToArrayAsync();
		//}

		public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
			=> await _context.Set<T>().SingleOrDefaultAsync(expression);

		public virtual async Task<T?> SingleAsync(Expression<Func<T, bool>> expression)
			=> await _context.Set<T>().SingleAsync(expression);

		public async Task UpdateAsync(T entity)
		{
			_context.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(IEnumerable<T> entities)
		{
			_context.UpdateRange(entities);
			await _context.SaveChangesAsync();
		}

		public virtual async Task<IEnumerable<T>> GetPagedOrderByDescendingAsync<TKey>(int page, int pageSize, Expression<Func<T, bool>>? expression, Expression<Func<T, TKey>> orderByDesc)
		{
			return expression == null
				? await _context.Set<T>()
				.OrderByDescending(orderByDesc)
				.Paginate(page, pageSize)
				.ToArrayAsync()
				: await _context.Set<T>()
				.Where(expression)
				.OrderByDescending(orderByDesc)
				.Paginate(page, pageSize)
				.ToArrayAsync();
		}
		
	}
}