using sXb_service.Models;
using sXb_service.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Repos.Interfaces
{
    public interface IListingRepo : IRepo<Listing>
    {
        Task<Listing> Add(Listing listing);

        Task<Listing> Update(Listing listing);

        Task<IEnumerable<Listing>> ByUser(string userId);
    }
}
