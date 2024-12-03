using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        Role? GetRoleById(long id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(long id);
        IEnumerable<User> GetUsersByRole(long roleId);
    }
}
