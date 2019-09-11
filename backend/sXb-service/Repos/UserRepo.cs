using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sXb_service.EF;
using sXb_service.Repos.Base;
using sXb_service.Models;
using sXb_service.Repos.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace sXb_service.Repos
{
    public class UserRepo : IUserRepo
    {
        public readonly TxtXContext Db;
        public DbSet<User> Table { get; }

        public UserRepo(DbContextOptions<TxtXContext> options)
        {
            Db = new TxtXContext(options);
            Table = Db.Set<User>();
        }


        private bool _disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //Free any other managed objects here
            }
            Db.Dispose();
            _disposed = true;
        }


        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public IEnumerable<User> FindUsers(string keyword)
        {
            IEnumerable<User> results = Table
                .Where(e =>
                       e.FirstName.ToLower().Contains(keyword.ToLower()) ||
                       e.LastName.ToLower().Contains(keyword.ToLower()));

            List<User> users = new List<User>();

            foreach (User user in results) users.Add(Get(user.Id).Result);

            return users;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> results = Table.OrderBy(x => x.LastName);
            return results;
        }

        public async Task<User> Get(string id)
        {
            return await Table.FindAsync(id);
        }

        public int Update(User user, bool persist = true )
        {
            //user.ConcurrencyStamp = System.Guid.NewGuid().ToString();
            user.ConcurrencyStamp = Table.AsNoTracking()
                .Where(w => w.Id == user.Id)
                .FirstOrDefault().ConcurrencyStamp;
            Table.Update(user);
            return persist ? SaveChanges() : 0;
        }
        public string FindIdByName(string first, string last)
        {
            User user = Table.Where(e =>
                            e.FirstName.ToLower().Equals(first.ToLower()) ||
                            e.LastName.ToLower().Equals(last.ToLower())).First();
            return user.Id;
        }
        public string GetUsernameByEmail(string email)
        {
            User user = Table.Where(e =>
           e.Email.ToLower().Equals(email.ToLower())
           ).First();
            return user.UserName;
        }
    }
}