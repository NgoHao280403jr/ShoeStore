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
using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay_Application.Services;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay_Application.View
{
    public partial class frm_Products : Form
    {
        private readonly CategoryService _categoryService;
        private readonly ParentService _parentService;
        private readonly ProductService _productService;
        private List<Product> categories;
        private readonly QlShopBanGiayContext _context;
        public frm_Products()
        {
            InitializeComponent();
            this.Load += Frm_Products_Load;

            _context = new QlShopBanGiayContext(); 
            _categoryService = new CategoryService(new CategoryRepository(_context));
            _parentService = new ParentService(new ParentCategoryRepository(_context));
            _productService = new ProductService(new ProductRepository(_context));
        }

        private void Frm_Products_Load(object? sender, EventArgs e)
        {
            LoadProducts();
        }

        private void dgv_danhsachdm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadProducts()
        {
            var products = _context.Products
                                   .Include(p => p.Category)
                                   .OrderBy(p => p.Productid)
                                   .ToList();

            if (products != null && products.Count > 0)
            {
                dgv_danhsachsp.DataSource = products;

                dgv_danhsachsp.AutoGenerateColumns = false;
                dgv_danhsachsp.Columns.Clear();

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Productid",
                    HeaderText = "Mã SP",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Productname",
                    HeaderText = "Tên SP",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Category.Categoryname",
                    HeaderText = "Danh Mục",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Price",
                    HeaderText = "Giá",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Discount",
                    HeaderText = "Giảm Giá (%)",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity",
                    HeaderText = "Số Lượng",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Ratingcount",
                    HeaderText = "Số Đánh Giá",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Productdescription",
                    HeaderText = "Mô Tả SP",
                    Width = 250,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Isactive",
                    HeaderText = "Hiển Thị",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.ColumnHeadersHeight = 30;
                dgv_danhsachsp.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachsp.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dgv_danhsachsp.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachsp.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachsp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachsp.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachsp.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có sản phẩm nào để hiển thị.");
            }
        }
    }
}
