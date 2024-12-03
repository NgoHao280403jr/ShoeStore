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
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay_Application.View
{
    public partial class frm_Users : Form
    {
        private readonly UserService _userService;
        private readonly QlShopBanGiayContext _context;
        private readonly RoleService _roleService;
        public frm_Users()
        {
            InitializeComponent();
            this.Load += Frm_Users_Load;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.dgv_danhsachnd.CellClick += Dgv_danhsachnd_CellClick;
            this.cbo_Mavaitro.SelectedIndexChanged += Cbo_Mavaitro_SelectedIndexChanged;
            this.ck_Hienthi1.CheckedChanged += Ck_Hienthi1_CheckedChanged;
            this.ckHienThi.CheckedChanged += CkHienThi_CheckedChanged;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            _context = new QlShopBanGiayContext();
            _userService = new UserService(new UserRepository(_context));
            _roleService = new RoleService(new RoleRepository(_context));

        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main();
            mainForm.Show();
        }

        private void Ck_Hienthi1_CheckedChanged(object? sender, EventArgs e)
        {
            

            if (ck_Hienthi1.Checked)
            {
                txt_Mkcu.PasswordChar = '\0';
            }
            else
            {
                txt_Mkcu.PasswordChar = '●';
            }
            txt_Mkcu.Refresh();
        }

        private void CkHienThi_CheckedChanged(object? sender, EventArgs e)
        {           
            if (ckHienThi.Checked)
            {
                txt_Mknew.PasswordChar = '\0';
                txt_Mknew1.PasswordChar = '\0';
            }
            else
            {
                txt_Mknew.PasswordChar = '●';
                txt_Mknew1.PasswordChar = '●';
            }
        }

        private void Dgv_danhsachnd_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_danhsachnd.Rows[e.RowIndex];

                // Lấy UserId từ dòng đã chọn
                txt_Manguoidung.Text = row.Cells["MãNgườiDùng"].Value.ToString();
                txt_Tennguoidung.Text = row.Cells["TênNgườiDùng"].Value.ToString();
                cbo_Mavaitro.SelectedValue = row.Cells["VaiTrò"].Value;
                ck_Hoatdong.Checked = Convert.ToBoolean(row.Cells["HoạtĐộng"].Value);
                ck_Bikhoa.Checked = Convert.ToBoolean(row.Cells["BịKhóa"].Value);
            }
        }

        private void Cbo_Mavaitro_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbo_Mavaitro.SelectedValue != null && long.TryParse(cbo_Mavaitro.SelectedValue.ToString(), out var roleId))
            {
                MessageBox.Show($"Bạn đã chọn vai trò có ID: {roleId}");
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            var users = _userService.GetAllUsers()
                                    .Where(u => u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    .ToList();

            if (!users.Any())
            {
                MessageBox.Show("Không tìm thấy người dùng nào!");
                return;
            }

            dgv_danhsachnd.DataSource = users.Select(u => new
            {
                MãNgườiDùng = u.Userid,
                TênNgườiDùng = u.Username,
                VaiTrò = u.Roleid,
                HoạtĐộng = u.Isactive,
                BịKhóa = u.Isbanned
            }).ToList();
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa!");
                return;
            }

            long userId = Convert.ToInt64(txt_Manguoidung.Text);
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _userService.DeleteUser(userId);
                MessageBox.Show("Xóa người dùng thành công!");
                LoadUsers();
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (ValidateInput(isUpdateOrDelete: true))
            {
                long userId = Convert.ToInt64(txt_Manguoidung.Text);
                var user = _userService.GetUserById(userId);

                if (user != null)
                {
                    user.Username = txt_Tennguoidung.Text;

                    if (!string.IsNullOrWhiteSpace(txt_Mknew.Text))
                    {
                        if (string.IsNullOrWhiteSpace(txt_Mkcu.Text))  
                        {
                            MessageBox.Show("Vui lòng nhập mật khẩu cũ!");
                            return;
                        }

                        if (txt_Mkcu.Text != user.Password)  
                        {
                            MessageBox.Show("Mật khẩu cũ không đúng!");
                            return;
                        }

                        if (txt_Mknew1.Text != txt_Mknew1.Text)  
                        {
                            MessageBox.Show("Mật khẩu mới không khớp!");
                            return;
                        }
                        user.Password = txt_Mknew.Text;  
                    }

                    user.Roleid = Convert.ToInt64(cbo_Mavaitro.SelectedValue);
                    user.Isactive = ck_Hoatdong.Checked;
                    user.Isbanned = ck_Bikhoa.Checked;

                    _userService.UpdateUser(user);
                    MessageBox.Show("Cập nhật người dùng thành công!");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng để cập nhật!");
                }
            }
        }

        private bool ValidateInput(bool isUpdateOrDelete = false)
        {
            if (isUpdateOrDelete && string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để thực hiện thao tác!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_Tennguoidung.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng!");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txt_Mknew.Text) && txt_Mknew.Text != txt_Mknew1.Text)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không khớp!");
                return false;
            }

            return true;
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var user = new User
                {
                    Username = txt_Tennguoidung.Text,
                    Password = txt_Mknew.Text,
                    Roleid = Convert.ToInt64(cbo_Mavaitro.SelectedValue),
                    Isactive = ck_Hoatdong.Checked,
                    Isbanned = ck_Bikhoa.Checked
                };

                _userService.AddUser(user);
                MessageBox.Show("Thêm người dùng thành công!");
                LoadUsers();
            }
        }

        private void Frm_Users_Load(object? sender, EventArgs e)
        {
            LoadUsers();
            LoadRoles();
        }
        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();

            if (users == null || !users.Any())
            {
                dgv_danhsachnd.DataSource = null;
                MessageBox.Show("Không có người dùng nào trong hệ thống!");
                return;
            }

            dgv_danhsachnd.DataSource = users.Select(u => new
            {
                MãNgườiDùng = u.Userid,
                TênNgườiDùng = u.Username,
                VaiTrò = u.Roleid,
                HoạtĐộng = u.Isactive,
                BịKhóa = u.Isbanned
            }).ToList();
        }
        private void LoadRoles()
        {
            var roles = _roleService.GetAllRoles(); 

            if (roles == null || !roles.Any())
            {
                MessageBox.Show("Không có vai trò nào trong hệ thống!");
                return;
            }

            cbo_Mavaitro.DataSource = roles;
            cbo_Mavaitro.DisplayMember = "RoleName";  
            cbo_Mavaitro.ValueMember = "Roleid";    

            cbo_Mavaitro.SelectedIndex = 0;
        }
    }
}
