using System;
using sXb_service.Repos.Base;
using sXb_service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sXb_service.Repos.Interfaces
{
    public interface IUserRepo 
    {
        IEnumerable<User> GetAll();
        Task<User> Get(string id);
        IEnumerable<User> FindUsers(string keyword);
        int Update(User user, bool persists = true);
        string FindIdByName(string first, string last);
        string GetUsernameByEmail(string email);
        bool UsernameExists(string username);
        bool EmailExists(string email);
    }
}
