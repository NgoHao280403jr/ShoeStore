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

namespace QLBanGiay_Application.View
{
    public partial class frm_ParentProduct : Form
    {
        private readonly UserService _userService;
        private readonly ParentService _parentService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private List<Parentproductcategory> parents;
        private readonly QlShopBanGiayContext _context;
        public frm_ParentProduct(UserService userService)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _context = new QlShopBanGiayContext();
            _parentService = new ParentService(new ParentCategoryRepository(_context));
            this.Load += Frm_ParentProduct_Load;
            this.dgv_danhsachdm.SelectionChanged += Dgv_danhsachdm_SelectionChanged;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.btn_Thoat.Click += Btn_Thoat_Click;

            _context = new QlShopBanGiayContext();
            _parentService = new ParentService(new ParentCategoryRepository(_context));
            _productService = new ProductService(new ProductRepository(_context));
            _categoryService = new CategoryService(new CategoryRepository(_context));
            _userService = userService;
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main(_userService);
            mainForm.Show();
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            string searchTerm = txt_Timkiem.Text.Trim().ToLower();  
            var allParents = _parentService.GetAllParentCategories();
            var filteredParents = allParents
                .Where(p => p.Parentcategoryname.ToLower().Contains(searchTerm)) 
                .ToList();

            dgv_danhsachdm.DataSource = filteredParents;
            if (filteredParents.Count == 0)
            {
                MessageBox.Show("Không tìm thấy danh mục cha nào với từ khóa '" + searchTerm + "'.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (long.TryParse(txt_Madm.Text, out long categoryId))
            {
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này cùng với các danh mục con không?",
                                                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Lấy và xóa tất cả sản phẩm thuộc danh mục này
                            var relatedProducts = _productService.GetProductsByCategory(categoryId);
                            if (relatedProducts.Any())
                            {                              
                                var result = MessageBox.Show("Danh mục này có sản phẩm liên quan. Bạn có muốn xóa tất cả sản phẩm liên quan không?",
                                                              "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    foreach (var product in relatedProducts)
                                    {
                                        _productService.DeleteProduct(product.Productid);
                                    }
                                }
                            }

                            // Xóa danh mục cha
                            _categoryService.DeleteCategory(categoryId);
                            _context.SaveChanges();
                            transaction.Commit();

                            LoadParentCategories(); 
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Đã xảy ra lỗi khi xóa: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
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
                var existingCategory = _parentService.GetParentCategoryById(categoryId);
                if (existingCategory != null)
                {
                    existingCategory.Parentcategoryname = txt_Tendm.Text.Trim();
                    _parentService.UpdateParentCategory(existingCategory);
                    LoadParentCategories();

                    var index = parents.FindIndex(p => p.Parentcategoryid == categoryId);
                    if (index >= 0)
                    {
                        dgv_danhsachdm.ClearSelection();
                        dgv_danhsachdm.Rows[index].Selected = true;
                        dgv_danhsachdm.CurrentCell = dgv_danhsachdm.Rows[index].Cells[0];
                    }
                }
                else
                {
                    MessageBox.Show("Danh mục cha không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("ID danh mục không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            var newCategory = new Parentproductcategory
            {
                Parentcategoryname = txt_Tendm.Text.Trim()
            };

            _parentService.AddParentCategory(newCategory);
            LoadParentCategories();
        }

        private Parentproductcategory _lastSelectedRow;
        private void Dgv_danhsachdm_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgv_danhsachdm.SelectedRows.Count > 0)
            {
                var selectedRow = dgv_danhsachdm.SelectedRows[0].DataBoundItem as Parentproductcategory;

                if (selectedRow != null && selectedRow != _lastSelectedRow)
                {
                    txt_Madm.Text = selectedRow.Parentcategoryid.ToString();
                    txt_Tendm.Text = selectedRow.Parentcategoryname;

                    _lastSelectedRow = selectedRow;
                }
            }
            else
            {
                txt_Madm.Clear();
                txt_Tendm.Clear();
            }
        }

        private void Frm_ParentProduct_Load(object? sender, EventArgs e)
        {
            LoadParentCategories();
        }
        private void LoadParentCategories()
        {
            parents = _parentService.GetAllParentCategories().ToList();

            if (parents != null && parents.Count > 0)
            {
                dgv_danhsachdm.DataSource = parents;

                dgv_danhsachdm.AutoGenerateColumns = false;
                dgv_danhsachdm.Columns.Clear();

                dgv_danhsachdm.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ParentCategoryId",
                    HeaderText = "Mã Danh Mục Cha",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdm.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ParentCategoryName",
                    HeaderText = "Tên Danh Mục Cha",
                    Width = 220,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });
                dgv_danhsachdm.ColumnHeadersHeight = 30;
                dgv_danhsachdm.RowsDefaultCellStyle.BackColor = Color.LightYellow;
                dgv_danhsachdm.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
                dgv_danhsachdm.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.BackColor = Color.LightYellow;
                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv_danhsachdm.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachdm.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có danh mục cha nào để hiển thị.");
            }
        }
    }
}
