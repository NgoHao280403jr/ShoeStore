using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateLogin(string username, string password)
        {
            var user = _userRepository.GetUserByUsernameAndPassword(username, password);
            return user != null;
        }
        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }
    }
}
