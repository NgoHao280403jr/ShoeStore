using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public Role? GetRoleById(long id)
        {
            return _roleRepository.GetRoleById(id);
        }

        public void AddRole(Role role)
        {
            _roleRepository.AddRole(role);
        }

        public void UpdateRole(Role role)
        {
            _roleRepository.UpdateRole(role);
        }

        public void DeleteRole(long id)
        {
            _roleRepository.DeleteRole(id);
        }
    }
}
