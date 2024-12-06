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

        public bool ValidateLogin(string username, string password)
        {
            var user = _userRepository.GetUserByUsernameAndPassword(username, password);
            return user != null;
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
