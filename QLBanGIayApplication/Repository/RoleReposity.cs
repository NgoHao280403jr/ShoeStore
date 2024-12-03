using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly QlShopBanGiayContext _context;

        public RoleRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
        public IEnumerable<User> GetUsersByRole(long roleId)
        {
            return _context.Users.Where(u => u.Roleid == roleId).ToList();
        }
        public Role? GetRoleById(long id)
        {
            return _context.Roles.FirstOrDefault(r => r.Roleid == id);
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void UpdateRole(Role role)
        {
            var existingRole = _context.Roles.FirstOrDefault(r => r.Roleid == role.Roleid);
            if (existingRole != null)
            {
                existingRole.Rolename = role.Rolename;
                _context.SaveChanges();
            }
        }

        public void DeleteRole(long id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Roleid == id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        }
    }
}
