using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using sXb_service.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Repos.Base
{
    public abstract class BaseRepo<T> : IRepo<T>, IDisposable where T : new()
    {
        protected TxtXContext _db;
        private bool _disposed = false;

        public BaseRepo()
        {
            _db = new TxtXContext();
        }

        public BaseRepo(TxtXContext context)
        {
            _db = context;
        }

        public BaseRepo(DbContextOptions options)
        {
            _db = new TxtXContext(options);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if(disposing)
            {
                //Free any other managed objects here
            }
            _db.Dispose();
            _disposed = true;
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch(RetryLimitExceededException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public abstract Task<bool> Exist(Guid id);
        public abstract Task<T> Find(Guid id);
        public abstract IEnumerable<T> GetAll();
        public abstract Task<T> Remove(Guid id);
    }
}
