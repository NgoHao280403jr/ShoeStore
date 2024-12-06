using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(long userId)
        {
            return _context.Users.FirstOrDefault(u => u.Userid == userId);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.Userid);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.Roleid = user.Roleid;
                existingUser.Isactive = user.Isactive;
                existingUser.Isbanned = user.Isbanned;
                _context.SaveChanges();
            }
        }

        public void DeleteUser(long userId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public IEnumerable<User> GetUsersByRole(long roleId)
        {
            return _context.Users.Where(u => u.Roleid == roleId).ToList();
        }

    }
}
