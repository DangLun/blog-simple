using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Contract.Shared
{
    public class PagedList<T>
    {
        private PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            int take = pageSize ?? 5;
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * take).Take(take).ToListAsync();

            return new(items, pageNumber, take, totalCount);
        }
    }
}
