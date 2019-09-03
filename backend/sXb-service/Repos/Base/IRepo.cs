using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sXb_service.Repos.Base
{
    public interface IRepo<T> where T : new()
    {
        Task<bool> Exist(Guid id);

        Task<T> Find(Guid id);

        IEnumerable<T> GetAll();

        Task<T> Remove(Guid id);
    }
}