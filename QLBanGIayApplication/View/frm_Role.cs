using Microsoft.EntityFrameworkCore;
using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;

namespace QLBanGiay_Application.View
{
    public partial class frm_Role : Form
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly QlShopBanGiayContext _context;
        public frm_Role()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.Load += Frm_Role_Load;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.dgv_Roles.CellClick += Dgv_Roles_CellClick;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            _context = new QlShopBanGiayContext();
            _roleService = new RoleService(new RoleRepository(_context));
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main(_userService);
            mainForm.Show();
        }

        private void Dgv_Roles_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_Roles.Rows[e.RowIndex];

                txt_Mavaitro.Text = row.Cells["MãVaiTrò"].Value.ToString();
                txt_Tenvaitro.Text = row.Cells["TênVaiTrò"].Value.ToString();
            }
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            long roleId = Convert.ToInt64(txt_Mavaitro.Text); 
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa vai trò này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _roleService.DeleteRole(roleId);
                MessageBox.Show("Xóa vai trò thành công!");
                LoadRoles();
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (ValidateInput())
            {
                long roleId = Convert.ToInt64(txt_Mavaitro.Text);
                var role = _roleService.GetRoleById(roleId);

                if (role != null)
                {
                    role.Rolename = txt_Tenvaitro.Text;
                    _roleService.UpdateRole(role);
                    MessageBox.Show("Cập nhật vai trò thành công!");
                    LoadRoles();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vai trò để cập nhật!");
                }
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var newRole = new Role
                {
                    Rolename = txt_Tenvaitro.Text
                };

                _roleService.AddRole(newRole);
                MessageBox.Show("Thêm vai trò thành công!");
                LoadRoles();
            }
        }

        private void Frm_Role_Load(object? sender, EventArgs e)
        {
            LoadRoles();
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            string keyword = txt_Timkiem.Text;
            var roles = _roleService.GetAllRoles()
                                    .Where(r => r.Rolename.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    .ToList();

            dgv_Roles.DataSource = roles.Select(r => new
            {
                MãVaiTrò = r.Roleid,
                TênVaiTrò = r.Rolename
            }).ToList();
        }
        // Kiểm tra input
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txt_Tenvaitro.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò!");
                return false;
            }
            return true;
        }

        // Tải danh sách vai trò
        private void LoadRoles()
        {
            var roles = _roleService.GetAllRoles();
            dgv_Roles.DataSource = roles.Select(r => new
            {
                MãVaiTrò = r.Roleid,
                TênVaiTrò = r.Rolename
            }).ToList();
        }

    }
}
