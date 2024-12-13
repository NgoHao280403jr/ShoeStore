using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository;

namespace QLBanGiay_Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //public bool ValidateLogin(string username, string password)
        //{
        //    var user = _userRepository.GetUserByUsernameAndPassword(username, password);
        //    return user != null;
        //}
        public bool ValidateLogin(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);

            if (user != null)
            {
                if (IsPasswordHashed(user.Password))
                {
                    string hashedPassword = HashPassword(password); // Băm mật khẩu nhập từ người dùng
                    if (user.Password == hashedPassword && (user.Roleid == 1 || user.Roleid == 2))
                    {
                        return true;
                    }
                }
                else
                {
                    // Nếu mật khẩu chưa băm, so sánh trực tiếp
                    if (user.Password == password && (user.Roleid == 1 || user.Roleid == 2)) 
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsPasswordHashed(string password)
        {
            return password.Length == 64;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(long userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(long userId)
        {
            _userRepository.DeleteUser(userId);
        }

        public IEnumerable<User> GetUsersByRole(long roleId)
        {
            return _userRepository.GetUsersByRole(roleId);  
        }
    }
}
