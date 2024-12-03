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
using Microsoft.EntityFrameworkCore;
using QLBanGiay_Application.Services;

namespace QLBanGiay_Application.View
{
    public partial class frm_ProductSize : Form
    {
        private readonly QlShopBanGiayContext _context;
        private readonly ProductSizeService _productSizeService;
        private readonly ProductService _productService;
        public frm_ProductSize()
        {
            InitializeComponent();
            this.Load += Frm_ProductSize_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.dgv_Sizes.CellClick += Dgv_Sizes_CellClick;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            _context = new QlShopBanGiayContext();
            _productSizeService = new ProductSizeService(new ProductSizeRepository(_context));  
            _productService = new ProductService(new ProductRepository(_context)); 
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main();
            mainForm.Show();
        }

        private void Dgv_Sizes_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgv_Sizes.Rows[e.RowIndex];
                int productSizeId = Convert.ToInt32(selectedRow.Cells["MãKíchThước"].Value);

                txt_Masize.Text = productSizeId.ToString();
                txt_Sosize.Text = selectedRow.Cells["KíchThước"].Value.ToString();
                txt_Soluong.Text = selectedRow.Cells["SốLượng"].Value.ToString();
            }
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Masize.Text))
            {
                MessageBox.Show("Vui lòng chọn kích thước sản phẩm để xóa!");
                return;
            }

            int productSizeId = Convert.ToInt32(txt_Masize.Text);
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa kích thước sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _productSizeService.DeleteProductSize(productSizeId);
                MessageBox.Show("Xóa kích thước sản phẩm thành công!");
                LoadProductSizes();
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (ValidateInput(isUpdateOrDelete: true))
            {
                int productSizeId = Convert.ToInt32(txt_Masize.Text);
                var productSize = _productSizeService.GetProductSizeById(productSizeId);

                if (productSize != null)
                {
                    productSize.Size = txt_Sosize.Text;
                    productSize.Quantity = Convert.ToInt32(txt_Soluong.Text);

                    _productSizeService.UpdateProductSize(productSize);
                    MessageBox.Show("Cập nhật kích thước sản phẩm thành công!");
                    LoadProductSizes();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kích thước sản phẩm để cập nhật!");
                }
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var productSize = new ProductSize
                {
                    ProductId = Convert.ToInt32(cbo_Masp.SelectedValue),
                    Size = txt_Sosize.Text,
                    Quantity = Convert.ToInt32(txt_Soluong.Text)
                };

                _productSizeService.AddProductSize(productSize);
                MessageBox.Show("Thêm kích thước sản phẩm thành công!");
                LoadProductSizes();
            }
        }

        private bool ValidateInput(bool isUpdateOrDelete = false)
        {
            if (isUpdateOrDelete && string.IsNullOrWhiteSpace(txt_Masize.Text))
            {
                MessageBox.Show("Vui lòng chọn kích thước sản phẩm để thực hiện thao tác!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_Sosize.Text))
            {
                MessageBox.Show("Vui lòng nhập kích thước sản phẩm!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_Soluong.Text) || !int.TryParse(txt_Soluong.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!");
                return false;
            }

            return true;
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            string searchText = txt_Timkiem.Text.ToLower();

            var filteredProducts = _productService.GetAllProducts()
                .Where(p => p.Productname.ToLower().Contains(searchText))
                .ToList();

            if (filteredProducts.Any())
            {
                cbo_Masp.DataSource = filteredProducts;
                cbo_Masp.DisplayMember = "ProductName"; 
                cbo_Masp.ValueMember = "ProductId";   
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào!");
            }
        }

        private void Frm_ProductSize_Load(object? sender, EventArgs e)
        {
            LoadProductsIntoComboBox();
            LoadProductSizes();
        }

        private void LoadProductsIntoComboBox()
        {
            var products = _productService.GetAllProducts(); 

            if (products != null && products.Any())
            {
                cbo_Masp.DataSource = products;
                cbo_Masp.DisplayMember = "ProductName";
                cbo_Masp.ValueMember = "ProductId"; 
            }
        }
        private void LoadProductSizes()
        {
            var productSizes = _productSizeService.GetAllProductSizes();

            if (productSizes == null || !productSizes.Any())
            {
                dgv_Sizes.DataSource = null;
                MessageBox.Show("Không có kích thước sản phẩm nào trong hệ thống!");
                return;
            }

            var productSizeList = productSizes.Select(ps => new
            {
                MãKíchThước = ps.ProductSizeId,
                KíchThước = ps.Size,
                SốLượng = ps.Quantity
            }).ToList();

            dgv_Sizes.DataSource = productSizeList;

            dgv_Sizes.Columns[0].HeaderText = "Mã Kích Thước";
            dgv_Sizes.Columns[1].HeaderText = "Kích Thước";
            dgv_Sizes.Columns[2].HeaderText = "Số Lượng";
        }

    }
}
