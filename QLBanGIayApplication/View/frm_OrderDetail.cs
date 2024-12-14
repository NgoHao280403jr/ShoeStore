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
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ProductSizeService _productSizeService;
        private readonly QlShopBanGiayContext _context;
       public frm_OrderDetail(UserService userService)
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
            _userService = userService;
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
                var selectedOrderDetail = (Orderdetail)dgv_OD.Rows[e.RowIndex].DataBoundItem;
                txtMaCTHD.Text = selectedOrderDetail.Orderdetailid.ToString();
                cbbMaHD.SelectedItem = selectedOrderDetail.Order;
                cbbMaSP.SelectedValue = selectedOrderDetail.Productid;
                cbbSize.SelectedValue = selectedOrderDetail.Size;
                txtSL.Value = decimal.Parse(selectedOrderDetail.Quantity.ToString());
                txtGiaSP.Text = selectedOrderDetail.Unitprice.ToString();
            }
            else
            {
                MessageBox.Show("Chua co o nao duoc chon");
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                var orderDetails = _orderDetailService.GetAllOrderDetails();
                dgv_OD.DataSource = orderDetails.ToList();
                dgv_OD.Columns["Orderdetailid"].HeaderText = "Mã chi tiết đơn hàng";
                dgv_OD.Columns["Orderid"].HeaderText = "Mã đơn hàng";
                dgv_OD.Columns["Productid"].HeaderText = "Mã sản phẩm";
                dgv_OD.Columns["Size"].HeaderText = "Kích thước";
                dgv_OD.Columns["Quantity"].HeaderText = "Số lượng";
                dgv_OD.Columns["Unitprice"].HeaderText = "Đơn giá";
                dgv_OD.Columns["Subtotal"].HeaderText = "Thành tiền";
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
                cbbMaHD.SelectedIndex = -1;

                var products = _productService.GetAllProducts().ToList();
                cbbMaSP.DataSource = products;
                cbbMaSP.DisplayMember = "Productname";
                cbbMaSP.ValueMember = "Productid";
                cbbMaSP.SelectedIndex = -1;

                var productSize = _productSizeService.GetAllProductSizes().ToList();
                cbbSize.DataSource = productSize;
                cbbSize.DisplayMember = "Size";
                cbbSize.ValueMember = "Size";
                cbbSize.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            frm_Main main = new frm_Main(_userService); 
            this.Close();
            main.ShowDialog();
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaCTHD.Text) ||
                    cbbMaHD.SelectedItem == null ||
                    cbbMaSP.SelectedValue == null ||
                    cbbSize.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin để cập nhật chi tiết đơn hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                long orderDetailId = 0;
                long orderId = 0;
                long productId = 0;
                double unitPrice = 0.0;

                if (!long.TryParse(txtMaCTHD.Text, out orderDetailId))
                {
                    MessageBox.Show("Mã chi tiết đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!long.TryParse(cbbMaHD.SelectedValue.ToString(), out orderId))
                {
                    MessageBox.Show("Mã đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!long.TryParse(cbbMaSP.SelectedValue.ToString(), out productId))
                {
                    MessageBox.Show("Mã sản phẩm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(txtGiaSP.Text, out unitPrice))
                {
                    MessageBox.Show("Giá sản phẩm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var orderDetail = new Orderdetail
                {
                    Orderdetailid = orderDetailId,
                    Orderid = orderId,
                    Productid = productId,
                    Size = cbbSize.SelectedValue.ToString(),
                    Quantity = (int)txtSL.Value,
                    Unitprice = unitPrice
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
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết đơn hàng?",
                                                        "Xác nhận",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        var orderId = long.Parse(dgv_OD.SelectedRows[0].Cells["Orderid"].Value.ToString());
                        var productId = long.Parse(dgv_OD.SelectedRows[0].Cells["Productid"].Value.ToString());

                        _orderDetailService.DeleteOrderDetail(orderId, productId);
                        MessageBox.Show("Xóa chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetails();
                    }
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
                if (cbbMaHD.SelectedValue == null || cbbMaSP.SelectedValue == null || cbbSize.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ thông tin cho chi tiết đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var orderDetail = new Orderdetail
                {
                    Orderid = long.TryParse(cbbMaHD.SelectedValue.ToString(), out var orderid) ? orderid : 0, 
                    Productid = long.TryParse(cbbMaSP.SelectedValue.ToString(), out var productid) ? productid : 0, 
                    Size = cbbSize.SelectedValue.ToString(), 
                    Quantity = (int)txtSL.Value,
                    Unitprice = double.TryParse(txtGiaSP.Text, out var unitprice) ? unitprice : 0.0 
                };

                if (orderDetail.Orderid == 0 || orderDetail.Productid == 0 || orderDetail.Unitprice == 0)
                {
                    MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại thông tin đã nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
