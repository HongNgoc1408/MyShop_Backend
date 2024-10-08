﻿using Microsoft.EntityFrameworkCore;
using MyShop_Backend.Data;

namespace MyShop_Backend.Repositories.CommonRepositories
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

		public virtual async Task DeleteAsync(T entity)
		{
			_context.Remove(entity);
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

		public async Task<T?> FindAsync(params object?[]? keyValues)
		{
			return await _context.FindAsync<T>(keyValues);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_context.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}