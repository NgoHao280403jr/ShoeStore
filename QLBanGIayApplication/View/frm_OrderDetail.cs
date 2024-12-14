using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
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

namespace QLBanGiay_Application.View
{
    public partial class frm_OrderDetail : Form
    {
        private readonly OrderdetailService _orderDetailService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly ProductSizeService _productSizeService;
        private readonly QlShopBanGiayContext _context;
        public frm_OrderDetail()
        {
            InitializeComponent();
            btn_Them.Click += Btn_Them_Click;
            btn_Xoa.Click += Btn_Xoa_Click;
            btn_Capnhat.Click += Btn_Capnhat_Click;
            btn_Thoat.Click += Btn_Thoat_Click;
            dgv_OD.CellClick += Dgv_OD_CellClick;
            btnSearch.Click += BtnSearch_Click;
            this.Load += Frm_OrderDetail_Load;


            _context = new QlShopBanGiayContext();
            _productService = new ProductService(new ProductRepository(_context));
            _orderService = new OrderService(new OrderRepository(_context));
            _orderDetailService = new OrderdetailService(new OrderdetailRepository(_context));
            _productSizeService = new ProductSizeService(new ProductSizeRepository(_context));
        }

        private void Frm_OrderDetail_Load(object? sender, EventArgs e)
        {
            LoadOrderDetails();
            LoadComboBoxes();
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            var order = _orderDetailService.SearchOrderDetails(keyword);
            if (order != null)
            {
                dgv_OD.DataSource = order;
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn.");
            }
        }

        private void Dgv_OD_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_OD.Rows[e.RowIndex];
                cbbMaHD.SelectedItem = row.Cells["Orderid"].Value.ToString();
                cbbMaSP.SelectedValue = row.Cells["Productid"].Value.ToString();
                cbbSize.SelectedItem = row.Cells["Size"].Value.ToString();
                txtSL.Value = decimal.Parse(row.Cells["Quantity"].Value.ToString());
                txtGiaSP.Text = row.Cells["Unitprice"].Value.ToString();
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                var orderDetails = _orderDetailService.GetAllOrderDetails();
                dgv_OD.DataSource = orderDetails.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadComboBoxes()
        {
            try
            {
                var orders = _orderService.GetAllOrders().ToList();
                cbbMaHD.DataSource = orders;
                cbbMaHD.DisplayMember = "Orderid";
                cbbMaHD.ValueMember = "Orderid";

                var products = _productService.GetAllProducts().ToList();
                cbbMaSP.DataSource = products;
                cbbMaSP.DisplayMember = "Productname";
                cbbMaSP.ValueMember = "Productid"; 

                var productSize= _productSizeService.GetAllProductSizes().Distinct().ToList();
                cbbSize.DataSource = productSize;
                cbbSize.DisplayMember = "Size";
                cbbSize.ValueMember = "ProductSizeId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            try
            {
                var orderDetail = new Orderdetail
                {
                    Orderdetailid = long.Parse(txtMaCTHD.Text),
                    Orderid = long.Parse(cbbMaHD.SelectedItem.ToString()),
                    Productid = long.Parse(cbbMaSP.SelectedValue.ToString()),
                    Size = cbbSize.SelectedValue.ToString(),
                    Quantity = (int)txtSL.Value,
                    Unitprice = double.Parse(txtGiaSP.Text)
                };

                _orderDetailService.UpdateOrderDetail(orderDetail);
                MessageBox.Show("Cập nhật chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrderDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgv_OD.SelectedRows.Count > 0)
                {
                    var orderId = long.Parse(dgv_OD.SelectedRows[0].Cells["Orderid"].Value.ToString());
                    var productId = long.Parse(dgv_OD.SelectedRows[0].Cells["Productid"].Value.ToString());

                    _orderDetailService.DeleteOrderDetail(orderId, productId);
                    MessageBox.Show("Xóa chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrderDetails();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            try
            {
                var orderDetail = new Orderdetail
                {
                    Orderid = long.Parse(cbbMaHD.SelectedItem.ToString()),
                    Productid = long.Parse(cbbMaSP.SelectedValue.ToString()),
                    Size = cbbSize.SelectedValue.ToString(),
                    Quantity = (int)txtSL.Value,
                    Unitprice = double.Parse(txtGiaSP.Text)
                };

                _orderDetailService.AddOrderDetail(orderDetail);
                MessageBox.Show("Thêm chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrderDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
