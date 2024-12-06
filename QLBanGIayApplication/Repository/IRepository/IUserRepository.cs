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
        User GetUserByUsernameAndPassword(string username, string password);
        void AddUser(User user);
    }
}
