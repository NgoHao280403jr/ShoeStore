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
        private string _imageFileName;
        private List<Product> categories;
        private readonly QlShopBanGiayContext _context;
        public frm_Products()
        {
            InitializeComponent();
            this.Load += Frm_Products_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.dgv_danhsachsp.CellClick += Dgv_danhsachsp_CellClick;
            this.btn_Taianh.Click += Btn_Taianh_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.cbo_Madmcha.SelectedIndexChanged += Cbo_Madmcha_SelectedIndexChanged;
            this.txt_Gia.KeyPress += Txt_Gia_KeyPress;
            this.txt_Gia.TextChanged += Txt_Gia_TextChanged;
            this.txt_Giamgia.KeyPress += Txt_Giamgia_KeyPress;

            _context = new QlShopBanGiayContext(); 
            _categoryService = new CategoryService(new CategoryRepository(_context));
            _parentService = new ParentService(new ParentCategoryRepository(_context));
            _productService = new ProductService(new ProductRepository(_context));
        }

        private void Txt_Soluong_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true; 
            }
        }

        private void Txt_Giamgia_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void Txt_Gia_TextChanged(object? sender, EventArgs e)
        {
            string input = txt_Gia.Text.Replace(",", "");

            if (decimal.TryParse(input, out decimal result))
            {
                // Định dạng giá trị với dấu phẩy phân cách hàng nghìn
                txt_Gia.Text = string.Format("{0:N0}", result);
                txt_Gia.SelectionStart = txt_Gia.Text.Length;
            }
            else
            {
                txt_Gia.Text = input;
            }
        }

        private void Txt_Gia_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
            {
                e.Handled = true; 
            }
        }

        private void Cbo_Madmcha_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbo_Madmcha.SelectedValue != null && cbo_Madmcha.SelectedValue is long parentCategoryId)
            {
                var subCategories = _categoryService.GetCategoriesByParentCategoryId(parentCategoryId);
                cbo_Madmcon.DataSource = subCategories;
                cbo_Madmcon.DisplayMember = "Categoryname";
                cbo_Madmcon.ValueMember = "Categoryid";
                cbo_Madmcon.Enabled = subCategories.Count > 0;
            }
            else
            {
                cbo_Madmcon.Enabled = false;
                cbo_Madmcon.DataSource = null;
            }
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Masp.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long productId = long.Parse(txt_Masp.Text);

            try
            {
                var existingProduct = _productService.GetProductById(productId); // Lấy sản phẩm theo ID
                if (existingProduct == null)
                {
                    MessageBox.Show("Sản phẩm không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _productService.DeleteProduct(productId);
                _context.SaveChanges();
                LoadProducts(); // Cập nhật lại danh sách sản phẩm
                MessageBox.Show("Xóa sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Tensp.Text) ||
            string.IsNullOrWhiteSpace(txt_Gia.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long productId = long.Parse(txt_Masp.Text);
            var existingProduct = _productService.GetProductById(productId); // Lấy sản phẩm theo ID

            if (existingProduct == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            existingProduct.Productname = txt_Tensp.Text.Trim();
            existingProduct.Price = decimal.TryParse(txt_Gia.Text, out var price) ? (long?)price : null;
            existingProduct.Discount = int.TryParse(txt_Giamgia.Text, out var discount) ? discount : (int?)null;
            existingProduct.Productdescription = txt_Mota.Text;
            existingProduct.Isactive = ck_Hienthi.Checked;
            existingProduct.Categoryid = (long?)cbo_Madmcon.SelectedValue;
            existingProduct.Parentcategoryid = (long?)cbo_Madmcha.SelectedValue;

            try
            {
                _productService.UpdateProduct(existingProduct);
                _context.SaveChanges();
                LoadProducts(); // Cập nhật lại danh sách sản phẩm
                MessageBox.Show("Cập nhật sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txt_Tensp.Text) ||
                string.IsNullOrWhiteSpace(txt_Gia.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newProduct = new Product
            {
                Productname = txt_Tensp.Text.Trim(),
                Price = decimal.TryParse(txt_Gia.Text, out var price) ? (long?)price : null, // Chuyển đổi rõ ràng
                Discount = int.TryParse(txt_Giamgia.Text, out var discount) ? discount : (int?)null, // Chuyển đổi sang int
                Productdescription = txt_Mota.Text,
                Isactive = ck_Hienthi.Checked,
                Categoryid = (long?)cbo_Madmcon.SelectedValue,
                Parentcategoryid = (long?)cbo_Madmcha.SelectedValue,
                Image = GetImageFileName() // Tạo phương thức để lấy tên file hình ảnh
            };

            // Kiểm tra tính hợp lệ của giá và số lượng
            if (newProduct.Price == null || newProduct.Price <= 0)
            {
                MessageBox.Show("Giá sản phẩm không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _productService.AddProduct(newProduct);
                _context.SaveChanges();
                LoadProducts();
                MessageBox.Show("Thêm sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetImageFileName()
        {
            return _imageFileName; // Trả về tên file hình ảnh
        }
        private void Btn_Taianh_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;
                    _imageFileName = Path.GetFileName(selectedImagePath); 

                    string destinationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img");

                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    string destinationPath = Path.Combine(destinationDirectory, _imageFileName);
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }

                    File.Copy(selectedImagePath, destinationPath, true);
                    Picture_SP.Image = Image.FromFile(destinationPath);
                }
            }
        }

        private void Dgv_danhsachsp_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedProduct = (Product)dgv_danhsachsp.Rows[e.RowIndex].DataBoundItem;

                // Cập nhật thông tin sản phẩm lên các textbox
                txt_Masp.Text = selectedProduct.Productid.ToString();
                txt_Tensp.Text = selectedProduct.Productname;
                txt_Gia.Text = selectedProduct.Price?.ToString("N0") ?? "0";
                txt_Giamgia.Text = selectedProduct.Discount?.ToString() ?? "0";
                txt_Mota.Text = selectedProduct.Productdescription ?? "";
                ck_Hienthi.Checked = selectedProduct.Isactive;

                //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", selectedProduct.Image);
                string imagePath = Path.Combine(@"D:\Dowload\ShoeStore-main\ShoeStore-main\img", selectedProduct.Image);
                if (!string.IsNullOrEmpty(selectedProduct.Image) && File.Exists(imagePath))
                {
                    Picture_SP.Image = Image.FromFile(imagePath);
                }
                else
                {
                    Picture_SP.Image = null;
                }

                if (selectedProduct.Parentcategoryid != null)
                {
                    cbo_Madmcha.SelectedValue = selectedProduct.Parentcategoryid;
                }
                else
                {
                    cbo_Madmcha.SelectedIndex = -1; 
                }

                if (selectedProduct.Parentcategoryid != null)
                {
                    long parentCategoryId = selectedProduct.Parentcategoryid.Value;

                    // Lọc danh mục con dựa trên danh mục cha
                    var subCategories = _categoryService.GetCategoriesByParentCategoryId(parentCategoryId);
                    cbo_Madmcon.DataSource = subCategories;
                    cbo_Madmcon.DisplayMember = "Categoryname";
                    cbo_Madmcon.ValueMember = "Categoryid";

                    if (selectedProduct.Categoryid != null)
                    {
                        cbo_Madmcon.SelectedValue = selectedProduct.Categoryid;
                    }
                    else
                    {
                        cbo_Madmcon.SelectedIndex = -1; 
                    }

                    cbo_Madmcon.Enabled = subCategories.Count > 0; 
                }
                else
                {
                    cbo_Madmcon.Enabled = false;
                    cbo_Madmcon.DataSource = null; 
                }
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            // Tìm các sản phẩm trong cơ sở dữ liệu, chuyển Productname về chữ thường
            var products = _context.Products
                                   .Include(p => p.Category)
                                   .Where(p => p.Productname.ToLower().Contains(keyword)) // Chuyển cả tên sản phẩm về chữ thường
                                   .OrderBy(p => p.Productid)
                                   .ToList();

            // Kiểm tra và hiển thị kết quả
            if (products != null && products.Count > 0)
            {
                dgv_danhsachsp.DataSource = products;
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào.");
            }
        }

        private void Frm_Products_Load(object? sender, EventArgs e)
        {
            LoadProducts();
            LoadParentCategories();
            LoadCategories();
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
                dgv_danhsachsp.ReadOnly = true;

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
                    DataPropertyName = "Parentcategoryid", 
                    HeaderText = "Danh Mục Cha",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Categoryid", 
                    HeaderText = "Danh Mục Con",
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

                //dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                //{
                //    DataPropertyName = "Ratingcount",
                //    HeaderText = "Số Đánh Giá",
                //    Width = 100,
                //    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                //});

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Productdescription",
                    HeaderText = "Mô Tả SP",
                    Width = 250,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachsp.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Image", 
                    HeaderText = "Hình Ảnh",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });


                dgv_danhsachsp.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Isactive",
                    HeaderText = "Hiển Thị",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachsp.ColumnHeadersHeight = 50;
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
        private void LoadParentCategories()
        {
            var parentCategories = _parentService.GetAllParentCategories();
            cbo_Madmcha.DataSource = parentCategories;
            cbo_Madmcha.DisplayMember = "Parentcategoryname";
            cbo_Madmcha.ValueMember = "Parentcategoryid";
            cbo_Madmcha.SelectedIndex = -1;
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetAllCategories();
            cbo_Madmcon.DataSource = categories;
            cbo_Madmcon.DisplayMember = "Categoryname";
            cbo_Madmcon.ValueMember = "Categoryid";
            cbo_Madmcon.SelectedIndex = -1;
        }
    }
}
