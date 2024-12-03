using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(long userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(long userId);
        User GetUserByUsernameAndPassword(string username, string password);
        IEnumerable<User> GetUsersByRole(long roleId);
    }
}
