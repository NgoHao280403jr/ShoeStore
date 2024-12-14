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
        private readonly UserService _userService;
        private readonly QlShopBanGiayContext _context;
        private readonly ProductSizeService _productSizeService;
        private readonly ProductService _productService;
        private ProductSize _lastSelectedRow;
        public frm_ProductSize(UserService userService)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += Frm_ProductSize_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.dgv_Sizes.SelectionChanged += Dgv_Sizes_SelectionChanged;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            this.txt_Sosize.KeyPress += Txt_Sosize_KeyPress;
            this.txt_Soluong.KeyPress += Txt_Soluong_KeyPress;
            _context = new QlShopBanGiayContext();
            _productSizeService = new ProductSizeService(new ProductSizeRepository(_context));  
            _productService = new ProductService(new ProductRepository(_context));
            _userService = userService;
        }

        private void Txt_Soluong_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true; 
            }
        }

        private void Txt_Sosize_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8) 
            {
                e.Handled = true;  
            }
        }

        private void Dgv_Sizes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgv_Sizes.SelectedRows.Count > 0)
            {
                var selectedRow = dgv_Sizes.SelectedRows[0].DataBoundItem as ProductSize;

                if (selectedRow != null && selectedRow != _lastSelectedRow)
                {
                    txt_Masize.Text = selectedRow.ProductSizeId.ToString();
                    cbo_Masp.SelectedValue = selectedRow.ProductId;  
                    txt_Sosize.Text = selectedRow.Size;
                    txt_Soluong.Text = selectedRow.Quantity.ToString();
                    _lastSelectedRow = selectedRow;
                }
            }
            else
            {
                txt_Masize.Clear();
                txt_Sosize.Clear();
                txt_Soluong.Clear();
                cbo_Masp.SelectedIndex = -1;
            }
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main(_userService);
            mainForm.Show();
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
                int productId = Convert.ToInt32(cbo_Masp.SelectedValue);
                string size = txt_Sosize.Text;
                int quantity = Convert.ToInt32(txt_Soluong.Text);

                // Kiểm tra xem sản phẩm với số size này đã tồn tại chưa
                var existingProductSize = _productSizeService.GetProductSizesByProductIdAndSize(productId, size);

                if (existingProductSize != null)
                {
                    // Nếu sản phẩm và size đã tồn tại, cộng thêm số lượng
                    existingProductSize.Quantity += quantity;
                    _productSizeService.UpdateProductSize(existingProductSize);
                    MessageBox.Show("Cập nhật số lượng kích thước sản phẩm thành công!");
                }
                else
                {
                    // Nếu chưa tồn tại, thêm mới sản phẩm
                    var productSize = new ProductSize
                    {
                        ProductId = productId,
                        Size = size,
                        Quantity = quantity
                    };

                    _productSizeService.AddProductSize(productSize);
                    MessageBox.Show("Thêm kích thước sản phẩm mới thành công!");
                }

                LoadProductSizes();  // Cập nhật lại danh sách kích thước sản phẩm
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

            if (!int.TryParse(txt_Sosize.Text, out int size) || size < 39 || size > 45)
            {
                MessageBox.Show("Kích thước phải nằm trong khoảng từ 39 đến 45!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadProductsIntoComboBox()
        {
            var products = _productService.GetAllProducts();

            if (products != null && products.Any())
            {
                cbo_Masp.DataSource = products;
                cbo_Masp.DisplayMember = "ProductName";
                cbo_Masp.ValueMember = "ProductId";
                cbo_Masp.DropDownHeight = 200; 
            }
        }

        private void Frm_ProductSize_Load(object? sender, EventArgs e)
        {
            LoadProductsIntoComboBox();
            LoadProductSizes();
        }


        private void LoadProductSizes()
        {
            var productSizes = _productSizeService.GetAllProductSizes();

            if (productSizes != null && productSizes.Any())
            {
                BindingSource bindingSource = new BindingSource { DataSource = productSizes };
                dgv_Sizes.DataSource = bindingSource;

                dgv_Sizes.Columns.Clear();

                dgv_Sizes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductSizeId", 
                    HeaderText = "Mã Size",
                    Width = 100
                });
                dgv_Sizes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductId",
                    HeaderText = "Sản phẩm",
                    Width = 100
                });

                dgv_Sizes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Size", 
                    HeaderText = "Kích Thước",
                    Width = 150
                });

                dgv_Sizes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity", 
                    HeaderText = "Số Lượng",
                    Width = 100
                });

                dgv_Sizes.ColumnHeadersHeight = 30;
                dgv_Sizes.ReadOnly = true;
                dgv_Sizes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv_Sizes.AllowUserToAddRows = false;
                dgv_Sizes.AllowUserToDeleteRows = false;
            }
            else
            {
                dgv_Sizes.DataSource = null;
                MessageBox.Show("Không có kích thước nào để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
