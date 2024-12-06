using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanGiay_Application.View
{
    public partial class frm_Main : Form
    {
        private bool isMenuOpen = false;
        public frm_Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.btn_Menu.Click += Btn_Menu_Click;
            this.btn_Qlsanpham.Click += Btn_Qlsanpham_Click;
            this.btn_Qldanhmuc.Click += Btn_Qldanhmuc_Click;
            this.btn_Qldanhmuc2.Click += Btn_Qldanhmuc2_Click;
            this.btn_Qlkhachhang.Click += Btn_Qlkhachhang_Click;
            this.btn_Qlnhanvien.Click += Btn_Qlnhanvien_Click;
            this.btn_Qlquyen.Click += Btn_Qlquyen_Click;
            this.btn_Qlnguoidung.Click += Btn_Qlnguoidung_Click;
            this.btn_Qlsize.Click += Btn_Qlsize_Click;
            this.btn_Qldonhang.Click += Btn_Qldonhang_Click;
            this.btn_Qlctdh.Click += Btn_Qlctdh_Click;
            this.btn_Qlhdbh.Click += Btn_Qlhdbh_Click;
            HideMenuButtons();
        }

        private void Btn_Qlhdbh_Click(object? sender, EventArgs e)
        {
            frm_Invoice mainForm = new frm_Invoice();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlctdh_Click(object? sender, EventArgs e)
        {
            frm_OrderDetail mainForm = new frm_OrderDetail();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qldonhang_Click(object? sender, EventArgs e)
        {
            frm_Orders mainForm = new frm_Orders();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlsize_Click(object? sender, EventArgs e)
        {
            frm_ProductSize mainForm = new frm_ProductSize();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlnguoidung_Click(object? sender, EventArgs e)
        {
            frm_Users mainForm = new frm_Users();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlquyen_Click(object? sender, EventArgs e)
        {
            frm_Role mainForm = new frm_Role();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlnhanvien_Click(object? sender, EventArgs e)
        {
            frm_Employee mainForm = new frm_Employee();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlkhachhang_Click(object? sender, EventArgs e)
        {
            frm_Customers mainForm = new frm_Customers();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qldanhmuc2_Click(object? sender, EventArgs e)
        {
            frm_ProductCategory mainForm = new frm_ProductCategory();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qldanhmuc_Click(object? sender, EventArgs e)
        {
            frm_ParentProduct mainForm = new frm_ParentProduct();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlsanpham_Click(object? sender, EventArgs e)
        {
            frm_Products mainForm = new frm_Products();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Menu_Click(object? sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                HideMenuButtons();
                isMenuOpen = false;
            }
            else
            {
                ShowMenuButtons();
                isMenuOpen = true;
            }
        }

        private void HideMenuButtons()
        {
            btn_Qldanhmuc.Visible = false;
            btn_Qldanhmuc2.Visible = false;
            btn_Qlsanpham.Visible = false;
            btn_Qlkhachhang.Visible = false;
            btn_Qlnguoidung.Visible = false;
            btn_Qlquyen.Visible = false;
            btn_Qlnhanvien.Visible = false;
            btn_Qldonhang.Visible = false;
            btn_Qlctdh.Visible = false;
            btn_Qlhdbh.Visible = false;
            btn_Qlsize.Visible = false;
        }
        private void ShowMenuButtons()
        {
            btn_Qldanhmuc.Visible = true;
            btn_Qldanhmuc2.Visible = true;
            btn_Qlsanpham.Visible = true;
            btn_Qlkhachhang.Visible = true;
            btn_Qlnguoidung.Visible = true;
            btn_Qlquyen.Visible = true;
            btn_Qlnhanvien.Visible = true;
            btn_Qldonhang.Visible = true;
            btn_Qlctdh.Visible = true;
            btn_Qlhdbh.Visible = true;
            btn_Qlsize.Visible = true;
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {

        }
    }
}
