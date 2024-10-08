namespace MyShop_Backend.Services.PagedServices
{
	public static class PagedService
	{
		public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int currentPage, int pageSize)
			=> query.Skip((currentPage - 1) * pageSize).Take(pageSize);
	}
}
