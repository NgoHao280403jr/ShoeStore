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

namespace QLBanGiay_Application.View
{
    public partial class frm_ProductCategory : Form
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly ParentService _parentService;
        private readonly ProductService _productService;
        private List<Productcategory> categories;
        private readonly QlShopBanGiayContext _context;

        public frm_ProductCategory()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += Frm_ProductCategory_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.dgv_danhsachdm.SelectionChanged += Dgv_danhsachdm_SelectionChanged;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.btn_Thoat.Click += Btn_Thoat_Click;

            _context = new QlShopBanGiayContext(); 
            _categoryService = new CategoryService(new CategoryRepository(_context));
            _productService = new ProductService(new ProductRepository(_context));
            _parentService = new ParentService(new ParentCategoryRepository(_context));
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main(_userService);
            mainForm.Show();
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (long.TryParse(txt_Madm.Text, out long categoryId))
            {
                // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
                var relatedProducts = _productService.GetProductsByCategory(categoryId);

                if (relatedProducts.Any())
                {
                    // Hiển thị thông báo cho người dùng
                    var result = MessageBox.Show("Danh mục này có sản phẩm liên quan. Bạn có muốn xóa tất cả sản phẩm liên quan không?",
                                                  "Thông báo",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Xóa tất cả sản phẩm liên quan trước
                        foreach (var product in relatedProducts)
                        {
                            long productId = product.Productid; 
                            _productService.DeleteProduct(productId);
                        }
                    }
                    else
                    {
                        return; // Ngừng hàm nếu người dùng không muốn xóa
                    }
                }

                // Tiến hành xóa danh mục
                _categoryService.DeleteCategory(categoryId);
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                LoadCategories(); // Tải lại danh sách sau khi xóa
                MessageBox.Show("Xóa danh mục thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ID danh mục không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (long.TryParse(txt_Madm.Text, out long categoryId))
            {
                var existingCategory = _categoryService.GetCategoryById(categoryId);
                if (existingCategory != null)
                {
                    existingCategory.Categoryname = txt_Tendm.Text.Trim();
                    existingCategory.Parentcategoryid = (long)cbo_Madmcha.SelectedValue;
                    _categoryService.UpdateCategory(existingCategory);
                    LoadCategories();

                    // Chọn lại hàng tương ứng trong DataGridView
                    var index = categories.FindIndex(c => c.Categoryid == categoryId);
                    if (index >= 0)
                    {
                        dgv_danhsachdm.ClearSelection();
                        dgv_danhsachdm.Rows[index].Selected = true;
                        dgv_danhsachdm.CurrentCell = dgv_danhsachdm.Rows[index].Cells[0];
                    }
                }
                else
                {
                    MessageBox.Show("Danh mục không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("ID danh mục không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            var newCategory = new Productcategory
            {
                Categoryname = txt_Tendm.Text.Trim(),
                Parentcategoryid = (long)cbo_Madmcha.SelectedValue
            };

            _categoryService.AddCategory(newCategory);
            LoadCategories(); 
        }

        private Productcategory _lastSelectedRow;
        private void Dgv_danhsachdm_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgv_danhsachdm.SelectedRows.Count > 0)
            {
                // Lấy hàng đã chọn
                var selectedRow = dgv_danhsachdm.SelectedRows[0].DataBoundItem as Productcategory;

                if (selectedRow != null && selectedRow != _lastSelectedRow)
                {
                    // Cập nhật các ô văn bản với thông tin từ hàng đã chọn
                    txt_Madm.Text = selectedRow.Categoryid.ToString();
                    txt_Tendm.Text = selectedRow.Categoryname;
                    cbo_Madmcha.SelectedValue = selectedRow.Parentcategoryid;
                    _lastSelectedRow = selectedRow;
                }
            }
            else
            {
                txt_Madm.Clear();
                txt_Tendm.Clear();
                cbo_Madmcha.SelectedIndex = -1;
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            string searchTerm = txt_Timkiem.Text.Trim().ToLower(); 
            var categories = _categoryService.GetAllCategories();

            var filteredCategories = categories.Where(c => c.Categoryname.ToLower().Contains(searchTerm)).ToList();

            dgv_danhsachdm.DataSource = filteredCategories;

            if (filteredCategories.Count == 0)
            {
                MessageBox.Show("Không tìm thấy danh mục nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Frm_ProductCategory_Load(object? sender, EventArgs e)
        {
            LoadCategories();
        }
        private void LoadCategories()
        {
            categories = _categoryService.GetAllCategories().ToList(); 

            if (categories != null && categories.Count > 0)
            {
                dgv_danhsachdm.DataSource = categories;
             
                dgv_danhsachdm.AutoGenerateColumns = false; 
                dgv_danhsachdm.Columns.Clear(); 

                dgv_danhsachdm.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Categoryid",
                    HeaderText = "Mã Danh Mục",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdm.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Categoryname",
                    HeaderText = "Tên Danh Mục",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachdm.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Parentcategoryid",
                    HeaderText = "Mã Danh Mục Cha",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdm.ColumnHeadersHeight = 30;
                // Thay đổi màu nền và kiểu chữ
                dgv_danhsachdm.RowsDefaultCellStyle.BackColor = Color.LightBlue;
                dgv_danhsachdm.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
                dgv_danhsachdm.DefaultCellStyle.Font = new Font("Arial", 10);

                // Thiết lập cho tiêu đề
                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachdm.EnableHeadersVisualStyles = false; 

                LoadParentCategories();
            }
            else
            {
                MessageBox.Show("Không có danh mục nào để hiển thị.");
            }
        }
        private void LoadParentCategories()
        {
            var parentService = _parentService.GetAllParentCategories(); 
            // Cài đặt danh sách cho cbo_Madmcha
            cbo_Madmcha.DataSource = parentService;
            cbo_Madmcha.DisplayMember = "Parentcategoryname"; 
            cbo_Madmcha.ValueMember = "Parentcategoryid"; 
        }

    }
}
