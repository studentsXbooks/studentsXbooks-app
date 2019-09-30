using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Helpers
{
    public class Paging<T>
    {
        public int Skip { get; }
        public int Take { get; }

        public Paging(int page, int take, IEnumerable<T> allData)
        {
            this.Skip = page * take - take;
            this.Take = take;
            this.Data = allData.Skip(this.Skip).Take(this.Take);
            this.TotalDataCount = allData.Count();
        }

        public Paging(int page, IEnumerable<T> allData) : this(page, 20, allData)
        {
        }

        public int TotalDataCount { get; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalDataCount / PageSize);

        public int PageSize => Take;

        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrev => CurrentPage > 1;

        public int CurrentPage => Skip < Take ? 1 : Skip / Take + 1;

        public IEnumerable<T> Data { get; }
    }
}
