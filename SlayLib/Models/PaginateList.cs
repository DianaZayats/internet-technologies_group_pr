using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlayLib.Models
{
    public class PaginateList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginateList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginateList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await Task.Run(() => source.Count());
            var items = await Task.Run(() =>
                source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            );

            return new PaginateList<T>(items, count, pageIndex, pageSize);
        }
    }

}
