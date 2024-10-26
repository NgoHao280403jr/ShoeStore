using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly QlShopBanGiayContext _context;

        public UserRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
